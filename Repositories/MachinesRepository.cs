using DapperGenericRepository.Contracts;
using DapperGenericRepository.Models;
using System.Collections.Generic;

namespace DapperGenericRepository.Repositories
{
    public class MachinesRepository : BaseRepository<Machines>, IMachinesRepository
    {
        public string connectionString = @"Data Source=.\VULCAN;Initial Catalog=VULCAN;Persist Security Info=True;User ID=CITUser;Password=Vulcan$2010!;MultipleActiveResultSets=True;";
        private readonly string _tableSchema = "production";
        private readonly string _tableName = "categories";
        private readonly string _schemaTableName = "production.categories";
        private readonly string _idColumn = string.Empty;

        public MachinesRepository()
        {
            _idColumn = GetPrimaryKeyName(_tableSchema, _tableName, connectionString);
        }

        //public void DeleteById(int Id)
        //{
        //    DeleteById(Id, _idColumn, _schemaTableName, connectionString);
        //}

        //public IEnumerable<categories> GetAll(string where = null)
        //{
        //    return GetAll($"{_tableSchema}.{_tableName}", connectionString);
        //}

        //public void Insert(categories entity)
        //{
        //    Insert(entity, _schemaTableName, connectionString, true);
        //}

        //public void Insert(IEnumerable<categories> entities)
        //{
        //    foreach (var entity in entities)
        //    {
        //        Insert(entity, _schemaTableName, connectionString, true);
        //    }
        //}

        //public void Update(IEnumerable<categories> entities)
        //{
        //    foreach (var entity in entities)
        //    {
        //        Update(entity, _idColumn, _schemaTableName, connectionString);
        //    }
        //}

        //public void Update(categories entity)
        //{
        //    Update(entity, _idColumn, _schemaTableName, connectionString);
        //}

        //public string GetIdColumn()
        //{
        //    return GetPrimaryKeyName(_tableSchema, _tableName, connectionString);
        //}


        public void Dispose() { }

        public string ExecuteStoredProcedure(string spName)
        {
            //var parameters = new DynamicParameters();
            //parameters.Add("@TableName", dbType: DbType.Int32, direction: ParameterDirection.Input);
            var result = ExecuteStoredProcedure(spName, connectionString);
            return "";
        }
    }

}
