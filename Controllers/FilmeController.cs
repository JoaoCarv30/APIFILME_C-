using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {

       private FilmeContext _context;
       private IMapper _mapper;
        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context; //Injeção de dependência.
            _mapper = mapper;
        }
       

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto); //Cria um novo filme.
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

        [HttpPut("{id}")]

        public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id); //Busca o filme pelo id.

            if (filme == null) //Se o filme não existir, retorna NotFound.
            {
                return NotFound();
            }

            _mapper.Map(filmeDto, filme); //Atualiza o filme com as informações do filmeDto.

            _context.SaveChanges();

            return NoContent();
        }
    }
}

