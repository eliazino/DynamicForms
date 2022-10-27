using Core.Application.Interfaces.Repository.MongoDB;
using Core.Models.Entities;
using Infrastructure.Interfaces.MongoDB;
using Infrastructure.Persistence;
using NetCore.AutoRegisterDi;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.MongoDB {
    [RegisterAsScoped]
    public class UserRepository : Repository<Users>, IUserRepository, IRepository<Users> {
        public UserRepository(IMongoDb db) : base(db) { }
        public async Task<bool> createUser(Users user) {
            return await create(user);
        }

        public async Task<List<Users>> getUser(string email) {
            Expression<Func<Users, bool>> condition = F => F.email == email;
            return (List<Users>)await getByCondition(condition);
        }

        public async Task<bool> updateUser(Users user) {
            Expression<Func<Users, bool>> condition = F => F.email == user.email;
            return await update(user, condition);
        }
    }
}
