using Aplication.Utils;
using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Interfaces
{
    public interface IAuthService
    {
        public Response GetLoginUser(dtoLoginUser loginUser);
    }
}
