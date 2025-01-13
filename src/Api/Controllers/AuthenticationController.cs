using Argo.CA.Application.Common.Security.JwtTokenGeneration;
using Argo.CA.Contracts.Authentication;
using Argo.CA.Domain.UserAggregate;
using Argo.CA.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Argo.CA.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;
    private readonly IJwtTokenGenerator _tokenService;

    public AuthenticationController(
        UserManager<User> userManager,
        AppDbContext context,
        IJwtTokenGenerator tokenService,
        ILogger<AuthenticationController> logger)
    {
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] LoginRequest request)
    {
        var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);
        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

        if (userInDb is null)
        {
            return Unauthorized();
        }

        var roles = await _userManager.GetRolesAsync(userInDb);

        var accessToken = _tokenService.GenerateToken(
            userInDb.Id,
            userInDb.UserName!,
            userInDb.Email!,
            roles);

        await _context.SaveChangesAsync();

        return Ok(new LoginResponse(
            userInDb.UserName,
            userInDb.Email,
            accessToken));
    }
}