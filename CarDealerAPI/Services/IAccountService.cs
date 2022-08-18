using CarDealerAPI.DTOS;

namespace CarDealerAPI.Services
{
    public interface IAccountService
    {
        void RegisterUser(UserCreateDTO userDto);
        string GenerateToken(UserLoginDTO login);
    }
}