using AutoMapper;
using SoapApi.Data;
using SoapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Helpers
{
    public class MapConfig
    {
        public MapperConfiguration requestToDTO = new MapperConfiguration(cfg => cfg.CreateMap<FriendRequest, FriendRequestDTO>());

        public MapperConfiguration requestUser = new MapperConfiguration(cfg => cfg.CreateMap<User, RequestUserDTO>());

        public MapperConfiguration requestsToDTO = new MapperConfiguration(cfg => cfg.CreateMap<List<FriendRequest>, List<FriendRequestDTO>>());

        //public class FriendRequestResolver : IValueResolver<List<FriendRequest>,FriendRequest, List<FriendRequestDTO>>
        //{
        //    public List<FriendRequestDTO> Resolve(List<FriendRequest> requests, FriendRequest useless, List<FriendRequestDTO> destList, ResolutionContext context)
        //    {
        //        var mapConfig = new MapConfig();
        //        var mapper = mapConfig.requestToDTO.CreateMapper();
        //        foreach (FriendRequest fr in requests)
        //        {
        //            destList.Add(mapper.Map<FriendRequestDTO>(fr));
        //        }

        //        return destList;
        //    }
        //}

        //public class RequestUserResolver : IValueResolver<FriendRequest, FriendRequestDTO, RequestUserDTO>
        //{
        //    public RequestUserDTO Resolve(FriendRequest req, FriendRequestDTO reqDto, RequestUserDTO user)
        //    {
        //        var mapConfig = new MapConfig();
        //        var mapper = mapConfig.requestUser.CreateMapper();

        //    }
        //}
    }
}
