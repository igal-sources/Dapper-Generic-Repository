using DapperGenericRepository.Models;
using System;
using System.Collections.Generic;

namespace DapperGenericRepository.Contracts
{
    public interface ICategoriesRepository : IDisposable
    {
        IEnumerable<categories> GetAllCategories(string where = null);

        void InsertCategories(IEnumerable<categories> entities);

        void InsertCategory(categories entity);

        void DeleteCategoryById(int Id);

        void UpdateCategories(IEnumerable<categories> entities);

        void UpdateCategory(categories entity);

        string GetIdColumn();

        IEnumerable<categories> GetCategories(string spName);

        IEnumerable<categories> GetCategoryById(string spName, int categoryId);
    }
}