using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Entities {
    public class ProjectOwners : BaseEntity {
        public string email { get; protected set; }
        public HashSet<string> projects { get; protected set; } = new HashSet<string>();
        public ProjectOwners() { }
        public ProjectOwners(string projectID) {
            this.projects.Add(projectID);
        }
    }
}
