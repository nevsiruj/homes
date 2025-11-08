using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Domain.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Http;


[Route("[controller]")]
[ApiController]

public class AgenteController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;


    public AgenteController(IAuthService authService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;

    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginRequestUserName
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class EmailRequest
    {
        public string Email { get; set; }
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (await _authService.ValidateCredentials(request.Email, request.Password))
        {
            var token = await _authService.GenerateToken(request.Email);
            if (token != null)
                return Ok(new { token });
        }
        return Unauthorized();
    }

    [HttpPost("loginWithUserName")]
    public async Task<IActionResult> loginWithUserName([FromBody] LoginRequestUserName request)
    {
        // Verifica las credenciales utilizando el nombre de usuario y la contraseña
        if (await _authService.ValidateCredentialsByUsername(request.UserName, request.Password))
        {
            // Genera el token de acceso si las credenciales son válidas
            var token = await _authService.GenerateTokenByUsername(request.UserName);
            if (token != null)
            {
                // Devuelve el token en caso de éxito
                return Ok(new { token });
            }
        }
        // Devuelve Unauthorized si las credenciales son inválidas
        return Unauthorized();
    }


    [HttpGet("getLoggedUser")]
    public async Task<IActionResult> GetLoggedUser()
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length);
            var user = await _authService.GetLoggedUser(token);
            var principal = _authService.ValidateToken(token);
            if (principal != null)
            {

                return Ok(user);
            }
        }

        return Unauthorized();
    }



    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authService.Register(model);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Registration successful" });
            }

            return BadRequest(new { Errors = result.Errors });
        }

        return BadRequest(new { Message = "Invalid registration request" });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return Ok(new { Message = "Logged out successfully" });
    }
    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword([FromBody] EmailRequest request)
    {
        var res = await _authService.ResetPasswordAsync(request.Email);
        if (res.Succeeded)
        {
            return Ok(new { Message = res });

        }
        return BadRequest(new { Errors = res.Errors });

    }
    [HttpPost("changePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var res = await _authService.ChangePasswordAsync(request);
        if (res.Succeeded)
        {
            return Ok(new { Message = res });

        }
        return BadRequest(new { Errors = res.Errors });
    }



    [HttpGet("getUserRol/{userId}")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var roles = await _authService.GetUserRoles(userId);
        if (roles != null)
        {
            return Ok(roles);
        }
        else
        {
            return NotFound();
        }
    }

 

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
    {
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader.StartsWith("Bearer "))
        {

        var result = await _authService.AssignRoleToUser(userId, roleName); // Implementa este método en tu servicio de autenticación para asignar el rol al usuario
        if (result.Succeeded)
        {
            return Ok(new { Message = $"Rol '{roleName}' asignado al usuario correctamente" });
        }
        else
        {
            return BadRequest(new { Message = $"Error al asignar el rol '{roleName}' al usuario", Errors = result.Errors });
        }
        }

        return Unauthorized();
        ;
    }



}
