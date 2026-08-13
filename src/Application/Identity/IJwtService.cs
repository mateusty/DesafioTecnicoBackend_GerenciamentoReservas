using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Identity
{
    public interface IJwtService
    {
        public string GenerateToken(Guid userId, string username);
    }
}
