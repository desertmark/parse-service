using parse_service;
namespace parse_service.Managers
{
    public interface IParseManager
    {
        List<dynamic> FromXls(IFormFile file, ParseOptions options);
    }
}
