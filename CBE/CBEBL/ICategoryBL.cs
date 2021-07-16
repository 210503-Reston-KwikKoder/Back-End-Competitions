using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBEModels;

namespace CBEBL
{
    public interface ICategoryBL
    {
        /// <summary>
        /// Adds a user, returns added category, null if an error
        /// </summary>
        /// <param name="c">category to add</param>
        /// <returns>Category added, null if not found</returns>
        Task<Category> AddCategory(Category c);
        /// <summary>
        /// Method to return all categories found in the database
        /// </summary>
        /// <returns>List of all categories</returns>
        Task<List<Category>> GetAllCategories();
        /// <summary>
        /// Get a category by it's number name (defined in Octokit.Language)
        /// </summary>
        /// <param name="name">name of language </param>
        /// <returns>category requested</returns>
        Task<Category> GetCategory(int name);
        /// <summary>
        /// Getting category by it's id, not name
        /// </summary>
        /// <param name="id">id in database</param>
        /// <returns>category with a given id</returns>
        Task<Category> GetCategoryById(int id);
    }
}
