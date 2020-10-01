using SoapApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SoapApi.Data.Repositories;
using SoapApi.Models;

namespace SoapApi.Services
{
    public class UserService
    {
        private readonly QueryRepository _query;
        private readonly CommandRepository _command;
        public UserService(QueryRepository query, CommandRepository command)
        {
            _query = query;
            _command = command;
        }
        public async Task InitializeUser(string Id, string Username)
        {
            //Create user settings

            string query = "Users";
            User newUser = new User()
            {
                Id = Id,
                Username = Username,
                Firstname = "",
                Lastname = "",
                LastOnline = DateTime.Now,
                DateJoined = DateTime.Now,
                Status = Status.Online,
                ProfilePicture = ""
            };

            await _command.Post(newUser, query);

            return;
        }

        public async Task<FullUserDTO> GetUser(string query)
        {
            return await _query.GetSingle(new User(), new FullUserDTO(), query);
        }
    }
}
