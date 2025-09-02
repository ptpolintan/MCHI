using MHCI.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHCI.Application.Interfaces
{
    public interface IUserService
    {
        UserModel? Authenticate(string email, string password);
    }
}
