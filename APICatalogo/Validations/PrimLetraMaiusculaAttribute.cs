using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validations
{
    public class PrimLetraMaiusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var primeiraLetra  = value.ToString()[0].ToString();
            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                return new ValidationResult("Primeira letra do nome do produto deve ser maiúscula");
            }

            return ValidationResult.Success;
        }
    }
}
