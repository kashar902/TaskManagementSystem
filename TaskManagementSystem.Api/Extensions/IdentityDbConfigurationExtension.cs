﻿using Microsoft.AspNetCore.Identity;
using TaskManagementSystem.DAL.Data;

namespace TaskManagementSystem.Api.Extensions;
public static class IdentityDbConfigurationExtension
{
    public static void AddIdentityDb(this IServiceCollection services) 
    {
        services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<DataContext>();

    }
}
