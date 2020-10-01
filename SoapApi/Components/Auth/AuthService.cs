using Microsoft.EntityFrameworkCore;
using SoapApi.Data;
using SoapApi.Models;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class AuthService
    {
        private readonly SoapApiContext context;

        public AuthService(SoapApiContext context)
        {
            this.context = context;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<User> Login(string username, string password)
        {
            User user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<bool> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UsernameExists(string username)
        {
            string query = "Auth:Username:" + username;
            if (await context.Users.FirstOrDefaultAsync(x => x.Username == username) == null)
                return false;
            return true;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> IdExists(string Id)
        {
            if (await context.Users.FirstOrDefaultAsync(d => d.Id == Id) == null)
                return false;
            return true;
        }
    }
}
