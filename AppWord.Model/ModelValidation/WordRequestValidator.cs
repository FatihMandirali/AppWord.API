using AppWord.Model.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.ModelValidation
{
    public class WordRequestValidator : AbstractValidator<WordRequest>
    {
        public WordRequestValidator()
        {
            RuleFor(x => x.WordTr).NotEmpty().NotNull().WithMessage("Lütfen kelimeti girin");
            RuleFor(x => x.WordEn).NotEmpty().NotNull().WithMessage("Lütfen kelime anlamını");
        }
    }
}
