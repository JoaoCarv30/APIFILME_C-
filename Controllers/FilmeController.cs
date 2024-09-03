using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto); //Cria um novo filme.
            _context.Filmes.Add(filme); //Adiciona o filme no contexto.
            _context.SaveChanges(); //Salva as alterações no banco de dados.
            return CreatedAtAction(nameof(RecuperaFilmePorId), new { id = filme.Id }, filme); //Retorna o filme criado.

        }

        [HttpGet]
        public IEnumerable<ReadFilmeDto> RecuperaFilmes([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            // //return filmes; //Retorna a lista de filmes.
            // // return filmes.Skip(0).Take(3); //Retorna os 3 primeiros filmes apenas.
            // return _context.Filmes.Skip(skip).Take(take); //Retorna os filmes de acordo com a paginação.
            //                                               // A URL DIGITADA FICARIA ASSIM: https://localhost:5074/Filme?skip=0&take=3 ME RETORNANDO OS 3 PRIMEIROS FILMES.
            //                                               //CASO EU N PASSE O SKIP E O TAKE NA URL ELE VAI RETORNAR OS PRIMEIROS 50 FILMES.
            return _mapper.Map<List<ReadFilmeDto>>(_context.Filmes.Skip(skip).Take(take)); //Retorna os filmes de acordo com a paginação.

        }

        [HttpGet("{id}")]
        public IActionResult? RecuperaFilmePorId(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme == null)
            {
                return NotFound();
            }
            var filmeDto = _mapper.Map<ReadFilmeDto>(filme); //Cria um filmeDto com as informações do filme.
            return Ok(filmeDto); //Retorna o filmeDto.
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

        [HttpPatch("{id}")]

        public IActionResult AtualizaFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDto> patch)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id); //Busca o filme pelo id.

            if (filme == null) //Se o filme não existir, retorna NotFound.
            {
                return NotFound();
            }

            var filmeParaAtualizar = _mapper.Map<UpdateFilmeDto>(filme); //Cria um filmeDto com as informações do filme.

            patch.ApplyTo(filmeParaAtualizar, ModelState); //Aplica as alterações do patch no filmeDto.

            if (!TryValidateModel(filmeParaAtualizar)) //Se o modelo não for válido, retorna BadRequest.
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(filmeParaAtualizar, filme); //Atualiza o filme com as informações do filmeDto.

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletaFilme(int id)
        {
            var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id); //Busca o filme pelo id.

            if (filme == null) //Se o filme não existir, retorna NotFound.
            {
                return NotFound();
            }

            _context.Remove(filme); //Remove o filme do contexto.

            _context.SaveChanges();

            return NoContent();
        }



    }
}

