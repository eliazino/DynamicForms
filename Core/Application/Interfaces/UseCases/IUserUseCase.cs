using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Core.Application.DTOs.Response.ResponseFormat;

namespace Core.Application.Interfaces.UseCases {
    public interface IUserUseCase {
        Task<RawResponse> login(string email, string code = null);
        Task<RawResponse> inviteToProject(string email, string projectID, bool owner = false);
        Task<RawResponse> removeFromProject(string email, string projectID);
    }
}
