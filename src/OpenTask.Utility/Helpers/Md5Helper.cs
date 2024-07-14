// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using System.Security.Cryptography;
using System.Text;

namespace OpenTask.Utility.Helpers
{
    public class Md5Helper
    {
        public static string MD5Encrypt64(string password)
        {
            string cl = $"BaoXin{password}KeJi";
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }
    }
}
