using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IUser
    {
        public void AddUser(BO.User user);

        public void RemoveUser(string userName);

        public void UpdateUser(BO.User user);

        public User? ReadUser(string userName,bool throwexception=false);

        public IEnumerable<BO.User>  ReadAllUsers();

        public bool checkExistId(int id);

    }
}
