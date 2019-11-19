using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;
using Org.BouncyCastle.Security;

namespace NoTrick.Web.Services {
    public class SignInService {
        private readonly IAccountRepo _accountRepo;

        public SignInService(IAccountRepo accountRepo) {
            _accountRepo = accountRepo;
        }
        
        public AccountStatus CheckAccount(string email, string password) {
            var account = _accountRepo.GetByEmail(email);
            if (account == null) return AccountStatus.Unauthenticated;

            var hashedPassword = PasswordHasher.HashPassword(password, account.PasswordSalt);
            
            return hashedPassword.Item1 != account.PasswordHash ? AccountStatus.Unauthenticated : account.Status;
        }

        public async Task SignInAsync(string email, bool rememberMe, HttpContext httpContext) {
            var account = _accountRepo.GetByEmail(email);
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, account.EMail),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                //TODO: set account name to username and fetch roles
                new Claim(ClaimTypes.Name, account.EMail)
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {
                AllowRefresh = true,
                ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddMonths(1) : DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);
            
        }
    }
}