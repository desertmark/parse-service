namespace parse_service.Repositories
{
    public interface IOdbcRepository
    {
        List<dynamic> Get(IFormFile file, string tableName);
    }
}