using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;
using DalApi;
using DO;

internal class UserImplementation:IUser
{
    readonly string s_users_xml = "users";

    public string Create(DO.User item)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml );

        DO.User? userSameUserName = users.Find(x => x.UserName == item.UserName);
        if (userSameUserName != null)
        {
            throw new DalDoesNotExistException($"User with UserName={item.UserName} already exist");
        }
        users.Add(item);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
          return item.UserName;
    }


    public void Delete(string userName)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml );
        DO.User ? userToRemove = users.Find(x => x.UserName  == userName);
        if (userToRemove == null)
        {
            throw new DalDoesNotExistException($"User with UserName={userName } does not exist");
        }
       
       users.RemoveAll(t => t.UserName==userName );
        XMLTools.SaveListToXMLSerializer<DO.User>(users,s_users_xml );

    }


    public DO.User? Read(string userName, bool throwAnException=false)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml );
        if (users.Any(usr => usr.UserName  == userName ) == false)
            if (throwAnException)
                throw new DalDoesNotExistException($"User with UserName={userName} does not exist");
            else
                return null;


        DO.User saveItem = users.Find(saveItem => saveItem.UserName==userName )!;
        return saveItem;
    }

    

    public IEnumerable<DO.User> ReadAll()
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml );
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
        return from user in users
               select user;
    }

    public void Update(DO.User item)
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        DO.User? user = users.Find(t => t.UserName  == item.UserName );
        if (user == null)
            throw new DalDoesNotExistException($"User with UserName ={ item.UserName } does not exist");
        else
        {
            users.RemoveAll(t => t.UserName  == item.UserName);
            users.Add(item);
        }
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);

    }

    public void DeleteAll()
    {
        List<DO.User> users = XMLTools.LoadListFromXMLSerializer<DO.User>(s_users_xml);
        users.Clear();
        XMLTools.SaveListToXMLSerializer<DO.User>(users, s_users_xml);
    }

    public string getEmail()
    {
        return Config.email;
    }

    public string getPassword()
    {
        return Config.password;
    }



}
