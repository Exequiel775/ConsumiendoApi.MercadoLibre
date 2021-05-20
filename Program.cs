namespace Consumi.Api.ML
{
    using System;
    using System.Threading.Tasks;
    using Generator;
    public class Program
    {
        public static async Task Main(string[] args)
        {
            int selectedOptioon = 0;
            do
            {
                Console.Clear();
                Console.WriteLine("Consulta de productos por vendedor");
                Console.WriteLine("Al ingresar el id de los vendedores debe separarlos por un espacio y una coma.");
                Console.WriteLine("Por ejemplo: 123456, 431124, 12312312, 312312");
                Console.Write("Por favor ingrese el id de los usuarios a buscar: ");
                string usuarios = Console.ReadLine();

                try
                {
                    Console.Clear();
                    Console.WriteLine("Generando log...");
                    var generator = new GenerateLog();
                    await generator.Generate(usuarios);
                    Console.Clear();
                    Console.WriteLine("El log fue creado exitosamente en el directorio.");
                    Console.Write("¿Desea hacer otra consulta? [1]SI | [0]NO : ");
                    selectedOptioon = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    Console.Write($"Ocurrio un error al generar el log: {e.Message}");
                }
            }
            while(selectedOptioon != 0);
        }
    }
}
