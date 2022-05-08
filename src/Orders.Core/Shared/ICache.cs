using System.Threading.Tasks;

namespace Orders.Core.Shared;

public interface ICache
{
    Task Store<T>(
        string key,
        T value,
        params string[] @params
    );

    Task<T> Get<T>(string key);
    Task<bool> Delete(string key);
}