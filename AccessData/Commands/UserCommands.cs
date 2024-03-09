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

        public UserCommands(IOptions<UserStoreDatabaseSettings> userStoreDatabaseSettings)
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
            catch
            {
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
            catch
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

        public User FindUserByFieldAsync(string fieldName, object value, List<string> excludeFields)
        {
            // Crear filtro para buscar un documento en la colección de usuarios
            var filter = Builders<User>.Filter.And(
                // Condición para el campo dinámico especificado por fieldName que debe ser igual al valor especificado por value
                Builders<User>.Filter.Eq(fieldName, BsonValue.Create(value)),
                // Condición para el campo 'Active' que debe ser igual a 1
                Builders<User>.Filter.Eq(u => u.Active, 1)
            );

            // Crear un constructor de proyección para seleccionar campos específicos para excluir
            var projectionBuilder = Builders<User>.Projection;

            // Excluir campos específicos de la proyección según la lista excludeFields
            var fields = excludeFields.Select(field => projectionBuilder.Exclude(field));

            // Ejecutar la consulta en la colección de usuarios, aplicando el filtro y la proyección
            User result = _userCollection.Find(filter)
                .Project<User>(projectionBuilder.Combine(fields)) // Aplicar la proyección al resultado de la consulta
                .FirstOrDefault(); // Obtener el primer documento que cumple con el filtro y la proyección


            return result;
        }

        public bool ExistUserByFieldAsync(string fieldName, object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return false;
            }
            var filter = Builders<User>.Filter.And(
                Builders<User>.Filter.Eq(fieldName, BsonValue.Create(value)),
                Builders<User>.Filter.Eq(u => u.Active, 1)
            );
            bool result = _userCollection.Find(filter).Any();
            return result;
        }

        public async Task<bool> UpdateUserForFieldsById(string id, User user, List<string> fieldsToUpdate)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, id);
            var updateBuilder = Builders<User>.Update.Set(u => u.Friends, user.Friends);

            foreach (var fieldName in fieldsToUpdate)
            {
                if(user.GetType()!= null)
                    updateBuilder = updateBuilder.Set(fieldName, user.GetType().GetProperty(fieldName).GetValue(user));

            }
            var result = await _userCollection.UpdateOneAsync(filter, updateBuilder);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteUser(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u._id, id);
            var updateBuilder = Builders<User>.Update.Set(u => u.Active, 0);
            var result = await _userCollection.UpdateOneAsync(filter, updateBuilder);
            return result.ModifiedCount > 0;
        }

        public List<UserDto> GetFriendsByUser(string idUSer)
        {
            List<UserDto> user = _userCollection.Find(x => x._id == idUSer && x.Active==1).Project(x =>  x.Friends ).First() ?? new List<UserDto>();
            return user;
        }

        
    }
}
