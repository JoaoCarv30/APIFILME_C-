using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesApi.Data.Dtos
{
    public class CreateFilmeDto
    {
     
        [Required(ErrorMessage = "O campo Titulo é obrigatório.")]
        public string Titulo { get; set; }
        [Required(ErrorMessage = "O campo Genero é obrigatório.")]
        [StringLength(30, ErrorMessage = "O campo Gênero não pode ter mais que 30 caracteres.")]
        public string Genero { get; set; }
        [Required(ErrorMessage = "O campo Duração é obrigatório.")]
        [Range(1, 600, ErrorMessage = "A duração deve ter no mínimo 1 e no máximo 600 minutos.")]
        public int Duracao { get; set; }
    }
}