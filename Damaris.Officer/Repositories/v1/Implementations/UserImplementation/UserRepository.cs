using Damaris.Domain.v1.Dtos.GenericDtos;
using Damaris.Domain.v1.Models.Account;
using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Data.v1;
using Damaris.Officer.Repositories.v1.Interfaces.UserInterface;

namespace Damaris.Officer.Repositories.v1.Implementations.UserImplementation
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private readonly OfficerDbContext _officerDbContext;
        private readonly ILogger _logger;

        /// <summary>
        /// User repository Constructor
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="connectionString"></param>
        public UserRepository(ILoggerFactory loggerFactory, OfficerDbContext officerDbContext) : base(loggerFactory, officerDbContext)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _officerDbContext = officerDbContext;
        }

        public Task ApplyUserPatchAsync<TEntity>(TEntity? entity, List<PatchDto>? patchDto) where TEntity : User
        {
            throw new NotImplementedException();
        }

        public Task CreateNewUserAsync(Accounts? user, bool twoUser = false)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
