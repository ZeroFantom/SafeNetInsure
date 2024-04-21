using System.Collections.ObjectModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SafeNetInsureWebApp.Context;
using SafeNetInsureWebApp.Data.Authorization;
using SafeNetInsureWebApp.Models;
using static SafeNetInsureWebApp.Data.Services.ServiceDataPolicy;

namespace SafeNetInsureWebApp.Data.Services;

public class ServiceDataUser
{
    private readonly InsureDbContext _dbContext;

    public ServiceDataUser(InsureDbContext dbContext, IHttpContextAccessor contextAccessor)
    {
        _dbContext = dbContext;

        if (contextAccessor.HttpContext?.User.GetClaimIssuer(ClaimTypes.Sid)?.Value is { } id)
            User = _dbContext.Users.Include(x => x.UserInfo).Include(x => x.RoleHasUser)
                .FirstOrDefault(x => x.IdUser.ToString() == id)!;
    }

    public User User { get; set; } = new();
    public Policy Policy { get; set; } = new();

    public ObservableCollection<Policy> PoliciesUser => new(Policies.Where(x => x.UserIdUser == User.IdUser).ToList());

    [Authorize]
    public async Task DeleteModel<T>(T model)
    {
        _dbContext.Remove(model);
        await _dbContext.SaveChangesAsync();
    }

    [Authorize]
    public async Task UpdateModel()
    {
        await _dbContext.SaveChangesAsync();
    }

    [Authorize]
    public async Task<T> InsertModel<T>(T objNewItem)
    {
        await _dbContext.AddAsync(objNewItem);
        await _dbContext.SaveChangesAsync();
        return objNewItem;
    }
}