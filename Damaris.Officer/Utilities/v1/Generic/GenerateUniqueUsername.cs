using Damaris.Domain.v1.Models.User;
using Damaris.Officer.Data.v1;
using Damaris.Officer.Repositories.v1;
using Damaris.Officer.Repositories.v1.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace Damaris.Officer.Utilities.v1.Generic
{
    public class GenerateUniqueUsername
    {    

       
        public static string GenereateUsername(string firstName, string lastName)
        {
            // Combine first name and last name to create a username
            string username = $"{firstName}{lastName}";
          
            return username;
        }

        
    }
}
