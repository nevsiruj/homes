using AlmacenSystem.Infrastructure.Repositories.Common;
using Astuc.Domain.DTOs;
using AutoMapper;
using Domain.DTOs;
using EIRL.Application.Services;
using EIRL.Application.Services.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Modules.UsersModule.Services
{

    public interface IUserService : IGenericService<UserDTO>
    {
        Task<IdentityResult> CreateUserWithImageAsync(UserCreateDTO userDto);
        Task<IdentityResult> UpdateUserWithImageAsync(string userId, UpdateUserDTO userDto);

        //Task<List<UserDTO>> GetUsersVendedores();
        //Task<List<UserDTO>> GetUsersLideres();
        Task<List<UserDTO>> GetUsersAdmin();
        Task<List<UserDTO>> GetUsersAgente();
        Task<int> GetClientCount();
        Task<bool> Update(UserDTO user);

    }
    public class UserService : GenericService<ApplicationUser, UserDTO>, IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper; // Agregado para reemplazar el uso del mapper del repositorio
        private readonly string _imagesFolder = "UserImages"; // Asegúrate de que esta carpeta existe.
        private readonly ProjectDbContext _projectDbContext;
        private readonly IAuthService _authService;

        public UserService(IMapper mapper, UserManager<ApplicationUser> userManager, ProjectDbContext projectDbContext, IAuthService authService) : base(null, mapper)
        {
            _userManager = userManager;
            _mapper = mapper; // Establecer el mapper
            _projectDbContext = projectDbContext;
            _authService = authService;
        }

        public async Task<List<UserDTO>> GetUsersAdmin()
        {
            return await _authService.GetUsersAdmin();
        }
        public async Task<List<UserDTO>> GetUsersAgente()
        {
            return await _authService.GetUsersAgente();
        }
        public async Task<bool> Update(UserDTO user)
        {
            return await _authService.Update(user);
        }

        public async Task<int> GetClientCount()
        {
            var clientCount = await _projectDbContext.Clientes.CountAsync();
            return clientCount;
        }

       
        public async Task<IdentityResult> CreateUserWithImageAsync(UserCreateDTO userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);

            //if (userDto.ProfileImage != null)
            //{
            //    // Verifica si el directorio existe, si no, créalo
            //    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), _imagesFolder);
            //    if (!Directory.Exists(directoryPath))
            //    {
            //        Directory.CreateDirectory(directoryPath);
            //    }

            //    // Guarda la imagen en el sistema de archivos.
            //    var imagePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + Path.GetExtension(userDto.ProfileImage.FileName));
            //    using (var fileStream = new FileStream(imagePath, FileMode.Create))
            //    {
            //        await userDto.ProfileImage.CopyToAsync(fileStream);
            //    }

            //    // Establece la ruta de la imagen en el usuario.
            //    user.ProfileImagePath = imagePath;
            //}

            // Crea el usuario.
            return await _userManager.CreateAsync(user, userDto.Password);
        }

       


        public async Task<IdentityResult> UpdateUserWithImageAsync(string userId, UpdateUserDTO userDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Aquí manejas el caso de no encontrar el usuario, por ejemplo, lanzando una excepción o retornando un resultado de error.
                throw new KeyNotFoundException($"No se encontró un usuario con el ID {userId}.");
            }

            // Mapea las propiedades editables desde el DTO al ApplicationUser,
            // excluyendo la imagen para manejarla por separado.
            _mapper.Map(userDto, user);

            //if (userDto.ProfileImage != null)
            //{
            //    // Elimina la imagen anterior si existe.
            //    if (!string.IsNullOrEmpty(user.ProfileImagePath))
            //    {
            //        var existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), user.ProfileImagePath);
            //        if (File.Exists(existingImagePath))
            //        {
            //            File.Delete(existingImagePath);
            //        }
            //    }

            //    // Verifica si el directorio existe, si no, créalo
            //    var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), _imagesFolder);
            //    if (!Directory.Exists(directoryPath))
            //    {
            //        Directory.CreateDirectory(directoryPath);
            //    }

            //    // Guarda la nueva imagen en el sistema de archivos.
            //    var newImagePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + Path.GetExtension(userDto.ProfileImage.FileName));
            //    using (var fileStream = new FileStream(newImagePath, FileMode.Create))
            //    {
            //        await userDto.ProfileImage.CopyToAsync(fileStream);
            //    }

            //    // Actualiza la ruta de la imagen en el usuario.
            //    user.ProfileImagePath = Path.GetRelativePath(Directory.GetCurrentDirectory(), newImagePath);
            //}

            // Aquí continúas con la actualización del usuario en la base de datos.
            var result = await _userManager.UpdateAsync(user);
            return result;
        }


    }
}
