using Neo4j.Driver;
using Newtonsoft.Json;
using SoapApi.Data;
using SoapApi.Data.Repositories;
using SoapApi.Helpers;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoapApi.Services
{
    public class EventService
    {
        private readonly IDriver _driver;
        public EventService(IDriver driver)
        {
            _driver = driver;
        }
        public async Task<Event> CreateEvent(string name, string userId)
        {

            Event newEvent = new Event()
            {
                Id = Guid.NewGuid().ToString(),
                Navn = name,
                OwnerId = userId
            };

            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";
            //Node node = new Node();
            var test = new Dictionary<string, object>() { { "events", ParameterSerializer.ToDictionary(new List<Event>() { newEvent }) } };
            try
            {
                string cypher = new StringBuilder()
                .AppendLine("UNWIND $events AS event")
                .AppendLine("MATCH (m:User) WHERE m.Id = '" + userId + "'")
                .AppendLine("CREATE (n:Event) SET n = event")
                .AppendLine("MERGE (m)-[r:OWNS]->(n)")
                .AppendLine("MERGE (m)-[s:ATTENDS]->(n)")
                .AppendLine("RETURN n")
                .ToString();

                cursor = await session.RunAsync(cypher, test);
                var result = await cursor.SingleAsync(r => r.Values["n"].As<INode>());

                json = JsonConvert.SerializeObject(result.Properties);

            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }

            return newEvent;
        }

        public async Task<List<Event>> GetOwnedEvents(string userId)
        {
            List<Event> events = new List<Event>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";

            try
            {
                string cypher = new StringBuilder()
                .AppendLine("MATCH (n:User {Id: '" + userId + "'})-[:OWNS]->(m) RETURN m")
                .ToString();

                cursor = await session.RunAsync(cypher);
                var result = await cursor.ToListAsync(r => r.Values["m"].As<INode>());

                foreach (INode node in result)
                {
                    json = JsonConvert.SerializeObject(node.Properties);
                    events.Add(JsonConvert.DeserializeObject<Event>(json));
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

            return events;
        }

        public async Task<List<Event>> GetAttendingEvents(string userId)
        {
            List<Event> events = new List<Event>();
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            string json = "";

            try
            {
                string cypher = new StringBuilder()
                .AppendLine("MATCH (n:User {Id: '" + userId + "'})-[:ATTENDS]->(m) RETURN m")
                .ToString();

                cursor = await session.RunAsync(cypher);
                var result = await cursor.ToListAsync(r => r.Values["m"].As<INode>());

                foreach (INode node in result)
                {
                    json = JsonConvert.SerializeObject(node.Properties);
                    events.Add(JsonConvert.DeserializeObject<Event>(json));
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

            return events;
        }

        public async Task<string> JoinEvent(string userId, string eventId)
        {
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                string cypher = new StringBuilder()
                .AppendLine("MATCH (n:User {Id: '" + userId + "'}), (m:Event {Id: '" + eventId + "'})")
                .AppendLine("MERGE (n)-[r:ATTENDS]->(m)")
                .AppendLine("RETURN (m)")
                .ToString();

                cursor = await session.RunAsync(cypher);
                //var result = await cursor.ToListAsync(r => r.Values["m"].As<INode>());

                //foreach (INode node in result)
                //{
                //    json = JsonConvert.SerializeObject(node.Properties);
                //    if (json.Contains(eventId))
                //        return "Nope!";
                //    events.Add(JsonConvert.DeserializeObject<Event>(json));
                //}

                //cypher = new StringBuilder()
                //    .AppendLine("MATCH (n:User {Id: '" + userId + "'})")
                //    .AppendLine("MATCH (m:Event {Id: '" + eventId + "'})")
                //    .AppendLine("").ToString();
            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }


            return "Ok";
        }

        public async Task<string> LeaveEvent(string userId, string eventId)
        {
            IResultCursor cursor;
            IAsyncSession session = _driver.AsyncSession();
            try
            {
                string cypher = new StringBuilder()
                .AppendLine("MATCH (n:User {Id: '" + userId + "'})-[r:ATTENDS]->(m:Event {Id: '" + eventId + "'})")
                .AppendLine("DELETE r")
                .AppendLine("RETURN (m)")
                .ToString();

                cursor = await session.RunAsync(cypher);
            }
            catch (Exception ex)
            {
                string exep = ex.ToString();
            }
            finally
            {
                await session.CloseAsync();
            }

            return "Ok";
        }
    }
}
