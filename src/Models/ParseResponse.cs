namespace parse_service;

public class ParseResponse
{
    public ParseResponse(dynamic data, String type)
    {
        Data = data;
        Type = type;
    }
    public dynamic? Data { get; set; }
    public String? Type { get; set; }
}
