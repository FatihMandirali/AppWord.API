using AppWord.Model.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.ModelValidation
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull().WithMessage("Lütfen kullanıcı alanını uygun doldurun");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Lütfen şifre alanını uygun doldurun");
        }

    }
}
