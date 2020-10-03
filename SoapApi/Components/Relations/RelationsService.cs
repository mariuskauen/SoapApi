using Microsoft.EntityFrameworkCore;
using Neo4j.Driver;
using Newtonsoft.Json;
using SoapApi.Data;
using SoapApi.Helpers;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class RelationsService
    {
        private readonly SoapApiContext context;
        private readonly IDriver _driver;

        public RelationsService(SoapApiContext context, IDriver driver)
        {
            this.context = context;
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

        public async Task<List<User>> GetMyRequests(string userId)
        {
            List<User> myFriends = new List<User>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";
            //Node node = new Node();

            try
            {
                string cypher = new StringBuilder()
                .Append("MATCH (n:User {Id: '" + userId + "'})-[:KNOWS]->(m) WHERE NOT (m)-[:KNOWS]->(n) RETURN m")
                .ToString();

                cursor = await session.RunAsync(cypher);
                var result = await cursor.ToListAsync(r => r.Values["m"].As<INode>());

                foreach (INode node in result)
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
        public async Task<List<User>> GetInboundRequests(string userId)
        {
            List<User> myFriends = new List<User>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";
            //Node node = new Node();

            try
            {
                string cypher = new StringBuilder()
                .Append("MATCH (n:User {Id: '" + userId + "'})<-[:KNOWS]-(m) WHERE NOT (m)<-[:KNOWS]-(n) RETURN m")
                .ToString();

                cursor = await session.RunAsync(cypher);
                var result = await cursor.ToListAsync(r => r.Values["m"].As<INode>());

                foreach (INode node in result)
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
