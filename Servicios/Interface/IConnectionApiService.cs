namespace Consumi.Api.ML.Servicios.Interface
{
    using System.Threading.Tasks;
    public interface IConnectionApiService<T> where T : class
    {
        Task<T> GetConnectionApi(string url, string requestUri);
    }
}