using SoapApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoapApi.Data.Repositories;
using SoapApi.Models;
using Neo4j.Driver;
using System.Text;
using Newtonsoft.Json;
using SoapApi.Helpers;

namespace SoapApi.Services
{
    public class UserService
    {
        private readonly QueryRepository _query;
        private readonly CommandRepository _command;
        private readonly IDriver _driver;
        public UserService(QueryRepository query, CommandRepository command, IDriver driver)
        {
            _query = query;
            _command = command;
            _driver = driver;
        }
        public async Task<User> InitializeUser(string Id, string Username)
        {
            //Create user settings

            User newUser = new User()
            {
                Id = Id,
                Username = Username,
                Firstname = "Marius",
                Lastname = "Skauen",
                LastOnline = DateTime.Now,
                DateJoined = DateTime.Now,
                ProfilePicture = "dumdumdumdumdumddmu"
            };

            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";
            //Node node = new Node();

            var test = new Dictionary<string, object>() { { "users", ParameterSerializer.ToDictionary(new List<User>() { newUser }) } };
            try
            {
                string cypher = new StringBuilder()
                .AppendLine("UNWIND $users AS user")
                .AppendLine("CREATE (n:User) SET n = user")
                .AppendLine("RETURN n")
                .ToString();

                cursor = await session.RunAsync(cypher, test);
                var result = await cursor.SingleAsync(r => r.Values["n"].As<INode>());

                //json = JsonConvert.SerializeObject(result);
                //node = JsonConvert.DeserializeObject<Node>(json);
                json = JsonConvert.SerializeObject(result.Properties);
                newUser = JsonConvert.DeserializeObject<User>(json);


            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }

            return newUser;
        }

        public async Task<FullUserDTO> GetUser(string query)
        {
            return await _query.GetSingle(new User(), new FullUserDTO(), query);
        }
    }
}
