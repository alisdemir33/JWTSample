using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.Auth
{
    public class TokenUser
    {
        Token token;
        User user;

        public Token Token { get => token; set => token = value; }
        public User User { get => user; set => user = value; }
    }
}
