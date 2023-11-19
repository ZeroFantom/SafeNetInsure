using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SafeNetInsureWebApp.Context;
using SafeNetInsureWebApp.Models;

namespace SafeNetInsureWebApp.Data.Authorization;

public static class ExpansionClaimsPrincipal
{
    private static string _userIssuer = "SafeNet";

    /// <summary>
    ///     Создание авторизационных данных для определённой схемы, в виде идентифицированного набора клеймов.
    /// </summary>
    /// <param name="user"></param>
    public static ClaimsIdentity CreateUserClaimIdentityIssuer(User user,
        string shema = CookieAuthenticationDefaults.AuthenticationScheme)
    {
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new(ClaimTypes.Sid, user.IdUser.ToString(), nameof(Int32), _userIssuer),
            new(ClaimTypes.Name, $"{user.UserInfo.FirstName} {user.UserInfo.LastName}", nameof(String), _userIssuer),
            new(ClaimTypes.UserData, JsonConvert.SerializeObject(user), nameof(User), _userIssuer)
        }, shema);

        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, user.RoleHasUser.RoleIdRoleNavigation.Title,
            nameof(String), _userIssuer));

        return claimsIdentity;
    }

    /// <summary>
    ///     Задаёт поставщика аккаунта для данного запуска.
    /// </summary>
    /// <param name="httpContext"></param>
    /// <param name="issuer">Поставщик</param>
    public static HttpContext SetIssuer(this HttpContext httpContext, string issuer = "NewCustom")
    {
        _userIssuer = issuer;
        return httpContext;
    }

    #region Методы сверки даннных внутри клеймов

    /// <summary>
    ///     Метод проверяет авторизационные данные пользователя в веб приложении.
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="contextUse">Использовать ли запрос к базе данных или просто убедиться в наличии данных.</param>
    /// <returns></returns>
    public static async Task<bool> IsAuthUser(this ClaimsPrincipal user, InsureDbContext dbContext,
        bool contextUse = false)
    {
        try
        {
            var idUser = user.GetClaimIssuer(ClaimTypes.Sid);
            var userData = user.GetClaimIssuer<User>(ClaimTypes.UserData);

            if (idUser is null || userData is null) return false;
            if (!contextUse) return true;

            return await dbContext.Users.AnyAsync(x => userData.Email == x.Email && userData.Password == x.Password);
        }
        catch (Exception)
        {
            return false;
        }
    }

    #endregion

    #region Методы извлечения клеймов информации

    public static Dictionary<string, List<string>>? GetClaimIssuer(this ClaimsPrincipal claimsPrincipal)
    {
        var claims = claimsPrincipal.Claims.Where(x => x.Issuer == _userIssuer).ToList();
        return claims.Count != 0
            ? claims.ToDictionary(x => x.Type, x => claims.Where(a => a.Type == x.Type).Select(b => b.Value).ToList())
            : null;
    }

    public static Claim? GetClaimIssuer(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Issuer == _userIssuer && x.Type == claimType);
        return claim ?? null;
    }

    public static T? GetClaimIssuer<T>(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var claim = claimsPrincipal.Claims.FirstOrDefault(x => x.Issuer == _userIssuer && x.Type == claimType);
        if (claim != null)
            try
            {
                return JsonConvert.DeserializeObject<T>(claim.Value);
            }
            catch (Exception)
            {
                //ignore
            }

        return default;
    }

    #endregion
}