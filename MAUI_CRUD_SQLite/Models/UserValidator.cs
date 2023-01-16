using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_CRUD_SQLite.Models
{
    public class UserValidator : AbstractValidator<UserVM>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Enter your name");
            RuleFor(x => x.CountryId).NotEmpty().WithMessage("Select your country");
            RuleFor(x => x.CityId).NotEmpty().WithMessage("Select your city");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Select your gender");
        }
    }
}
