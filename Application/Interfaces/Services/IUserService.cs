using Application.Queries.Dtos;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User?> AuthorizeAsync(AuthorizationRequestDto authorizationRequestDto);
    }
}
