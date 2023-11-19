using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using SafeNetInsureWebApp.Context;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Services;

public class ServiceAdministrationData
{
    private readonly InsureDbContext _dbContext;

    public ServiceAdministrationData(InsureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public static ObservableCollection<Role> Roles { get; set; } = new();
    public static ObservableCollection<User> Users { get; set; } = new();
    public static ObservableCollection<UserInfo> UserInfos { get; set; } = new();
    public static ObservableCollection<RoleHasUser> RoleHasUsers { get; set; } = new();

    [Authorize(Policy = "AdminPolicy")]
    public async Task GetUsers()
    {
        Users = new ObservableCollection<User>(await _dbContext.Users.ToListAsync());
    }

    [Authorize(Policy = "AdminPolicy")]
    public async Task GetUserInfos()
    {
        UserInfos = new ObservableCollection<UserInfo>(await _dbContext.UserInfos.ToListAsync());
    }

    [Authorize(Policy = "AdminPolicy")]
    public async Task GetRoleHasUser()
    {
        RoleHasUsers = new ObservableCollection<RoleHasUser>(await _dbContext.RoleHasUsers.ToListAsync());
    }

    [AllowAnonymous]
    public async Task GetRoles()
    {
        if (new ObservableCollection<Role>(await _dbContext.Roles.ToListAsync()) is not { } roles)
            return;

        Roles = roles.Count == 0
            ? await DefaultRolesAdd(roles)
            : roles;
    }

    [AllowAnonymous]
    private async Task<ObservableCollection<Role>> DefaultRolesAdd(ObservableCollection<Role> roles)
    {
        roles.AddRange(new List<Role>
        {
            new()
            {
                Title = "Администратор",
                Description = "Администрация страховой компании!"
            },
            new()
            {
                Title = "Пользователь",
                Description = "Клиент страховой компании!"
            }
        });
        await _dbContext.Roles.AddRangeAsync(roles);
        await _dbContext.SaveChangesAsync();
        return roles;
    }

    [AllowAnonymous]
    public async Task<User?> Login(User client)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == client.Email && x.Password == client.Password);
        return user ?? null;
    }

    [AllowAnonymous]
    public async Task Registration(User client)
    {
        await _dbContext.Users.AddAsync(client);
        await _dbContext.SaveChangesAsync();
    }
}