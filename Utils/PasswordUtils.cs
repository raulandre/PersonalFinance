﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using PersonalFinance.Models;

namespace PersonalFinance.Utils;

public static class PasswordUtils
{
    public static void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
    {
        using (var hmac = new HMACSHA512())
        {
            salt = hmac.Key;
            hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }

    public static bool VerifyPasswordHash(User user, string password, byte[] hash, byte[] salt)
    {
        using (var hmac = new HMACSHA512(user.PasswordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(hash);
        }
    }

    public static string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
        };

        var superSecretKey = GetSuperSecretKey();

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(superSecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string GetSuperSecretKey()
        => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") is var env
            && env == "Development" || env is null
            ? string.Concat(Enumerable.Repeat("nyan", 128))
            : Environment.GetEnvironmentVariable("SUPER_SECRET_TOKEN");
}
