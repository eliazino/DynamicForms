using Core.Exceptions;
using Core.Models.Entities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Test {
    [TestFixture]
    public class ProjectTest {

        [TestCase]
        public void TestConstructor() {
            var dto = new Application.DTOs.Request.ProjectDTO {
                projectName = "Sample",
                projectDetails = "Details"
            };
            Project project = new Project(
                dto, new Application.DTOs.Local.UserDTO() { }
                ); ;
            Assert.IsTrue(project.projectName == dto.projectName, "Project Name should be set");
            Assert.IsTrue(project.projectDetails == dto.projectDetails, "Project Details should be set");
            Assert.IsTrue(project.status == 1, "Default status is 1");
        }

        [TestCase]
        public void TestConstructorThrowsError() {
            var dto = new Application.DTOs.Request.ProjectDTO {
                projectName = "Sample"
            };
            Assert.Throws<InputError>(()=> {
                new Project(dto, new Application.DTOs.Local.UserDTO { });
            });
        }
    }
}
