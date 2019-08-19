using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brook.Totp.WebLogin2FA.Cache;
using Brook.Totp.WebLogin2FA.Models;
using Microsoft.EntityFrameworkCore;

namespace Brook.Totp.WebLogin2FA.Services
{
    public class UserService : IUserService
    {
        public readonly BrookTotpDBContext _dbContext;
        public UserService(BrookTotpDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserAsync(string email)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Email.Equals(email));

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var count = await _dbContext.Users.CountAsync(a => a.Email.Equals(email) && a.PassWord.Equals(password));

            return count > 0;
        }

        public async Task<bool> UpdateAsync(User user)
        {
           _dbContext.Users.Update(user);
            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }
        
    }
}
