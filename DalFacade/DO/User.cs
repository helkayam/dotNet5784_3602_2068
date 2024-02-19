using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO;
public record User
    (
    int Id
    
    )
{
    public User() : this(0) { }

    public bool isAdmin {  get; set; }=false; 

    public string PassWord { get; set; } = "";

    public string UserName { get; set; } = "";
}

