using System.Collections.Generic;

namespace NoTricks.Data.Repositories {
    public interface IMappingRepository<T> {
        /// <summary>
        /// Returns all objects in the repository of type T
        /// </summary>
        /// <returns>Collection Of T</returns>
        IEnumerable<T> GetAll();
        
        /// <summary>
        /// Inserts the mapping to a database.
        /// </summary>
        /// <param name="model">The model to insert</param>
        /// <returns>True if inserted</returns>
        bool Insert(T model);
        
        /// <summary>
        /// Checks if a mapping exist.
        /// </summary>
        /// <param name="model">The mapping to check if exist.</param>
        /// <returns>True if exist.</returns>
        bool Exist(T model);
        
        /// <summary>
        /// Removes a mapping.
        /// </summary>
        /// <param name="model">The mapping to remove.</param>
        /// <returns>true if removed.</returns>
        bool Remove(T model);
    }
}