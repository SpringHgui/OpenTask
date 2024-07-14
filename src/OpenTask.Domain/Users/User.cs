// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using OpenTask.Domain.Base.Repositorys;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace OpenTask.Domain.Users
{
    public class User : Aggregate<long>
    {
        public long Id { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }

        public required DateTime CreatedAt { get; set; }

        public bool CheckPass(string password)
        {
            return Password == MD5Encrypt64(password);
        }

        public string MD5Encrypt64(string password)
        {
            string cl = $"BaoXin{password}KeJi";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }

        private const string Key = "User";

        public string GenToken(string SigningKey)
        {
            Claim[] claims = new[] {
                    new Claim(Key,  JsonSerializer.Serialize(new CurrentUser{
                        UserId = Id,
                        UserName = Username,
                        //Teams = teams
                    })),
            };

            return "Bearer " + UserService.GenerateToken(claims, SigningKey);
        }
    }
}
