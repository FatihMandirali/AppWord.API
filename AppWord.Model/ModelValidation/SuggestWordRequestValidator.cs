using AppWord.Model.Models.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Model.ModelValidation
{
    public class SuggestWordRequestValidator : AbstractValidator<SuggestWordRequest>
    {
        public SuggestWordRequestValidator()
        {
            RuleFor(x => x.WordTr).NotEmpty().NotNull().WithMessage("Lütfen kelimenin türkçesini uygun doldurun");
            RuleFor(x => x.WordEn).NotEmpty().NotNull().WithMessage("Lütfen kelmenin ingilizcesini uygun doldurun");
        }
    }
}
