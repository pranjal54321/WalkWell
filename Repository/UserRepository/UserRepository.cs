using FinalProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinalProject.Repository.UserRepository
{
    public class UserRepository:IUser
    {

        private readonly FinalDbContext _context;
        private readonly IConfiguration configuration;

        public UserRepository(FinalDbContext context, IConfiguration configuration)
        {
            this._context = context;
            this.configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public  User GetUserById(int id)
        {
            return  _context.Users.FirstOrDefault(u => u.UserId == id);
        }
        public async Task<User> AddUserAsync(User user)
        {
            if (UserExist(user))
            {
                return null;
            }
            else
            {
                var result = await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return result.Entity;
            }
        }

        public string Login(string email, string password)
        {
            var userExist = _context.Users.FirstOrDefault(t => t.Email == email && t.Password== password);
            if (userExist != null)
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
             new Claim(ClaimTypes.Email,userExist.Email),
             new Claim("UserId",userExist.UserId.ToString()),
             new Claim(ClaimTypes.Role,userExist.UserType)
         };
                var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
                return new JwtSecurityTokenHandler().WriteToken(token);

            }
            return null;
        }

        public async Task<User> UpdateUserDetail(int id, User user)
        {
            var result = await _context.Users.FirstOrDefaultAsync(t => t.UserId == id);
            if (result != null)
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.Email = user.Email;
                result.Password = user.Password;
                
                result.Address = user.Address;

                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }

        private bool UserExist(User user)
        {
            return _context.Users.Any(t => t.Email == user.Email);
        }

    }
}
