using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.DTOs.Request {
    public class ProjectDTO {
        public string id { get; set; }
        public string projectName { get; set; }
        public string projectDetails { get; set; }
        public long dateCreated { get; set; }
        public int status { get; set; }
    }
}
