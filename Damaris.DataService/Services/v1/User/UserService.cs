using AutoMapper;
using Damaris.DataService.Repositories.v1.Interfaces.Generic;
using Damaris.DataService.Repositories.v1.Interfaces.UserInterfaces;
using Damaris.Domain.v1.Dtos.UserDtos;

namespace Damaris.DataService.Services.v1.User
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);

            // Optionally, you can use AutoMapper to map the User entity to a DTO.
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}
