namespace Consumi.Api.ML.Generator
{
    using System.IO;
    using System.Collections.Generic;
    using System;
    using Clases;
    using Servicios.Interface;
    using Servicios.Implementacion;
    using System.Linq;
    using System.Threading.Tasks;

    public class GenerateLog
    {
        private readonly IConnectionApiService<ApiMercadoLibre.Root> _apiServiceRoot;
        private readonly IConnectionApiService<ApiMercadoLibre.Categorias> _apiServiceCategories;
        private string urlApi = "https://api.mercadolibre.com";
        private string path = Directory.GetCurrentDirectory();
        public GenerateLog()
        : this(new ConnectionApiService<ApiMercadoLibre.Root>(), new ConnectionApiService<ApiMercadoLibre.Categorias>())
        {

        }

        public GenerateLog(IConnectionApiService<ApiMercadoLibre.Root> apiServiceRoot, IConnectionApiService<ApiMercadoLibre.Categorias> apiServiceCategories)
        {
            _apiServiceRoot = apiServiceRoot;
            _apiServiceCategories = apiServiceCategories;
        }

        public async Task Generate(string id_users)
        {
            try
            {
                string name = $"Log Fecha {DateTime.Now.Year}_{DateTime.Now.Month}_{DateTime.Now.Day} {DateTime.Now.Hour} {DateTime.Now.Minute} {DateTime.Now.Second} {DateTime.Now.Millisecond}.txt";

                StreamWriter _str = new StreamWriter($"{path}/{name}", false);

                var data = await GetSellerData(id_users);

                if (data.Item1 != null)
                {
                    foreach (var item in data.Item1.results)
                    {
                        var categorie = await _apiServiceCategories.GetConnectionApi(urlApi, $"/categories/{item.category_id}");
                        _str.Write($"User ID: {data.Item1.seller.id} {Environment.NewLine}Title ID:{item.id}{Environment.NewLine}Title Descripcion:{item.title}{Environment.NewLine}Category ID:{item.category_id}{Environment.NewLine}Category Name:{categorie.name}{Environment.NewLine}-{Environment.NewLine}");
                    }
                }

                if (data.Item2.Any())
                {
                    _str.Write($"De el/los siguientes ids no se encontraron resultados.{Environment.NewLine}");
                    foreach (var item in data.Item2)
                    {
                        _str.Write($"{item}{Environment.NewLine}");
                    }
                }

                _str.Close();
            }
            catch (Exception e)
            {
                throw new Exception($"Ocurrio el siguiente error al generar el log... {e.Message}");
            }
        }

        /*
        ==============================================================
        ====================== PRIVATE METHODS =======================
        ==============================================================
        */

        private async Task<(ApiMercadoLibre.Root, List<string>)> GetSellerData(string users)
        {
            var arrayUsers = users.Replace(",", "").Split(" ");

            var result = new ApiMercadoLibre.Root();
            var warning = new List<string>();

            for (int i = 0; i < arrayUsers.Length; i++)
            {
                var data = await _apiServiceRoot.GetConnectionApi(urlApi, $"/sites/MLA/search?seller_id={arrayUsers[i]}");

                if (!data.results.Any())
                {
                    warning.Add(arrayUsers[i]);
                    continue;
                }

                result = data;
            }

            return (result, warning);
        }

        private void CreateDirectory()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}