using SoapApi.Services;
using System.Threading.Tasks;


namespace SoapApi.Components.Mediator
{
    public class MediatorService
    {
        private readonly UserService user;

        public MediatorService(UserService user)
        {
            this.user = user;
        }
        public async Task InitializeUser(string id, string username)
        {
            await user.InitializeUser(id, username);
        }
    }
}
