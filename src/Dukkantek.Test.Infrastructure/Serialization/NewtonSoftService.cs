using Dukkantek.Test.Application.Common.Interfaces;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Dukkantek.Test.Infrastructure.Serialization;

public class NewtonSoftService : IJsonSerializer
{
    public T Deserialize<T>(string text)
    {
        return JsonConvert.DeserializeObject<T>(text);
    }

    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = new List<JsonConverter>
            {
                 new StringEnumConverter(new CamelCaseNamingStrategy())
            }
        });
    }

    public string Serialize<T>(T obj, Type type)
    {
        return JsonConvert.SerializeObject(obj, type, new());
    }
}