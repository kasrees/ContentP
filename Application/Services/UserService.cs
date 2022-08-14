using Application.Interfaces.Services;
using Application.Models;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private IOptions<UserAuthSettings> _authorizeSettings;

        public UserService(IUserRepository userRepository, IOptions<UserAuthSettings> authorizeSettings)
        {
            _userRepository = userRepository;
            _authorizeSettings = authorizeSettings;
        }

        public async Task<User?> AuthorizeAsync(Queries.Dtos.AuthorizationRequestDto authorizationRequestDto)
        {
            string encryptionKey = _authorizeSettings.Value.PasswordEncryptionKey;
            User? user = await _userRepository.GetByLoginAsync(authorizationRequestDto.Login);
            string encryptedPassword = EncryptionService.AesEncryptString(encryptionKey, authorizationRequestDto.Password);
            if (user?.Password != encryptedPassword)
                return null;
            return user;
        }
    }
}
