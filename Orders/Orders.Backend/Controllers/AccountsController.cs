using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Orders.Backend.Helpers;
using Orders.Backend.UnitsOfWork.Interfaces;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Orders.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileStoragecs _fileStoragecs;
        private readonly IMailHelper _mailHelper;

        public AccountsController(IUsersUnitOfWork usersUnitOfWork, IConfiguration configuration, IFileStoragecs fileStoragecs, 
            IMailHelper mailHelper)
        {
           _usersUnitOfWork = usersUnitOfWork;
           _configuration = configuration;
           _fileStoragecs = fileStoragecs;
            _mailHelper = mailHelper;
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO model)
        {
            User user = model;
            if(!string.IsNullOrEmpty(model.Photo))
            {
                var photoUser= Convert.FromBase64String(model.Photo);
                model.Photo = await _fileStoragecs.SaveFileAsync(photoUser, ".jpg", "users");
            }

            var result = await _usersUnitOfWork.AddUserAsync(user, model.Password);
            if(result.Succeeded)
            {
                await _usersUnitOfWork.AddUserToRoleAsync(user, user.UserType.ToString());
                var response=await SendConfirmationEmailAsync(user);

                return Ok(BuildToken(user));
            }
            return BadRequest(result.Errors.FirstOrDefault());
        }

        private async Task<ActionResponse<string>> SendConfirmationEmailAsync(User user)
        {
            var myToken = await _usersUnitOfWork.GenerateEmailConfirmationTokenAsync(user);
            var tokenLink = Url.Action("ConfirmEmail", "accounts", new
            {
                userId = user.Id,
                token=myToken,

            }, HttpContext.Request.Scheme, _configuration["UrlFrondend"]);
            return _mailHelper.SendMail(user.FullName, user.Email!,
                $"Orders - Account Confirmation",
                $"<h1>Orders - Account Confirmation</h1>" +
                $"<p>To enable the user, please click 'Confirm Email':</p>" +
                $"<b><a href ={tokenLink}>Confirmar Email</a></b>");

        }
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
        {
            token = token.Replace(" ", "+");
            var user = await _usersUnitOfWork.GetUserAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound();
            }

            var result = await _usersUnitOfWork.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault());
            }

            return NoContent();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAsync(User user)
        {
            try
            {
                var currentUser = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
                if(currentUser  == null)
                {
                    return NotFound();
                }
                if(!string.IsNullOrEmpty(user.Photo))
                {
                    var userPhoto=Convert.FromBase64String(user.Photo);
                    user.Photo = await _fileStoragecs.SaveFileAsync(userPhoto, ".jpg", "users");
                }
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Photo = !string.IsNullOrEmpty(user.Photo) && user.Photo != currentUser.Photo
                    ? user.Photo : currentUser.Photo;
                currentUser.PhoneNumber = user.PhoneNumber;
                currentUser.Address= user.Address;
                currentUser.CityId=user.CityId;
                var result=await _usersUnitOfWork.UpdateUserAsync(currentUser);
                if(result.Succeeded)
                {
                    return Ok(BuildToken(currentUser));
                }
                return BadRequest(result.Errors.FirstOrDefault());

            }
            catch (Exception Ex)
            {

                return BadRequest(Ex.Message);
            }
        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            var result = await _usersUnitOfWork.LoginAsync(model);
            if (result.Succeeded)
            {
                var user = await _usersUnitOfWork.GetUserAsync(model.Email);
                return Ok(BuildToken(user));
            }
            if (result.IsLockedOut)
            {
                return BadRequest("You have exceeded the maximum number of attempts, your account is blocked, try again in 5 minutes.");
            }

            if (result.IsNotAllowed)
            {
                return BadRequest("The user has not been enabled, you must follow the instructions in the email sent to enable the user.");
            }


            return BadRequest("Wrong email or password.");
        }

        private TokenDTO BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Email!),
                new(ClaimTypes.Role, user.UserType.ToString()),              
                new("FirstName", user.FirstName),
                new("LastName", user.LastName),
                new("Address", user.Address),
                new("Photo", user.Photo ?? string.Empty),
                new("CityId", user.CityId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwtKey"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddDays(30);
            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new TokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

        [HttpPost("changePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _usersUnitOfWork.GetUserAsync(User.Identity!.Name!);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _usersUnitOfWork.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault()!.Description);
            }

            return NoContent();

        }

    }

}
   