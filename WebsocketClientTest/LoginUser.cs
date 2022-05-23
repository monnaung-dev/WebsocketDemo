using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsocketClientTest
{
    public class LoginUser
    {
        public string login_id { get; set; }
        public string password { get; set; }
    }

    public class Root
    {
        public LoginUser User { get; set; }
    }
}
