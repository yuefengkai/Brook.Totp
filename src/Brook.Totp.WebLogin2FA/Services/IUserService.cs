using Brook.Totp.WebLogin2FA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string email);

        Task<bool> LoginAsync(string email,string password);

        Task<bool> UpdateAsync(User user);
    }
}
