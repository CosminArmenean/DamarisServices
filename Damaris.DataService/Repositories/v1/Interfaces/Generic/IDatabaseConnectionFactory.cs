using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damaris.DataService.Repositories.v1.Interfaces.Generic
{
    public interface IDatabaseConnectionFactory
    {
        Task<MySqlConnection> CreateConnectionAsync();
    }
}
