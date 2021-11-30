using System.Threading.Tasks;
using CarRental.Domain.Dto;
using CarRental.Domain.Ports.In;
using CarRental.Domain.Ports.Out;

namespace CarRental.Domain.Services
{
    public class UserService: IGetUserDetailsUseCase
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<UserDetails> GetUserDetails(string userId)
        {
            return _userRepository.GetUserDetailsAsync(userId);
        }
    }
}