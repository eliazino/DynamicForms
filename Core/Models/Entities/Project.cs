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
        public Project(ProjectDTO data) {
            if (isNullOrEmpty(data.projectName, data.projectDetails))
                throw new InputError("All fields are required");
            this.id = Cryptography.CharGenerator.genID(8, Cryptography.CharGenerator.characterSet.HEX_STRING);
            this.projectDetails = data.projectDetails;
            this.projectName = data.projectName;
            this.dateCreated = Utilities.getTodayDate().unixTimestamp;
            this.status = 1;
        }
        public Project() { }
    }
}
