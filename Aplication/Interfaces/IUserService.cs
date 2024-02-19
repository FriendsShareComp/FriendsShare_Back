﻿using Aplication.Utils;
using Domain.Dto;

namespace Aplication.Interfaces
{
    public interface IUserService
    {
        Response UserRegister(UserRegisterDto user);
        public Response GetFriendsByUser(string idUser);
        public Response AddFriendsByUser(string idUserLogged, string idUserFriend);
    }
}
