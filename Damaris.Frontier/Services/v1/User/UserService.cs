using Confluent.Kafka;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Damaris.Frontier.Services.v1.User
{
    public class UserService 
    {
        private readonly IDistributedCache _cache;
        private readonly IProducer<string, string> _kafkaProducer;


        public UserService(IDistributedCache cache, IProducer<string, string> kafkaProducer)
        {
            
            _cache = cache;
            _kafkaProducer = kafkaProducer;
        }

        //public User Authenticate(string username, string password)
        //{
        //    // Check cache first
        //    var cachedUser = _cache.GetString(username);
        //    if (!string.IsNullOrEmpty(cachedUser))
        //    {
        //        return JsonSerializer.Deserialize<User>(cachedUser);
        //    }

        //    // Perform user authentication logic here (query database)
        //    var user = _dbContext.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

        //    if (user != null)
        //    {
        //        // Cache authenticated user
        //        var cacheOptions = new DistributedCacheEntryOptions
        //        {
        //            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache for 10 minutes
        //        };
        //        _cache.SetString(username, JsonSerializer.Serialize(user), cacheOptions);

        //        // Send user authentication event to Kafka
        //        var kafkaMessage = new Message<string, string>
        //        {
        //            Key = user.Id.ToString(),
        //            Value = "User logged in"
        //        };
        //        _kafkaProducer.Produce("user-authentication-topic", kafkaMessage);
        //    }

        //    return user;
        //}
    }
}
