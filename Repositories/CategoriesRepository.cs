using DapperGenericRepository.Contracts;
using DapperGenericRepository.Models;
using System.Collections.Generic;

namespace DapperGenericRepository.Repositories
{
    public class CategoriesRepository : BaseRepository<categories>, ICategoriesRepository
    {
        public string connectionString = @"Data Source=.\;Initial Catalog=BikeStores;Persist Security Info=True;User ID=sa;Password=4everSQL;MultipleActiveResultSets=True;";
        private readonly string _tableSchema = "production";
        private readonly string _tableName = "categories";
        private readonly string _schemaTableName = "production.categories";
        private readonly string _idColumn = string.Empty;

        public CategoriesRepository()
        {
            _idColumn = GetPrimaryKeyName(_tableSchema, _tableName, connectionString);
        }

        public void DeleteCategoryById(int Id)
        {
            DeleteById(Id, _idColumn, _schemaTableName, connectionString);
        }

        public IEnumerable<categories> GetAllCategories(string where = null)
        {
            return GetAll($"{_tableSchema}.{_tableName}", connectionString);
        }

        public void InsertCategory(categories entity)
        {
            Insert(entity, _schemaTableName, connectionString, true);
        }

        public void InsertCategories(IEnumerable<categories> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity, _schemaTableName, connectionString, true);
            }
        }

        public void UpdateCategories(IEnumerable<categories> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity, _idColumn, _schemaTableName, connectionString);
            }
        }

        public void UpdateCategory(categories entity)
        {
            Update(entity, _idColumn, _schemaTableName, connectionString);
        }

        public string GetIdColumn()
        {
            return GetPrimaryKeyName(_tableSchema, _tableName, connectionString);
        }


        public void Dispose() { }

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