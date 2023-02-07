using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Helpers;
using TodoApi.UserServices;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TodoApi.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private Helper _helper;
        private readonly AppSettings _appSettings;
        private readonly JwtSecurityTokenHandler _jwtHandler;

        public UserController(
            IUserService userService,
            IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;            _helper = new Helper();            _jwtHandler = new JwtSecurityTokenHandler();
        }

        // GET: api/v1/user
        [HttpGet, Route("user/{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            if (!_helper.CheckBearerToken(Request, _jwtHandler, _appSettings))
            {
                return Unauthorized();
            }
            var user = await _userService.GetById(id);
            if(user == null) return Ok(new { message = ("This user is not exsiting") }); 
            try {
                return Ok(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    Created = user.Created,
                    Updated = user.Updated,
                });
            }  
            catch (Exception)
            {
                return Unauthorized();
            }            
        }
        /*PUT /api/v1/changePassword: Change user’s password*/
        /*Body email and password, url param: new password*/
        // PUT: api/v1/changePassword
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754        
        [HttpPut, Route("changePassword/{newpassword}")]
        public async Task<ActionResult<User>> ChangePassword(string newPassword, UserDTO userDTO)
        {            
            if (!_helper.CheckBearerToken(Request, _jwtHandler, _appSettings))
            {
                return Unauthorized();
            }
            try
            {               
                var user = await _userService.Authenticate(userDTO.Email, userDTO.Password);
                if (user == null)
                   return BadRequest(new { message = "Username or password is incorrect" });
                await _userService.ChangePW(user, newPassword);
                return Ok(new { message = "Password was changed" });                 
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }           
        }
        /*POST /api/v1/signup: Sign up as an user of the system, using email & password*/
        /*Body email and password */
        // POST: api/v1/signup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Route("signup")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            User newUser;
            try
            {
                newUser = await _userService.Create(user);
            }
            catch (Exception e) 
            {
                return Problem(e.Message);
            }

            return Ok(new
            {
                Id = newUser.Id,
                Email = newUser.Email,
                Created = newUser.Created,
                Updated = newUser.Updated,
            });
        }
        /*POST /api/v1/signin: Sign in using email & password. The system will return the JWT token that can be used to call the APIs that follow*/
        // POST: api/v1/signin
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Route("signin")]
        public async Task<ActionResult<User>> LoginUser(UserDTO userDTO)
        {
            var user = await _userService.Authenticate(userDTO.Email, userDTO.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, userDTO.Email.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _appSettings.Secret,                
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            return Ok(new
            {
                Id = userDTO.Id,
                Email = userDTO.Email,
                Token = tokenString
            });
        }

        // DELETE: api/v1/User/5
        [HttpDelete, Route("user{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.Delete(id);
            return NoContent();
        }

        // OPTIONS: api/v1/User
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpOptions]
        public Task<IActionResult> OptionsUser()
        {

            Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");

            return Ok();
        }*/      
    }
}
