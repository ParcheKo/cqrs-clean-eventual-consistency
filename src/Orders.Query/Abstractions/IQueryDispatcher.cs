using System.Threading.Tasks;

namespace Orders.Query.Abstractions
{
    public interface IQueryDispatcher
    {
        Task<TModel> ExecuteAsync<TModel>(IQuery<TModel> query);
    }
}