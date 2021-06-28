using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.App.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PublicApi.DTO.v1;

namespace WebApp.ApiControllers.Identity
{
    /// <summary>
    /// Controller for Login and Register
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor of account controller.
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="userManager"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            ILogger<AccountController> logger, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Logging in endpoint. 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>JWT token</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.JwtResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PublicApi.DTO.v1.JwtResponse>> Login([FromBody] Login dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            // TODO: wait a random time here to fool timing attacks
            if (appUser == null)
            {
                _logger.LogWarning("WebApi login failed. User {User} not found", dto.Email);
                return NotFound(new Message("User/Password problem!"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, false);
            if (result.Succeeded)
            {
                var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
                var jwt = Extensions.Base.IdentityExtensions.GenerateJwt(
                    claimsPrincipal.Claims,
                    _configuration["JWT:Key"],                    
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Issuer"],
                    DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                _logger.LogInformation("WebApi login. User {User}", dto.Email);
                return Ok(new JwtResponse()
                {
                    Token = jwt,
                    Firstname = appUser.FirstName,
                    Lastname = appUser.LastName,
                });
            }
            
            _logger.LogWarning("WebApi login failed. User {User} - bad password", dto.Email);
            return NotFound(new Message("User/Password problem!"));
        }

        
        /// <summary>
        /// User registration endpoint.
        /// </summary>
        /// <param name="dto">Register dto object.</param>
        /// <returns>JWT, AppUser FirstName, AppUser LastName or Bad Request</returns>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.JwtResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(PublicApi.DTO.v1.Message),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PublicApi.DTO.v1.JwtResponse>> Register([FromBody] Register dto)
        {
            var appUser = await _userManager.FindByEmailAsync(dto.Email);
            // TODO: wait a random time here to fool timing attacks
            if (appUser != null)
            {
                _logger.LogWarning(" User {User} already registered", dto.Email);
                return NotFound(new Message("User already registered"));
            }

            appUser = new Domain.App.Identity.AppUser()
            {
                Email = dto.Email,
                UserName = dto.Email,
                FirstName = dto.Firstname,
                LastName = dto.Lastname,
            };
            var result = await _userManager.CreateAsync(appUser, dto.Password);
            
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} created a new account with password", appUser.Email);
                
                var user = await _userManager.FindByEmailAsync(appUser.Email);
                if (user != null)
                {                
                    var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    var jwt = Extensions.Base.IdentityExtensions.GenerateJwt(
                        claimsPrincipal.Claims,
                        _configuration["JWT:Key"],                    
                        _configuration["JWT:Issuer"],
                        _configuration["JWT:Issuer"],
                        DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireDays"))
                    );
                    _logger.LogInformation("WebApi login. User {User}", dto.Email);
                    return Ok(new JwtResponse()
                    {
                        Token = jwt,
                        Firstname = appUser.FirstName,
                        Lastname = appUser.LastName,
                    });
                    
                }
                else
                {
                    _logger.LogInformation("User {Email} not found after creation", appUser.Email);
                    return BadRequest(new Message("User not found after creation!"));
                }
            }
            
            var errors = result.Errors.Select(error => error.Description).ToList();
            return BadRequest(new Message() {Messages = errors});
        }

    }
}