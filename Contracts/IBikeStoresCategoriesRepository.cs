using DapperGenericRepository.Models;
using System;
using System.Collections.Generic;

namespace DapperGenericRepository.Contracts
{
    public interface IBikeStoresCategoriesRepository : IDisposable
    {
        IEnumerable<categories> GetAll(string where = null);

        void Insert(IEnumerable<categories> entities);

        void Insert(categories entity);

        void DeleteById(int Id);

        void Update(IEnumerable<categories> entities);

        void Update(categories entity);

        string GetIdColumn();

        IEnumerable<categories> GetCategories(string spName);

        IEnumerable<categories> GetCategoryById(string spName, int categoryId);
    }
}