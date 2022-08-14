using Application.Interfaces.Services;
using Application.Queries.Dtos;
using Domain.Entities;
using KeywordsDashboard.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost, Route("auth")]
        public async Task<IActionResult> AuthAsync([FromBody] AuthorizationRequest authorizationRequest)
        {
            User? user = await _userService.AuthorizeAsync(new AuthorizationRequestDto
            {
                Login = authorizationRequest.Login,
                Password = authorizationRequest.Password
            });
            if (user is null)
            { 
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            return Ok();
        }
    }
}
