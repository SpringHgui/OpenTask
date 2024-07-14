// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using AutoMapper;
using Dommel;
using OpenTask.Domain.Users;
using OpenTask.Persistence.Entitys;
using OpenTask.Persistence.Extensions;
using OpenTask.Persistence.Models;

namespace OpenTask.Persistence.Repositorys
{
    [Inject]
    public class UserRepository : IUserRepository
    {
        private readonly DapperdbContext dbContext;
        private readonly IMapper mapper;

        public UserRepository(DapperdbContext dbContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public User? Find(long id)
        {
            using var conn = dbContext.CreateConnection();
            OtUser? user = conn.Select<OtUser>(x => x.Id == id).FirstOrDefault();
            return mapper.Map<User>(user);
        }

        public User? FindByUserName(string userName)
        {
            using var conn = dbContext.CreateConnection();
            OtUser? user = conn.Select<OtUser>(x => x.UserName == userName).FirstOrDefault();
            return mapper.Map<User>(user);
        }

        //public bool CheckPassWord(string userName, string passWord)
        //{
        //    var passWordHashed = ScUser.MD5Encrypt64(passWord);
        //    var ok = dbContext.ScUsers.Any(x => x.UserName == userName && x.Password == passWordHashed);
        //    return ok;
        //}

        public void Remove(User user)
        {
            using var conn = dbContext.CreateConnection();
            _ = conn.Delete(mapper.Map<OtUser>(user));
        }

        public long Save(User user)
        {
            using var conn = dbContext.CreateConnection();
            _ = conn.Insert(mapper.Map<OtUser>(user));
            return user.Id;
        }
    }
}
