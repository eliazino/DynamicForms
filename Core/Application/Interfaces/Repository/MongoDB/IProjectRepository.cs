using Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repository.MongoDB {
    public interface IProjectRepository {
        Task<bool> createProject(Project project);
        Task<List<Project>> getProject();
        Task<List<Project>> getProject(string id);
        Task<bool> endProject(string id);
        Task<bool> updateProject(Project project);
        Task<List<Project>> getProject(List<string> projectID);
    }
}
