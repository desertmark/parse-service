using System.Data.Odbc;
using System.IO;
using Dapper;

namespace parse_service.Repositories
{
    public class OdbcRepository : IOdbcRepository
    {
        public List<dynamic> Get(IFormFile file, string tableName)
        {
            var path = Path.GetTempFileName();
            file.CopyTo(new FileStream(path, FileMode.Create, FileAccess.ReadWrite));
            var connection = new OdbcConnection($"Driver={{Microsoft Access Driver (*.mdb, *.accdb)}};Dbq={path};");
            var queryText = $"SELECT * FROM {tableName}";
            var data = connection.Query<dynamic>(queryText);
            return data.ToList();
        }
    }
}