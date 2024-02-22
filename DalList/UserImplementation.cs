namespace Dal;
using DalApi;
using DO;
using System;
using System.Collections.Generic;


internal class UserImplementation : IUser
{
    public string Create(User item)
    {
        if (DataSource.Users.Any(user => user.UserName == item.UserName))
        {
            DataSource.Users.Add(item);
        }
        else
            throw new DalAlreadyExistException($"User with userName={item.UserName} already exist");
        return item.UserName;
    }



    public void Delete(string id)
    {

        DO.User? user = DataSource.Users.Find(user => user.UserName == id);
        if (user == null)
            throw new DalDoesNotExistException($"User with UserName={id} does not exist");

        DataSource.Users.RemoveAll(user => user.UserName == id);
    }

    

    public void DeleteAll()
    {
        foreach (var item in DataSource.Workers)
        {
            DataSource.Workers.Remove(item);
        }
    }

    public User? Read(string id, bool throwAnException = false)
    {
        if (DataSource.Users.Any(user => user.UserName == id))
            if (throwAnException)
                throw new DalDoesNotExistException($"Worker with id={id} does not exist");
            else
                return null;

        DO.User newUser = DataSource.Users.Find(newUser => newUser.UserName == id);


        return newUser;
    }





    public IEnumerable<User?> ReadAll()
    {

        return from user in DataSource.Users
               select user;
    }



    public void Update(User item)
    {
        DO.User u = DataSource.Users.Find(u => u.UserName == item.UserName);
        if (u == null)
            throw new DalDoesNotExistException($"User with UserName={item.UserName} does not exist");
        else
        {
            DataSource.Users.RemoveAll(user => user.UserName == item.UserName);
            DataSource.Users.Add(item);
        }
    }


}

