using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class ProdutoDtoUpdateRequest : IValidatableObject
    {
        [Range(1, 9999, ErrorMessage ="Estoque deve ser entre 1 e 9999")]
        public float Estoque { get; set; }
        
        public DateTime DataCadastro { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataCadastro.Date <= DateTime.Now.Date)
            {
                yield return new ValidationResult("A data deve ser maior que data atual",
                    new[] { nameof(this.DataCadastro) });
            }
        }
    }
}
