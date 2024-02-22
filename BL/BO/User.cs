using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class User
    {
        public int Id { get; set; } 
        public string UserName {  get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }   

        public string Name { get; set; }

        public string Email { get; set; }

    }
}
