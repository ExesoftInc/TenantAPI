using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenantAPI.Entities;
using TenantAPI.Models;

namespace TenantAPI.Authentication
{
    public interface IAuthenticate
    {
        Task<string> GenerateToken(string tenantName, UserModel userModel, bool isNew);
        Task<User> Register(User user, string password);

        Task<bool> ChangePassword(User user, string password);

        Task<User> RegisterByProvider(User user);

        bool NeedsProviderRegisterForm(Guid tenantId, string email, string provider);

        Task<User> Login(Guid tenantId, string email, string pass);

        Task<User> LoginByProvider(Guid tenantId, string id);

        bool UserExists(Guid tenantId, string email, string provider);
    }
}
