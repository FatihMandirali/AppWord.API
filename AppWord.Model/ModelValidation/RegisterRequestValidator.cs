using AppWord.Model.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.ModelValidation
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull().WithMessage("Lütfen kullanıcı alanını uygun doldurun");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Lütfen şifre alanını uygun doldurun");
            RuleFor(x => x.Email).EmailAddress().NotEmpty().NotNull().WithMessage("Lütfen email alanını uygun doldurun");
        }
    }
}
