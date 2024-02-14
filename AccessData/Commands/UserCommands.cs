using Aplication.Interfaces;
using Aplication.Utils;
using AutoMapper;
using Domain.Models;
using Domain.Security;
using Microsoft.Extensions.Options;
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
                user.Password= Encrypt.encryption(user.Password);

                
                // Validar los datos del usuario antes de insertar

                

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
    }
}
