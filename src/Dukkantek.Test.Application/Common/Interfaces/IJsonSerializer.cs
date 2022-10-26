namespace Dukkantek.Test.Application.Common.Interfaces;

public interface IJsonSerializer
{
    string Serialize<T>(T obj);

    string Serialize<T>(T obj, Type type);

    T Deserialize<T>(string text);
}
