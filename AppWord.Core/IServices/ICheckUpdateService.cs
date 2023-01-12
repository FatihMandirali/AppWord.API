using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppWord.Model.Models.Request;

namespace AppWord.Core.IServices
{
    public interface ICheckUpdateService
    {
        Task<string> CheckUpdate(CheckUpdateRequest request);
    }
}
