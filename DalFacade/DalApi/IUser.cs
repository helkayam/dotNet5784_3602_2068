namespace DalApi;
using DO;

public interface IUser
{
    string Create(User item);

    User? Read(string id, bool throwAnException = false);


    IEnumerable<User?> ReadAll();

    void Update(User item);

    void Delete(string id);

    void DeleteAll();

    string getEmail();

    string getPassword();
}

