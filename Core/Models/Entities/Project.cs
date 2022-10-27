using Core.Application.DTOs.Local;
using Core.Application.DTOs.Request;
using Core.Exceptions;
using Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Entities {
    public class Project : BaseEntity {
        public string id { get; protected set; }
        public string projectName { get; protected set; }
        public string projectDetails { get; protected set; }
        public long dateCreated { get; protected set; }
        public int status { get; protected set; }
        public string clientID { get; protected set; }
        public HashSet<string> owners { get; protected set; }
        public Project(ProjectDTO data, UserDTO user) {
            if (isNullOrEmpty(data.projectName, data.projectDetails))
                throw new InputError("All fields are required");
            this.id = Cryptography.CharGenerator.genID(8, Cryptography.CharGenerator.characterSet.HEX_STRING);
            this.projectDetails = data.projectDetails;
            this.projectName = data.projectName;
            this.dateCreated = Utilities.getTodayDate().unixTimestamp;
            this.status = 1;
            this.clientID = user.email;
            this.owners = new HashSet<string> { user.email };
        }
        public void removeOwner(string email) {
            this.owners.Add(email);
        }
        public void addOwner(string email) {
            this.owners.Add(email);
        }
        public Project() { }
    }
}
