// Licensed under the MIT License (the "License").
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//     https://github.com/SpringHgui/OpenTask/blob/master/LICENSE
// Copyright (c) 2024 Gui.H

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OpenTask.Domain.Users
{
    public class UserService
    {
        public static string GenerateToken(IEnumerable<Claim> claims, string Secret, int expiresMinutes = 60, string? issuer = null, string? audience = null)
        {
            SymmetricSecurityKey key = new(Convert.FromBase64String(Secret));
            SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityToken = new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public static ClaimsPrincipal? GetPrincipal(string token, string secret, bool validateLifetime = true)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secret))
            {
                return null;
            }

            if (token.StartsWith("Bearer"))
            {
                token = token.Replace("Bearer ", string.Empty);
            }

            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                {
                    return null;
                }

                TokenValidationParameters parameters = new()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = validateLifetime,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secret))
                };

                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}