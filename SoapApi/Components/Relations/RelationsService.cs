using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using Newtonsoft.Json;
using SoapApi.Data;
using SoapApi.Data.Repositories;
using SoapApi.Helpers;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class RelationsService
    {
        private readonly SoapApiContext context;
        private readonly MapConfig mapConfig;
        private readonly IDriver _driver;

        public RelationsService(SoapApiContext context, MapConfig mapConfig, IDriver driver)
        {
            this.context = context;
            this.mapConfig = mapConfig;
            _driver = driver;
        }
        public async Task<bool> CheckIfUserExist(string userId)
        {
            return (await context.Auths.FirstOrDefaultAsync(x => x.Id == userId) == null);
        }

        public async Task<List<FriendRequest>> GetRequestsForSend(string userId)
        {
            List<FriendRequest> reqs = new List<FriendRequest>(); //await context.FriendRequest.Where(x => x.SenderId == userId).ToListAsync();
            //reqs.AddRange(await context.FriendRequest.Where(x => x.ReceiverId == userId).ToListAsync());

            return reqs;
        }
        public async Task<RequestContainerDTO> GetRequests(string userId)
        {
            RequestContainerDTO dto = new RequestContainerDTO();
            //dto.MyReqs = await context.FriendRequest.Where(x => x.SenderId == userId).ToListAsync();
            //dto.StrangersReqs = await context.FriendRequest.Where(x => x.ReceiverId == userId).ToListAsync();

            return dto;
        }

        public async Task GetMyRequests(string userId)
        {
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";
            //Node node = new Node();

            try
            {
                string cypher = new StringBuilder()
                .AppendLine("UNWIND $users AS user")
                .ToString();

                cursor = await session.RunAsync(cypher);
                var result = await cursor.SingleAsync(r => r.Values["n"].As<INode>());

                //json = JsonConvert.SerializeObject(result);
                //node = JsonConvert.DeserializeObject<Node>(json);
                json = JsonConvert.SerializeObject(result.Properties);
                //newUser = JsonConvert.DeserializeObject<User>(json);


            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }
        }

        public async Task SendRequest(FriendRequest req)
        {
            IAsyncSession session = _driver.AsyncSession();

            try
            {
                string cypher = new StringBuilder()
                .AppendFormat("MATCH (n:User) WHERE n.Id = '{0}' ", req.SenderId)
                .AppendFormat("MATCH (m:User) WHERE m.Id = '{0}' ", req.ReceiverId)
                .Append("MERGE (n)-[r:KNOWS]->(m)")
                .ToString();

                await session.RunAsync(cypher);
            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }

            return;
        }

        public async Task AcceptRequest(string myId, string friendId)
        {

            IAsyncSession session = _driver.AsyncSession();

            try
            {
                string cypher = new StringBuilder()
                .AppendFormat("MATCH (n:User) WHERE n.Id = '{0}' ", myId)
                .AppendFormat("MATCH (m:User) WHERE m.Id = '{0}' ", friendId)
                .Append("MERGE (n)-[r:KNOWS]->(m)")
                .ToString();

                await session.RunAsync(cypher);
            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }

            return;
        }

        public async Task<List<User>> GetMyFriends(string userId)
        {
            List<User> myFriends = new List<User>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";
            //Node node = new Node();

            try
            {
                string cypher = new StringBuilder()
                .Append("MATCH (n:User {Id: '"+userId+"'})-[:KNOWS]->(m) WHERE (m)-[:KNOWS]->(n) RETURN m")
                .ToString();

                cursor = await session.RunAsync(cypher);
                var result = await cursor.ToListAsync(r => r.Values["m"].As<INode>());

                foreach(INode node in result)
                {
                    json = JsonConvert.SerializeObject(node.Properties);
                    myFriends.Add(JsonConvert.DeserializeObject<User>(json));
                }
            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }

            return myFriends;
        }
    }
}
