using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SafeNetInsureWebApp.Context;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Services;

public class ServiceDataPolicy
{
    private readonly InsureDbContext _dbContext;

    public ServiceDataPolicy(InsureDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public static ObservableCollection<Policy> Policies { get; set; } = new();
    public static ObservableCollection<ConditionPolicy> ConditionPolicies { get; set; } = new();
    public static ObservableCollection<ObjectInsurance> ObjectInsurances { get; set; } = new();

    [AllowAnonymous]
    public async Task GetPolicies()
    {
        Policies = new ObservableCollection<Policy>(await _dbContext.Policies.Include(x => x.UserIdUserNavigation)
            .Include(x => x.ConditionPolicyIdConditionPolicies).Include(x => x.ObjectInsuranceIdObjectInsurances)
            .ToListAsync());
    }

    [Authorize(Policy = "AdminPolicy")]
    public async Task GetCPolicies()
    {
        ConditionPolicies = new ObservableCollection<ConditionPolicy>(await _dbContext.ConditionPolicies.ToListAsync());
    }

    [Authorize(Policy = "AdminPolicy")]
    public async Task GetOPolicies()
    {
        ObjectInsurances = new ObservableCollection<ObjectInsurance>(await _dbContext.ObjectInsurances.ToListAsync());
    }
}