using DapperGenericRepository.Contracts;
using DapperGenericRepository.Models;
using System.Collections.Generic;

namespace DapperGenericRepository.Repositories
{
    public class CategoriesRepository : BaseRepository<categories>, IBikeStoresCategoriesRepository
    {
        public string connectionString = @"Data Source=.\VULCAN;Initial Catalog=BikeStores;Persist Security Info=True;User ID=CITUser;Password=Vulcan$2010!;MultipleActiveResultSets=True;";
        private readonly string _tableSchema = "production";
        private readonly string _tableName = "categories";
        private readonly string _schemaTableName = "production.categories";
        private readonly string _idColumn = string.Empty;

        public CategoriesRepository()
        {
            _idColumn = GetPrimaryKeyName(_tableSchema, _tableName, connectionString);
        }

        public void DeleteById(int Id)
        {
            DeleteById(Id, _idColumn, _schemaTableName, connectionString);
        }

        public IEnumerable<categories> GetAll(string where = null)
        {
            return GetAll($"{_tableSchema}.{_tableName}", connectionString);
        }

        public void Insert(categories entity)
        {
            Insert(entity, _schemaTableName, connectionString, true);
        }

        public void Insert(IEnumerable<categories> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity, _schemaTableName, connectionString, true);
            }
        }

        public void Update(IEnumerable<categories> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity, _idColumn, _schemaTableName, connectionString);
            }
        }

        public void Update(categories entity)
        {
            Update(entity, _idColumn, _schemaTableName, connectionString);
        }

        public string GetIdColumn()
        {
            return GetPrimaryKeyName(_tableSchema, _tableName, connectionString);
        }


        public void Dispose(){}

        public IEnumerable<categories> GetCategories(string spName)
        {
            return ExecuteStoredProcedure(spName, connectionString);
        }

        public IEnumerable<categories> GetCategoryById(string spName, int categoryId)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("@CategoryId", categoryId);

            return ExecuteStoredProcedure(spName, connectionString, dictionary);
        }
    }
}