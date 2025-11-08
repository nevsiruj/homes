using Microsoft.AspNetCore.Identity;

using System.Text;

using Microsoft.AspNetCore.Http;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Astuc.Domain.DTOs;
using MailKit.Security;
using MimeKit;
using Domain.DTOs;
using AutoMapper;


public interface IAuthService
{
    Task<SignInResult> Login(string email, string password);
    //Task<ApplicationUser> GetLoggedUser(string token);

    /// <summary>
    /// /
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<UserDTO> GetLoggedUser(string token);
    Task<List<UserDTO>> GetUsersWithRoles();
    //Task<List<UserDTO>> GetUsersVendedores();
    //Task<List<UserDTO>> GetUsersLideres();
    Task<List<UserDTO>> GetUsersAdmin(); 
    Task<List<UserDTO>> GetUsersAgente();
    /// <summary>
    /// /
    /// </summary>
    /// <returns></returns>

    Task Logout();
    Task<int?> GetUserTenantId();


    Task<IEnumerable<ApplicationUser>> GetUsers();
    Task<IdentityResult> Register(RegisterRequest model);
    Task<string> GenerateTokenByUsername(string username);
    Task<string> GenerateToken(string email);
    ClaimsPrincipal ValidateToken(string token);
    Task<bool> ValidateCredentials(string email, string password);
    Task<bool> ValidateCredentialsByUsername(string username, string password);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<IdentityResult> ResetPasswordAsync(string email);
    Task<IdentityResult> ChangePasswordAsync(ChangePasswordRequest request);
    Task<bool> Update(UserDTO user);
    Task<IdentityResult> AssignRoleToUser(string userId, string roleName);
    Task<IEnumerable<string>> GetUserRoles(string userId);
}

public class AgenteService : IAuthService
{
    // ... (como anteriormente)

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;


    public AgenteService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _roleManager = roleManager;
        _mapper = mapper;

    }


    private readonly string _secretKey = "MySuperSecretKeyWithAtLeast256Bits"; // Cambia esto por una clave secreta real

    public async Task<string> GenerateToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);  // Obtén los roles del usuario

        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
            };

        // Agrega los roles como claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateTokenByUsername(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return null;

        var roles = await _userManager.GetRolesAsync(user);  // Obtén los roles del usuario

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username),
    };

        // Agrega los roles como claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    public ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_secretKey);
        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    //public async Task<SignInResult> Login(string email, string password)
    //{
    //    var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
    //    if (result.Succeeded)
    //    {
    //        var user = await _userManager.FindByEmailAsync(email);
    //        var token = await GenerateJwtToken(user);

    //        _httpContextAccessor.HttpContext.Response.Headers.Add("Access-Token", token);


    //        var claims = new List<Claim>
    //        {
    //            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
    //            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    //            new Claim("TenantId", user.TenantId.ToString()),
    //             new Claim(ClaimTypes.Email, user.Email),
    //        };


    //        var claimsIdentity = new ClaimsIdentity(claims, "Jwt");
    //        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
    //    }

    //    return result;
    //}

    public async Task<SignInResult> Login(string email, string password)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(email);
            //if()

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = BuildClaims(user, userRoles);
            var token = GenerateToken(claims);
            _httpContextAccessor.HttpContext.Response.Headers.Add("Access-Token", token);

            var claimsIdentity = new ClaimsIdentity(claims, "Jwt");
            _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
        }

        return result;
    }
    public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null) return IdentityResult.Failed();

        var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);


        return result;


    }
    public async Task<IdentityResult> ResetPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "NotFound" });

        var newPassword = GenerateRandomPassword(user);
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
        var message = $"Tu nueva contraseña es: {newPassword}\n";

        await SendEmailAsync(email, "Nueva contraseña", message);

        if (result.Succeeded) return result;

        return IdentityResult.Failed();


    }
    private string GenerateRandomPassword(ApplicationUser user)
    {
        string seed = user.UserName + user.PhoneNumber + user.Nombre;

        // Convertir la semilla a un array de bytes
        byte[] bytes = Encoding.UTF8.GetBytes(seed);

        // Calcular un hash de la semilla
        int hash = BitConverter.ToInt32(System.Security.Cryptography.MD5.Create().ComputeHash(bytes), 0);

        // Usar el hash para generar una cadena aleatoria
        Random random = new Random(hash);
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        const int passwordLength = 10;
        var password = new char[passwordLength];
        for (int i = 0; i < password.Length; i++)
        {
            password[i] = chars[random.Next(chars.Length)];
        }
        var finalPass = new string(password) + ".";
        return finalPass;

    }


    private List<Claim> BuildClaims(ApplicationUser user, IList<string> roles = null)
    {
        var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

        if (roles != null)
        {
            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }
        }

        return claims;
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
        var symmetricSecurityKey = new SymmetricSecurityKey(key);
        var creds = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(1));

        var token = new JwtSecurityToken(
            "yourdomain.com",
            "yourdomain.com",
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }



    public class UserResult
    {
        public string Id { get; set; }
        public Dictionary<string, string> Claims { get; set; }
    }


    public async Task<UserDTO> GetLoggedUser(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null) return null; // Si no se puede validar el token, retorna null de inmediato.

        var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
        if (emailClaim == null) return null; // Si no hay claim de correo electrónico/nombre, retorna null.

        // Intenta encontrar el usuario por correo electrónico.
        var user = await _userManager.FindByEmailAsync(emailClaim.Value);

        // Si no se encuentra por correo electrónico, intenta por nombre de usuario.
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(emailClaim.Value);
        }

        if (user == null) return null; // Si no se encuentra el usuario ni por correo electrónico ni por nombre, retorna null.

        // En este punto, el usuario ha sido encontrado.
        // Mapea el usuario a UserDTO.
        var userDto = _mapper.Map<UserDTO>(user);

        // Obtiene los roles del usuario si es necesario.
        var roles = await GetUserRoles(user.Id);
        if (roles != null && roles.Any()) // Asegúrate de que roles no sea null y tenga elementos.
        {
            userDto.Rol = roles.FirstOrDefault(); // Asigna el primer rol al DTO.
        }

        return userDto; // Retorna el DTO, que ahora incluye el rol.

    }



    //public async Task<ApplicationUser> GetLoggedUser()
    //{
    //    try
    //    {
    //        var email = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
    //        if (email == null)
    //        {
    //            return null;
    //        }

    //        return await _userManager.FindByEmailAsync(email);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Ocurrió un error específico: " + ex.Message, ex);
    //    }
    //}



    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<int?> GetUserTenantId()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();
            if (authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length);
                var user = await GetLoggedUser(token);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener el tenantId del usuario: {ex.Message}");
        }

        return null;
    }


    public async Task<IEnumerable<string>> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        return await _userManager.GetRolesAsync(user);
    }


    public async Task<IEnumerable<ApplicationUser>> GetUsers()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<List<UserDTO>> GetUsersWithRoles()
    {
        var usersWithRoles = new List<UserDTO>();

        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            UserDTO newUser = _mapper.Map<UserDTO>(user);
            newUser.Rol = userRoles[0];
            usersWithRoles.Add(newUser);
        }

        return usersWithRoles;
    }
    public async Task<List<UserDTO>> GetUsersAgente()
    {
        var usersWithRoles = new List<UserDTO>();

        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            UserDTO newUser = _mapper.Map<UserDTO>(user);
            newUser.Rol = userRoles.Any() ? userRoles[0] : "Rol no asignado";
            usersWithRoles.Add(newUser);
        }

        return usersWithRoles.Where(u => u.Rol == "Agente").ToList();
    }
    public async Task<List<UserDTO>> GetUsersAdmin()
    {
        var usersWithRoles = new List<UserDTO>();

        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            UserDTO newUser = _mapper.Map<UserDTO>(user);
            newUser.Rol = userRoles.Any() ? userRoles[0] : "Rol no asignado";
            usersWithRoles.Add(newUser);
        }

        return usersWithRoles.Where(u => u.Rol == "Admin").ToList();
    }
    public async Task<bool> Update(UserDTO user)
    {
        var userExists = await _userManager.FindByIdAsync(user.Id);
        if (userExists == null)
        {
            return false;
        }
        await AssignRoleToUser(user.Id, user.Rol);

        userExists.Apellido = user.Apellido;
        userExists.Nombre = user.Nombre;
        userExists.Dni = user.Dni;

        //IVAN
        //mappear los campos para actualizar
        //ej userExists.Especialidad = user.Especialidad





        await _userManager.UpdateAsync(userExists);
        return true;


    }
    public async Task<IdentityResult> Register(RegisterRequest model)
    {
        var user = new ApplicationUser();
        var userType = user.GetType();
        var modelType = model.GetType();

        foreach (var prop in modelType.GetProperties())
        {
            var userProp = userType.GetProperty(prop.Name);
            if (userProp != null && userProp.PropertyType == prop.PropertyType)
            {
                userProp.SetValue(user, prop.GetValue(model));
            }
        }
        // Crear el usuario
        var createUserResult = await _userManager.CreateAsync(user, model.Password);
        if (!createUserResult.Succeeded)
        {
            return createUserResult;
        }

        var roleName = model.Rol; // Suponiendo que el nombre del rol está en el objeto RegisterRequest
        var assignRoleResult = await _userManager.AddToRoleAsync(user, roleName);
        if (!assignRoleResult.Succeeded)
        {
            // Si no se pudo asignar el rol, eliminar el usuario recién creado
            await _userManager.DeleteAsync(user);
            return assignRoleResult;
        }

        return IdentityResult.Success;
    }


    public async Task<bool> ValidateCredentials(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            return true;
        }
        return false;
    }

    public async Task<bool> ValidateCredentialsByUsername(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            return true;
        }
        return false;
    }




 


    public async Task<IdentityResult> AssignRoleToUser(string userId, string roleName)
    {
        // Verificar si el rol existe
        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            // El rol especificado no existe
            // Puedes manejar este caso de acuerdo a tus requerimientos
            return IdentityResult.Failed(new IdentityError { Description = $"El rol '{roleName}' no existe." });
        }

        // Obtener el usuario por su ID
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            // El usuario no existe
            // Puedes manejar este caso de acuerdo a tus requerimientos
            return IdentityResult.Failed(new IdentityError { Description = $"El usuario con ID '{userId}' no existe." });
        }

        // Remover todos los roles actuales del usuario
        var resultRemove = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

        // Verificar si se pudo remover los roles existentes correctamente
        if (!resultRemove.Succeeded)
        {
            return resultRemove;
        }

        // Asignar el nuevo rol al usuario
        var resultAdd = await _userManager.AddToRoleAsync(user, roleName);
        return resultAdd;
    }
    private async Task SendEmailAsync(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();

        var address = "tst2@vylaris.online";
        var pass = "Dev@2023";
        emailMessage.From.Add(new MailboxAddress("Cooperativa Escuelas", address));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("html") { Text = message };
        try
        {

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("dtc031.ferozo.com", 465, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(address, pass);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        catch (Exception e)
        {

            throw e;
        }
    }

    internal async Task GetAllAsync()
    {
        throw new NotImplementedException();
    }
}
