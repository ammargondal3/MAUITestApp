using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_CRUD_SQLite.Models
{
    public class CityValidator : AbstractValidator<CityVM>
    {
        public CityValidator()
        {
            RuleFor(x => x.CountryId).NotEmpty().WithMessage("Select your country");
            RuleFor(x => x.CityName).NotEmpty().WithMessage("Enter City Name");
        }
    }
}
