using EcommerceBackend.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackend.Domain.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
