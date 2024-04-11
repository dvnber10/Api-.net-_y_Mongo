using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using API.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories
{
    public class UserCollections : UserInterface
    {
        internal Contexto _contexto = new Contexto(); 
        private IMongoCollection<Usuario> collection;

        public UserCollections(){
            collection=_contexto.db.GetCollection<Usuario>("Usuarios");
        } 
        public async Task DeleteUser(string id) 
        {
            var filter = Builders<Usuario>.Filter.Eq(s => s.Id, new ObjectId(id));
                await collection.DeleteOneAsync(filter);
        }

        public async Task<List<Usuario>> GetAllUsuarios()
        {
            return await collection.FindAsync(new BsonDocument()).Result.ToListAsync();
        }
        public async Task<Usuario> GetUsuarioById(string id)
        {
            return await collection.FindAsync(new BsonDocument{{"_id", new ObjectId(id)}}).Result.FirstAsync();
        }
        public async Task<Usuario> GetUsuarioByLogin(string name, string pass)
        {
            var UserActive = await collection.FindAsync(c=>c.Nombre == name).Result.FirstOrDefaultAsync();
            var password = UserActive.Password;
            if (BCrypt.Net.BCrypt.Verify(pass,password))
            {
               return UserActive; 
            }else{
                UserActive = null;
                return UserActive;
            }
        }
        public async Task InsertUser(Usuario user)
        {
            
            await collection.InsertOneAsync(user);
        }
        public async Task UpdateUser(Usuario user)
        {
            var filter = Builders<Usuario>.Filter.Eq(s=> s.Id , user.Id);
            await collection.ReplaceOneAsync(filter,user);   
        }
        public static string HashPass(string HashPass){
            string PassEn = BCrypt.Net.BCrypt.HashPassword(HashPass, BCrypt.Net.BCrypt.GenerateSalt());
            return PassEn;
        }
    }
}