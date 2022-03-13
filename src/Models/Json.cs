using System.Dynamic;

namespace parse_service;

public class Json
{
    public dynamic obj = new ExpandoObject();

    public void setKey(String key, object value)
    {
        ((IDictionary<string, object>)obj).Add(key, value);
    }
}
