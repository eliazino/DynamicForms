using Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces.Repository.MongoDB {
    public interface IUserRepository {
        Task<List<Users>> getUser(string email);
        Task<bool> createUser(Users user);
        Task<bool> updateUser(Users user);
    }

    public interface IUserProjectRepository {
        Task<List<ProjectOwners>> getUser(string email);
        Task<bool> createUser(ProjectOwners user);
        Task<bool> updateUser(ProjectOwners user);

    }
}
