using Core.Application.DTOs.Configurations;
using Core.Application.DTOs.Local;
using Core.Application.DTOs.Response;
using Core.Application.Interfaces.Auth;
using Core.Application.Interfaces.Mail;
using Core.Application.Interfaces.Repository.MongoDB;
using Core.Application.Interfaces.UseCases;
using Core.Models.Entities;
using Core.Shared;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Core.Application.DTOs.Response.ResponseFormat;

namespace Core.Application.Services.UseCases {
    [RegisterAsScoped]
    public class UserUseCase : IUserUseCase {
        private readonly IUserRepository _userRepo;
        private readonly IIdentityMngr _identity;
        private readonly IProjectRepository _project;
        private readonly IEmailService _mailService;
        public UserUseCase(IUserRepository user, IOptionsMonitor<SystemVariables> sysVar, IIdentityMngr identity, IProjectRepository projectRepo, IEmailService mail) {
            this._userRepo = user;
            this._identity = identity;
            this._project = projectRepo;
            this._mailService = mail;
        }
        public async Task<RawResponse> inviteToProject(string email, string projectID, bool owner = false) {
            ResponseFormat response = new ResponseFormat();
            if (!_identity.valid)
                return response.failed("Access Denied! Authentication is invalid");
            var user = _identity.getProfile<UserDTO>();
            var userObj = await _userRepo.getUser(user.email);
            var project = await _project.getProject(projectID);
            if (project.Count < 1)
                return response.failed("Invalid Project. Project does not exist");
            if(!project[0].owners.Contains(user.email))
                return response.failed("Invalid Project. You don't own this project");
            if(!userObj[0].projects.Contains(projectID))
                return response.failed("Invalid Project. You have no admin right this project");
            string[] emails = email.Split(',');
            foreach(string em in emails) {
                var thisUsr = await _userRepo.getUser(em);
                Users invitee = new Users(em);
                bool created = false;
                if (thisUsr.Count > 0) {
                    invitee = thisUsr[0];
                    created = true;
                }                    
                invitee.addProject(projectID);
                if (created) {
                    await _userRepo.updateUser(invitee);
                } else {
                    await _userRepo.createUser(invitee);
                }
                if (owner)
                    project[0].addOwner(em);                
            }
            await _project.updateProject(project[0]);
            return response.success("Successful");
        }

        public async Task<RawResponse> login(string email, string code = null) {
            ResponseFormat response = new ResponseFormat();
            var user = await _userRepo.getUser(email);
            if (string.IsNullOrEmpty(code)) {
                Users userAcc = new Users(email);
                if (user.Count > 0) {
                    userAcc = user[0];
                    userAcc.newLogin();
                    await _userRepo.updateUser(userAcc);
                } else {
                    await _userRepo.createUser(userAcc);
                }                
                string message = "<blockquote><h3>Use the code below to complete your login:</h3><h4>"+userAcc.code+"</h4></blockquote>";
                await _mailService.send(new MailEnvelope { body = message, subject = "Login to your OpenForms Account", toAddress = new string[] { email }, toName = new string[] { email } });
                return response.success("Please check your email for your limited use password", new { code = string.Empty });
            }            
            if (user.Count < 1)
                return response.failed("Invalid request. Account not found");
            if(user[0].code != code)
                return response.failed("Invalid request. Invalid Code");
            if (user[0].expiry < Utilities.getTodayDate().unixTimestamp)
                return response.failed("Invalid request. Code has expired");
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("email", email);
            string token = _identity.getJWTIdentity(data, 1440*10);
            return response.success("Completed login", new { auth = token });
        }

        public async Task<RawResponse> removeFromProject(string email, string projectID) {
            ResponseFormat response = new ResponseFormat();
            if (!_identity.valid)
                return response.failed("Access Denied! Authentication is invalid");
            var user = _identity.getProfile<UserDTO>();
            var project = await _project.getProject(projectID);
            if (project.Count < 1)
                return response.failed("Invalid Project. Project does not exist");
            if (!project[0].owners.Contains(user.email))
                return response.failed("Invalid Project. You don't own this project");
            var userObj = await _userRepo.getUser(user.email);
            if (!userObj[0].projects.Contains(projectID))
                return response.failed("Invalid Project. You have no admin right this project");
            var profile = await _userRepo.getUser(email);
            if (profile.Count < 1)
                return response.failed("User was not found");
            project[0].removeOwner(email);
            await _project.updateProject(project[0]);
            profile[0].projects.Remove(projectID);
            await _userRepo.updateUser(profile[0]);
            return response.success("Completed action");
        }
    }
}
