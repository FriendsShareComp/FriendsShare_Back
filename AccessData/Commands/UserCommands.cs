using Aplication.Interfaces;
using Aplication.Utils;
using AutoMapper;
using Domain.Dto;
using Domain.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AccessData.Services
{
    public class UserCommands : IUserCommands
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserCommands(
            IOptions<UserStoreDatabaseSettings> userStoreDatabaseSettings,
            IMapper mapper)
        {
            var mongoClient = new MongoClient(
                userStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userStoreDatabaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                userStoreDatabaseSettings.Value.UserCollectionName);
        }

        public async Task<Response> CreateUser(User user)
        {

            var responseCreated = new Response(true, "creacion del usuario completada");
            responseCreated.StatusCode = 200;
            try
            {
                // Insertar el usuario en la colección
                await _userCollection.InsertOneAsync(user);
                responseCreated.objects = user;

            }
            catch (Exception ex)
            {
                // Aquí puedes registrar la excepción o manejarla de otra manera
                // En este ejemplo, simplemente relanzamos la excepción para que sea manejada en niveles superiores
                var response = new Response(false, "Internal server error");
                response.StatusCode = 500;
                return response;
                throw;
            }
            return responseCreated;
        }
        public Response SearchUserByCredentials(dtoLoginUser userDto)
        {
            var responseCreated = new Response(true, "Usuario existente");
            responseCreated.StatusCode = 200;
            try
            {
                User user = _userCollection.Find(x => (x.UserName == userDto.email || x.Email == userDto.email) && x.Password == userDto.password && x.Active==1).First();
                responseCreated.objects = user;
            }
            catch (Exception ex)
            {
                var response = new Response(false, "Internal server error");
                response.StatusCode = 500;
                return response;
                throw;
            }
            return responseCreated;
        }
        public bool UserExistByCredentials(string credential)
        {
            int count = (int) _userCollection.Find(x => x.UserName == credential || (x.Email == credential && x.Email!=null)).Count();

            

            return count>0;
        }
        public User GetUserById(string idUSer)
        {
            User user = _userCollection.Find(x => x._id == idUSer && x.Active == 1).First();
            return user;
        }

        public User FindUserByFieldAsync(string fieldName, object value)
        {
            var filter = Builders<User>.Filter.Eq(fieldName, BsonValue.Create(value));
            var result = _userCollection.Find(filter).First();
            return result;
        }

        public List<UserDto> GetFriendsByUser(string idUSer)
        {
            List<UserDto> user = _userCollection.Find(x => x._id == idUSer && x.Active==1).Project(x =>  x.Friends ).First() ?? new List<UserDto>();
            return user;
        }
    }
}
