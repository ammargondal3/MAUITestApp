using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_CRUD_SQLite.Models
{
    public class CountryValidator : AbstractValidator<CountryVM>
    {
        public CountryValidator()
        {
            RuleFor(x => x.CountryName).NotEmpty().WithMessage("Enter Country Name");
        }
    }
}
