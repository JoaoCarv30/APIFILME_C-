using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {

        private static List<Filme> filmes = new List<Filme>();
        private static int id = 0;

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] Filme filme)
        {
            filme.Id = id++;
            filmes.Add(filme);
          return  CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme);
          
        }

        [HttpGet]
        public IEnumerable<Filme> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            //return filmes; //Retorna a lista de filmes.
            // return filmes.Skip(0).Take(3); //Retorna os 3 primeiros filmes apenas.
            return filmes.Skip(skip).Take(take); //Retorna os filmes de acordo com a paginação.
                                                 // A URL DIGITADA FICARIA ASSIM: https://localhost:5074/Filme?skip=0&take=3 ME RETORNANDO OS 3 PRIMEIROS FILMES.
                                                 //CASO EU N PASSE O SKIP E O TAKE NA URL ELE VAI RETORNAR OS PRIMEIROS 50 FILMES.

        }

        [HttpGet("{id}")]
        public IActionResult? RecuperaFilmePorId(int id)
        {
            var filme = filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }
            return Ok(filme);
        }
    }
}

