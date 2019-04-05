using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Preguntado.Models.ViewModel.Api;
using Preguntado.Models.ViewModel.categoria;
namespace Preguntado.Controllers
{
    public class ApiController : Controller
    {
        // GET: Api
        public ApiCategoriaResult listado { get; set; }
        public async Task<string> Index()
        {
            string url = "https://opentdb.com/api.php?amount=50&type=multiple&encode=url3986&difficulty=easy";
            string json = await LlamarAPI(url);
            listado = JsonConvert.DeserializeObject<ApiCategoriaResult>(json);
            return "ok";
        }
        

        public static async Task<string> LlamarAPI(string url)
        {
            using (var cliente = new HttpClient())
            {
                var minutos = 60;
                cliente.Timeout = new TimeSpan(0, minutos, 0);
                var result = await cliente.GetAsync(url);
                return await result.Content.ReadAsStringAsync();
            }
        }


        public async Task<string> Create()
        {
            string url = "https://opentdb.com/api.php?amount=50&type=multiple&encode=url3986&difficulty=easy";
            string json = await LlamarAPI(url);
            listado = JsonConvert.DeserializeObject<ApiCategoriaResult>(json);

            //List<ViewModelABMCategoria> lista = listado.results.GroupBy(x => x.category).Select( a=> new ViewModelABMCategoria { Nombre =  , cantidadPreguntas = listado.results.Where(p => p.category == x.category).Count() }).ToList();

            return "ok";

        }


    }
}