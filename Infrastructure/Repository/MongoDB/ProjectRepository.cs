using Core.Application.Interfaces.Repository.MongoDB;
using Core.Models.Entities;
using Infrastructure.Persistence;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class ProjectRepository : Repository<Project>, IProjectRepository, Interfaces.MongoDB.IRepository<Project> {
        public ProjectRepository(IMongoDb db) : base(db) { }
        public async Task<bool> createProject(Project project) {
            return await create(project);
        }

        public async Task<bool> endProject(string id) {
            Expression<Func<Project, bool>> condition = F => F.id == id;
            return await update(new { status = 1 }, condition);
        }

        public async Task<List<Project>> getProject() {
            return (List<Project>)await getAll();
        }

        public async Task<List<Project>> getProject(string id) {
            Expression<Func<Project, bool>> condition = F => F.id == id;
            return (List<Project>)await getByCondition(condition);
        }
    }
}
