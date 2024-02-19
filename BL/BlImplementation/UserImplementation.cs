using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;

internal class UserImplementation: BlApi.IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void AddUser(BO.User user)
    {
        DO.User newUser = new DO.User() { PassWord=user.Password, UserName =user.UserName , isAdmin =user.IsAdmin};

        try
        {
            if (newUser.PassWord.Length >= 8 && newUser.PassWord.Any(t => (int)t >= 65 && (int)t <= 90) && newUser.PassWord.Any(t => (int)t >= 97 && (int)t <= 122))
                _dal.User.Create(newUser);
            else
                throw new BO.BlInvalidGivenValueException($"This password={user.Password} is not valid");
        }
        catch(DO.DalAlreadyExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with UserName={user.UserName} already exists", ex);

        }

    }

    public void RemoveUser(string userName)
    {
        try
        {
            _dal.User.Delete(userName);
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with UserName={userName} doesnt exists", ex);
        }
    }

    public IEnumerable<BO.User> ReadAllUsers()
    {
        return from users in _dal.User.ReadAll()
               select new BO.User() { IsAdmin =users.isAdmin, UserName =users.UserName, Password=users.PassWord };
    }

     public BO.User? ReadUser(string userName, bool throwexception = false)
     { 
        try
        {
            _dal.User.Read(userName, true);
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlAlreadyExistsException($"User with UserName={userName} doesnt exists", ex);
        }

        DO.User user = _dal.User.Read(userName);
        return new BO.User { IsAdmin = user.isAdmin, Password = user.PassWord, UserName = user.UserName, };
     }

    public void UpdateUser(BO.User user)
    {
       
        DO.User updUser=new DO.User() { PassWord =user.Password , UserName =user.UserName , isAdmin =user.IsAdmin};
        try
        {
            if (user.Password.Length >= 8 && user.Password.Any(t => (int)t >= 65 && (int)t <= 90) && user.Password.Any(t => (int)t >= 97 && (int)t <= 122))
                _dal.User.Update(updUser);
            else
                throw new BO.BlInvalidGivenValueException($"This password={user.Password} is not valid")
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException($"User with userName={user.UserName} does Not exist", ex);

        }


    }



}
