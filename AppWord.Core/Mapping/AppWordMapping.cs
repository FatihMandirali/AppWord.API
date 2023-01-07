using AppWord.Data.Entity;
using AppWord.Model.Models.Response;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWord.Core.Mapping
{
    public class AppWordMapping : Profile
    {
        public AppWordMapping()
        {
            CreateMap<Word, WordResponse>().ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
