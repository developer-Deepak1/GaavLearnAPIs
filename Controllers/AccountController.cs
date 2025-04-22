using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GaavLearnAPIs.Dtos;
using GaavLearnAPIs.Migrations;
using GaavLearnAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GaavLearnAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController(UserManager<AppUser> userManager
        , RoleManager<IdentityRole> roleManager
        , IConfiguration configuration
        ) : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;
        
        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(TokenDto tokenDto){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }
            var priciple=GetPrincipalFromExpiredToken(tokenDto.Token); 
            var user =await _userManager.FindByEmailAsync(tokenDto.Email);

            if(priciple is null ||user is null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime >= DateTime.UtcNow){
             if(priciple is null)
             return BadRequest(new AuthResponseDto{
                IsSuccess=false,
                Message="priciple is null"
             });
             if(user is null)
               return BadRequest(new AuthResponseDto{
                IsSuccess=false,
                Message="user is null"
             });
             if(user.RefreshToken != tokenDto.RefreshToken)
              return BadRequest(new AuthResponseDto{
                IsSuccess=false,
                Message="user.RefreshToken != tokenDto.RefreshToken"
             });
             if(user.RefreshTokenExpiryTime >= DateTime.UtcNow)
              return BadRequest(new AuthResponseDto{
                IsSuccess=false,
                Message="user.RefreshTokenExpiryTime >= DateTime.UtcNow"
             });


            }

            var newJwtToken=GenerateJwtToken(user);
            var newRefreshToken=GenerateRefreshToken();
            _=int.TryParse(_configuration.GetSection("JWTsetting").GetSection("RefreshTokenValidityIn").Value!, out int RefreshTokenValidityIn);
            user.RefreshToken=newRefreshToken;
            user.RefreshTokenExpiryTime=DateTime.UtcNow.AddMinutes(RefreshTokenValidityIn);
            await _userManager.UpdateAsync(user);
            
            return Ok(new AuthResponseDto
            {
                Token = newJwtToken,
                IsSuccess = true,
                Message = "Token Refresh!",
                RefreshToken=newRefreshToken
            });
        }
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token){
            var tokenParameters=new TokenValidationParameters{
                ValidateAudience=false,
                ValidateIssuer=false,
                ValidateIssuerSigningKey=true,
                IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWTsetting").GetSection("securityKey").Value!)),
                ValidateLifetime=false
            };
            var tokenHandler=new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenParameters, out SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase))
               throw new SecurityTokenException("Invalid Token ");
            
            return principal;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (registerDto.Roles is null)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                foreach (var role in registerDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Account Created Successful!"
            });

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found with email",
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid Password",
                });
            }

            var token = GenerateJwtToken(user);
            var refreshToken=GenerateRefreshToken();
            _=int.TryParse(_configuration.GetSection("JWTsetting").GetSection("RefreshTokenValidityIn").Value!, out int RefreshTokenValidityIn);
            user.RefreshToken=refreshToken;
            user.RefreshTokenExpiryTime=DateTime.UtcNow.AddMinutes(RefreshTokenValidityIn);
            await _userManager.UpdateAsync(user);
            return Ok(new AuthResponseDto
            {
                Token = token,
                IsSuccess = true,
                Message = "Login Success.",
                RefreshToken=refreshToken
            });
        }

        private string  GenerateRefreshToken(){
         var randomNumber= new byte[32];
         using var rng=RandomNumberGenerator.Create();
         rng.GetBytes(randomNumber);
         return Convert.ToBase64String(randomNumber);
        }       


        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<ActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordDto.Email);

            if (user is null)
            {
                return Ok(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not exists with email."
                });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var baseurl = _configuration.GetSection("DefaultConfiguration").GetSection("BaseUrl");
            var resetLink = $"{baseurl}/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}";

            // Looking to send emails in production? Check out our Email API/SMTP product!
            // using RestSharp;

            // var client = new RestClient("https://sandbox.api.mailtrap.io/api/send/3632535");
            // var request = new RestRequest();
            // request.AddHeader("Authorization", "Bearer 46589af6c420fc0521c274a8511e572a");
            // request.AddHeader("Content-Type", "application/json");
            // request.AddParameter("application/json", "{\"from\":{\"email\":\"hello@example.com\",\"name\":\"Mailtrap Test\"},\"to\":[{\"email\":\"dkt2445@gmail.com\"}],\"subject\":\"You are awesome!\",\"text\":\"Congrats for sending test email with Mailtrap!\",\"category\":\"Integration Test\"}", ParameterType.RequestBody);
            // var response = client.Post(request);
            // System.Console.WriteLine(response.Content);

            var client = new RestClient("https://sandbox.api.mailtrap.io/api/send/3632535");
            var request = new RestRequest
            {
                Method = Method.Post,
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Authorization", "Bearer 46589af6c420fc0521c274a8511e572a");
            request.AddJsonBody(new
            {
                from = new { email = "mailtrap@demomailtrap.com" },
                to = new[] { new { email = user.Email } },
                template_uuid = "b436f046-19b0-4165-a55b-e52251e45979",
                template_variables = new { user_email = user.Email, pass_reset_link = resetLink }
            });

            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                return Ok(new AuthResponseDto
                {
                    IsSuccess = true,
                    Message = "Email sent with password reset link. Please check your email."
                });
            }
            return BadRequest(new AuthResponseDto
            {
                IsSuccess = false,
                Message = "Failed to send email."
            });

        }

        private string GenerateJwtToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTsetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = [
             new (JwtRegisteredClaimNames.Email,user.Email??""),
            new (JwtRegisteredClaimNames.Name,user.FullName??""),
            new (JwtRegisteredClaimNames.NameId,user.Id??""),
            new (JwtRegisteredClaimNames.Aud,_configuration.GetSection("JWTsetting").GetSection("ValidAudience").Value!),
            new (JwtRegisteredClaimNames.Iss,_configuration.GetSection("JWTsetting").GetSection("ValidIssuer").Value!)
            ];

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDesciptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                 new SymmetricSecurityKey(key),
                 SecurityAlgorithms.HmacSha256
             )
            };

            var token = tokenHandler.CreateToken(tokenDesciptor);

            return tokenHandler.WriteToken(token);
        }

        [HttpGet("UserDetail")]
        [Authorize]
        public async Task<ActionResult<UserDetailDto>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);

            if (user is null)
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User Not Found"
                });
            }
            return Ok(new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfimed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetDetails()
        {

            var users = await _userManager.Users.Select(u => new UserDetailDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                Roles = _userManager.GetRolesAsync(u).Result.ToArray()
            }).ToListAsync();

            return Ok(users);
        }

    }
}
