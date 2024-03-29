﻿using Microsoft.Extensions.Configuration;

namespace PersonalFinance.Utils;

public class DatabaseUtils
{
    private readonly IConfiguration configuration;

    public DatabaseUtils(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IConfiguration GetConfiguration()
    {
        return configuration;
    }

    public string GetConnectionString()
    {
        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        if (env == "Production")
        {
            var url = new Uri(Environment.GetEnvironmentVariable("DATABASE_URL"));
            var host = url.Host;
            var database = url.LocalPath.TrimStart('/');
            var username = url.UserInfo.Split(':')[0];
            var password = url.UserInfo.Split(':')[1];
            return $"Host={host};Database={database};Username={username};Password={password};SSL Mode=Disable;";
        }

        return configuration.GetConnectionString("PostgreSQL");
    }
}