using SoapApi.Data.Repositories;
using SoapApi.Models;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class AuthService
    {
        private readonly CommandRepository _command;
        private readonly QueryRepository _query;
        private readonly UserService _user;

        public AuthService(QueryRepository query, CommandRepository command, UserService user)
        {
            _query = query;
            _command = command;
            _user = user;
        }

        public async Task<Auth> Login(string username, string password)
        {
            string query = "Auth:Username:" + username;
            var user = await _query.GetSingle(new Auth(), new Auth(), query);
            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<Auth> Register(Auth user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            string query = "Auth";

            await _command.Post(user, query);
            await _user.InitializeUser(user.Id, user.Username);

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            string query = "Auth:Username:" + username;
            if (await _query.GetSingle(new Auth(), new Auth(), query) == null)
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
            string query = "Auth:_id:" + Id;
            if (await _query.GetSingle(new Auth(), new Auth(), query) == null)
                return false;
            return true;
        }
    }
}
