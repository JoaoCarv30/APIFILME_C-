using FilmesApi.Data;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {

       private FilmeContext _context;
        public FilmeController(FilmeContext context)
        {
            _context = context; //Injeção de dependência.
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] Filme filme)
        {
            _context.Filmes.Add(filme); //Adiciona o filme no contexto.
            _context.SaveChanges(); //Salva as alterações no banco de dados.
          return  CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme); //Retorna o filme criado.
          
        }

        [HttpGet]
        public IEnumerable<Filme> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            //return filmes; //Retorna a lista de filmes.
            // return filmes.Skip(0).Take(3); //Retorna os 3 primeiros filmes apenas.
            return _context.Filmes.Skip(skip).Take(take); //Retorna os filmes de acordo com a paginação.
                                                 // A URL DIGITADA FICARIA ASSIM: https://localhost:5074/Filme?skip=0&take=3 ME RETORNANDO OS 3 PRIMEIROS FILMES.
                                                 //CASO EU N PASSE O SKIP E O TAKE NA URL ELE VAI RETORNAR OS PRIMEIROS 50 FILMES.

        }

        [HttpGet("{id}")]
        public IActionResult? RecuperaFilmePorId(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }
            return Ok(filme);
        }
    }
}

