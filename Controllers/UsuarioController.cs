using API.Models;
using API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private UserInterface db = new UserCollections();
        private readonly string? SecretKey;

        public UsuarioController( IConfiguration configuration){
            SecretKey= configuration.GetSection("Settings").GetSection("SecretKey").ToString();

        }
        [HttpGet]
        [Route ("AllUser")]
        public async Task<IActionResult> GetAllUsuarios(){
            return Ok(await db.GetAllUsuarios());
        }
        [HttpGet]
        [Route("Details")]
        public async Task<IActionResult> GetUserDetails(string id){
            return Ok(await db.GetUsuarioById(id));
        }
        [HttpPost]
        [Route("Create")]  
        public async Task<IActionResult> CreateUser([FromBody]Usuario user){
            if (user == null) return BadRequest();
            if (user.Nombre == string.Empty){
                ModelState.AddModelError("Nombre","El nombre no puede estar vacio");
            }
            user.Password = UserCollections.HashPass(user.Password);
            
            await db.InsertUser(user);
            return Created("Created",true);
        }
        [HttpPut]
        [Route("Update")]  
        public async Task<IActionResult> UpdateUser([FromBody]Usuario user, string id){
            if (user == null) return BadRequest();
            if (user.Nombre == string.Empty){
                ModelState.AddModelError("Nombre","El nombre no puede estar vacio");
            }
            user.Id=new MongoDB.Bson.ObjectId(id);
            user.Password= UserCollections.HashPass(user.Password);
            await db.UpdateUser(user);
            return Created("Created",true);
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteUser(string id){
            await db.DeleteUser(id);
            return NoContent();
        }
        [HttpPost]
        [Route ("Login")]
        public async Task<IActionResult> Login([FromBody]Usuario user){
            var userAct=await db.GetUsuarioByLogin(user.Nombre , user.Password);
            if (userAct == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized , new{tokenCompleto = ""});
            }
            var token = GenerarToken(user.Nombre);
            return StatusCode(StatusCodes.Status200OK , new {tokenCompleto = token});
        }
        private string GenerarToken(string NombreU){
            var security= Encoding.ASCII.GetBytes(SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new []{
                    new Claim(ClaimTypes.Name,NombreU)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(security), SecurityAlgorithms.HmacSha256Signature) 
            };
            var TokenHandler = new JwtSecurityTokenHandler();
            var token = TokenHandler.CreateToken(tokenDescriptor);
            return TokenHandler.WriteToken(token); 
        }
        
    }
}