using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface UserInterface
    {
        Task InsertUser(Usuario user);
        Task UpdateUser(Usuario user);
        Task DeleteUser(string id);
        Task <List<Usuario>> GetAllUsuarios();
        Task <Usuario> GetUsuarioById(string id);
        Task <Usuario> GetUsuarioByLogin(string name,string pass);
    }
}