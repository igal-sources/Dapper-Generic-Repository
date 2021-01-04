using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DapperGenericRepository
{
    public abstract class BaseRepository<T>
    {
        public virtual IEnumerable<T> GetAll(string tableName, string dataSource, string where = null)
        {
            using (var connection = GetOpenConnection(dataSource))
            {
                string query = $"SELECT * FROM {tableName}";

                if (!string.IsNullOrWhiteSpace(where))
                {
                    query += where;
                }

                return connection.Query<T>(query);
            }
        }

        public virtual void Insert(T entity, string tableName, string dataSource, bool isIdentity = false)
        {
            var insertQuery = GenerateInsertQuery(tableName, isIdentity);

            using (var connection = GetOpenConnection(dataSource))
            {
                connection.Execute(insertQuery, entity);
            }
        }

        public virtual void DeleteByGuid(T entity, Guid Id, string IdColumn, string tableName, string dataSource)
        {
            using (var connection = GetOpenConnection(dataSource))
            {
                connection.Execute($"DELETE FROM {tableName} WHERE {IdColumn}=@{IdColumn}", new { IdColumn = Id });
            }
        }

        public virtual void DeleteById(int Id, string IdColumn, string tableName, string dataSource)
        {
            using (var connection = GetOpenConnection(dataSource))
            {
                connection.Execute($"DELETE FROM {tableName} WHERE {IdColumn}={Id}");
            }
        }

        public virtual void Update(T entity, string IdColumn, string tableName, string dataSource)
        {
            var updateQuery = GenerateUpdateQuery(tableName, IdColumn);

            using (var connection = GetOpenConnection(dataSource))
            {
                connection.Execute(updateQuery, entity);
            }
        }

        public static Tuple<bool, string> ExecuteQuery(string sql, string dataSource, object param = null, CommandType? commandType = null)
        {
            try
            {
                using (var connection = GetOpenConnection(dataSource))
                {
                    connection.Execute(sql, param, commandType: commandType);
                    return Tuple.Create(true, "Done Successfully");
                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
        }

        public static IEnumerable<T> ExecuteStoredProcedure(string sql, string dataSource, Dictionary<string, object> parameters = null)
        {
            using (var connection = GetOpenConnection(dataSource))
            {
                return connection.Query<T>(sql, param: new DynamicParameters(parameters), commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public virtual string GetPrimaryKeyName(string tableSchema, string tableName, string dataSource)
        {
            var query = $@"SELECT col.COLUMN_NAME FROM information_schema.table_constraints tc
                            INNER JOIN information_schema.key_column_usage col
                                ON col.Constraint_Name = tc.Constraint_Name
                            AND col.Constraint_schema = tc.Constraint_schema
                        WHERE tc.Constraint_Type = 'PRIMARY KEY' AND tc.TABLE_SCHEMA = '{tableSchema}' AND col.Table_name = '{tableName}'";

            using (var connection = GetOpenConnection(dataSource))
            {
                return (string)connection.ExecuteScalar(query);
            }
        }

        #region Generate Queries

        public static string GenerateInsertQuery(string tableName, bool isIdentity = false)
        {
            var insertQuery = new StringBuilder();
            string insertWithIdentity = $"SET IDENTITY_INSERT {tableName} ON INSERT INTO {tableName} ";
            string insertWithoutIdentity = $"INSERT INTO {tableName} ";

            if (isIdentity)
            {
                insertQuery.Append(insertWithIdentity);
            }
            else
            {
                insertQuery.Append(insertWithoutIdentity);
            }

            insertQuery.Append("(");

            var properties = GenerateListOfProperties(GetProperties<T>());
            properties.ForEach(prop => { insertQuery.Append($"[{prop}],"); });

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            properties.ForEach(prop => { insertQuery.Append($"@{prop},"); });

            insertQuery.Remove(insertQuery.Length - 1, 1).Append(")");

            if (isIdentity)
            {
                insertQuery.Append($"SET IDENTITY_INSERT {tableName} OFF; ");
            }

            return insertQuery.ToString();
        }

        public static string GenerateMergeIntoQuery(string tableName, string idColumn, bool isIdentity = false)
        {
            var mergeIntoQuery = new StringBuilder();
            string mergeInto = $"MERGE INTO {tableName} AS TARGET USING (VALUES(";
            string mergeIntoWithIdentity = $"SET IDENTITY_INSERT {tableName} ON MERGE INTO {tableName} AS TARGET USING (VALUES(";

            if (isIdentity)
            {
                mergeIntoQuery.Append(mergeIntoWithIdentity);
            }
            else
            {
                mergeIntoQuery.Append(mergeInto);
            }

            var properties = GenerateListOfProperties(GetProperties<T>());
            properties.ForEach(prop => { mergeIntoQuery.Append($"@{prop},"); });
            mergeIntoQuery.Remove(mergeIntoQuery.Length - 1, 1); //remove last comma

            mergeIntoQuery.Append(")) AS SOURCE (");

            properties.ForEach(prop => { mergeIntoQuery.Append($"{prop},"); });

            mergeIntoQuery.Remove(mergeIntoQuery.Length - 1, 1); //remove last comma

            mergeIntoQuery.Append(") ");
            mergeIntoQuery.Append($"ON SOURCE.{idColumn} = TARGET.{idColumn} ");
            mergeIntoQuery.Append("WHEN MATCHED THEN ");
            mergeIntoQuery.Append("UPDATE SET ");

            properties.ForEach(property =>
            {
                if (!property.Equals(idColumn))
                {
                    mergeIntoQuery.Append($"{property} = SOURCE.{property},");
                }
            });

            mergeIntoQuery.Remove(mergeIntoQuery.Length - 1, 1); //remove last comma

            mergeIntoQuery.Append(" WHEN NOT MATCHED THEN ");

            mergeIntoQuery.Append("INSERT (");
            properties.ForEach(prop => { mergeIntoQuery.Append($"{prop},"); });
            mergeIntoQuery.Remove(mergeIntoQuery.Length - 1, 1); //remove last comma

            mergeIntoQuery.Append(") VALUES (");
            properties.ForEach(prop => { mergeIntoQuery.Append($"SOURCE.{prop},"); });
            mergeIntoQuery.Remove(mergeIntoQuery.Length - 1, 1); //remove last comma
            mergeIntoQuery.Append("); ");

            if (isIdentity)
            {
                mergeIntoQuery.Append($"SET IDENTITY_INSERT {tableName} OFF; ");
            }

            return mergeIntoQuery.ToString();
        }

        private static string GenerateUpdateQuery(string tableName, string IdColumn)
        {
            var updateQuery = new StringBuilder($"UPDATE {tableName} SET ");
            var properties = GenerateListOfProperties(GetProperties<T>());

            properties.ForEach(property =>
            {
                if (!property.Equals(IdColumn))
                {
                    updateQuery.Append($"{property}=@{property},");
                }
            });

            updateQuery.Remove(updateQuery.Length - 1, 1); //remove last comma
            updateQuery.Append($" WHERE {IdColumn}=@{IdColumn}");

            return updateQuery.ToString();
        }

        private static List<string> GenerateListOfProperties(IEnumerable<PropertyInfo> listOfProperties)
        {
            return (from prop in listOfProperties
                    let attributes = prop.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    where attributes.Length <= 0 || (attributes[0] as DescriptionAttribute)?.Description != "ignore"
                    select prop.Name).ToList();
        }

        private static IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return typeof(T).GetProperties();
        }

        #endregion Generate Queries

        #region Connections

        public static SqlConnection GetOpenConnection(string dataSource, bool mars = false)
        {
            if (mars)
            {
                SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(dataSource)
                {
                    MultipleActiveResultSets = true
                };

                dataSource = scsb.ConnectionString;
            }

            var connection = new SqlConnection(dataSource);
            connection.Open();
            return connection;
        }

        public static SqlConnection GetClosedConnection(string dataSource)
        {
            return new SqlConnection(dataSource);
        }

        #endregion Connections
    }
}