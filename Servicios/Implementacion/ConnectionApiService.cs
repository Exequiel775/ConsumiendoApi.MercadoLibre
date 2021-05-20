namespace Consumi.Api.ML.Servicios.Implementacion
{
    using System.Threading.Tasks;
    using Interface;
    using System.Net.Http;
    using System;
    using Newtonsoft.Json;
    public class ConnectionApiService<T> : IConnectionApiService<T> where T : class
    {
        public async Task<T> GetConnectionApi(string url, string requestUri)
        {
            var _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(url);

            var request = await _httpClient.GetAsync(requestUri);

            if (!request.IsSuccessStatusCode)
                throw new Exception("Ocurrio un error al obtener una conexion a la API");

            var json = await request.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}