using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NoTricks.Data.Models;
using NoTricks.Data.Repositories;

namespace NoTrick.Web.Services {
    public class SignInService {
        private readonly IAccountRepo _accountRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly IProfileRepo _profileRepo;
        private readonly ILogger<SignInService> _logger;

        public SignInService(IAccountRepo accountRepo,
                             IRoleRepo roleRepo,
                             IProfileRepo profileRepo,
                             ILogger<SignInService> logger) {
            _accountRepo = accountRepo;
            _roleRepo = roleRepo;
            _profileRepo = profileRepo;
            _logger = logger;
        }
        
        public AccountStatus CheckAccount(string email, string password) {
            var account = _accountRepo.GetByEmail(email);
            if (account == null) {
                _logger.LogInformation("No account with the email: " + email);
                return AccountStatus.Unauthenticated;
            }

            var hashedPassword = PasswordHasher.HashPassword(password, account.PasswordSalt);
            
            return hashedPassword.Item1 != account.PasswordHash ? AccountStatus.Unauthenticated : account.Status;
        }

        public async Task SignInAsync(string email, bool rememberMe, HttpContext httpContext) {
            var account = _accountRepo.GetByEmail(email);
            if (account == null) {
                var msg = $"email: {email} does not exist in accounts";
                throw new ArgumentException(msg, nameof(email));
            }
            
            var profile = _profileRepo.GetByAccountId(account.Id);
            var roles = _roleRepo.GetAllFormAccountId(account.Id); 
            
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, account.EMail),
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.GivenName, profile.FirstName),
                new Claim(ClaimTypes.Surname, profile.LastName),
                new Claim(ClaimTypes.Name, profile.PreferredFirstLastName),
            };
            
            if (profile.Birthday.HasValue) 
                claims.Add(new Claim(ClaimTypes.DateOfBirth, profile.Birthday.Value.ToLongDateString()));

            if (!string.IsNullOrWhiteSpace(profile.Phone))
                claims.Add(new Claim(ClaimTypes.OtherPhone, profile.Phone));
            
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r.Name)));
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties {
                AllowRefresh = true,
                ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddMonths(1) : DateTimeOffset.UtcNow.AddDays(1),
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);
            
            account.LastLoginAt = DateTime.UtcNow;
            _accountRepo.Update(account);
        }
    }
}