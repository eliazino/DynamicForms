using Core.Exceptions;
using Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Entities {
    public class Users : BaseEntity {
        public string id { get; protected set; }
        public string email { get; protected set; }
        public string code { get; protected set; }
        public long expiry { get; protected set; }
        public HashSet<string> projects { get; protected set; } = new HashSet<string>();
        public Users(string email) {
            this.id = Cryptography.CharGenerator.genID(12, Cryptography.CharGenerator.characterSet.HEX_STRING);
            this.email = email;
            this.code = Cryptography.CharGenerator.genID(6, Cryptography.CharGenerator.characterSet.NUMERIC);
            this.expiry = Utilities.getTodayDate().unixTimestamp + (60 * 20);
        }
        public Users() { }
        public void newLogin() {
            if (string.IsNullOrEmpty(this.email))
                throw new LogicError("Attempt to create login for invalid user failed");
            this.code = Cryptography.CharGenerator.genID(6, Cryptography.CharGenerator.characterSet.NUMERIC);
            this.expiry = Utilities.getTodayDate().unixTimestamp + (60 * 20);
        }
        public void addProject(string projectID) {
            projects.Add(projectID);
        }
    }
}
