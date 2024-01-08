using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi
{
    public interface ICrud<T> where T:class
    {
        /// <summary>
        /// Creates new entity object in DAL
        /// </summary>
        /// <param name="item">entity object</param>
        /// <returns></returns>
        int Create(T item);

        /// <summary>
        /// Reads entity object by its ID 
        /// </summary>
        /// <param name="id">id of entity</param>
        /// <returns></returns>
        T? Read(int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        T? Read(Func<T, bool> filter);

        /// <summary>
        /// Reads all entity objects
        /// </summary>
        /// <returns></returns>
        IEnumerable<T?> ReadAll(Func<T,bool>? filter=null);

        /// <summary>
        /// Updates entity object
        /// </summary>
        /// <param name="item">entity object</param>
        void Update(T item);

        /// <summary>
        /// Deletes an object by its Id
        /// </summary>
        /// <param name="id">ID of the object</param>
        void Delete(int id);


    }
}
