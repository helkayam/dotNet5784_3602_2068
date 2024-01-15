using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    /// <summary>
    /// Defining a generic common interface that will define all CRUD operations within it.
    /// Within the interface, 5 generic CRUD interface methods are defined: Create, Read, ReadAll, Update, Delete.
    /// </summary>
    /// <typeparam name="T">When any entity inherits from this generic interface it will send the name of the entity in place of this T </typeparam>
    public interface ICrud<T> where T:class
    {
        
        int Create(T item);

        T? Read(int id);
       
        T? Read(Func<T, bool> filter);

        IEnumerable<T?> ReadAll(Func<T,bool>? filter=null);

        void Update(T item);

        void Delete(int id);

        void DeleteAll();   


    }
}
