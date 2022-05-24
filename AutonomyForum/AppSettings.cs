﻿using System.Text;

namespace AutonomyForum;

public class AppSettings
{
    public string? DatabaseConnectionString { get; set; }
    public byte[] JwtSigningSecretKey => Encoding.UTF8.GetBytes(jwtSigningSecretKey!);
    public string? AdminPassword { get; set; }
    public string? ModeratorPassword { get; set; }

    private const string DefaultJwtSigningSecretKey = "f892905d-35ac-428c-afd1-1b8bb13384ea";
    private string? jwtSigningSecretKey;

    public static AppSettings CreateFrom(ConfigurationManager configuration)
    {
        var databaseConnectionString = Environment.GetEnvironmentVariable("DatabaseConnectionString") ??
                                       configuration.GetConnectionString("LocalDatabaseConnectionString");
        var jwtSigningSecretKey = Environment.GetEnvironmentVariable("JwtSigningSecretKey");
        var adminPassword = Environment.GetEnvironmentVariable("AdminPassword") ??
                            configuration.GetValue<string>("Admin:Password");
        var moderatorPassword = Environment.GetEnvironmentVariable("ModeratorPassword") ??
                            configuration.GetValue<string>("Moderator:Password");
        return new AppSettings
        {
            DatabaseConnectionString = databaseConnectionString,
            jwtSigningSecretKey = jwtSigningSecretKey ?? DefaultJwtSigningSecretKey,
            AdminPassword = adminPassword,
            ModeratorPassword = moderatorPassword
        };
    }
}