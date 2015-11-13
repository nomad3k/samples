using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CheeseShop.Common
{
    public static class DbUtil
    {
        private static IDictionary<string, object> ConvertToDictionary(object parameters)
        {
            if (parameters == null)
                return new Dictionary<string, object>();

            return parameters.GetType()
                             .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                             .ToDictionary(x => x.Name,
                                           x => x.GetValue(parameters) ?? DBNull.Value);
        }

        private static int Execute(IDbConnection connection,
                                   IDbTransaction transaction,
                                   string sql,
                                   object parameters = null)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = sql;
                foreach (var p in ConvertToDictionary(parameters))
                {
                    var param = command.CreateParameter();
                    param.ParameterName = p.Key;
                    param.Value = p.Value;
                    command.Parameters.Add(param);
                }
                return command.ExecuteNonQuery();
            }
        }

        public static int Execute(this IDbTransaction transaction,
                                  string sql,
                                  object parameters = null)
        {
            return Execute(transaction.Connection, transaction, sql, parameters);
        }

        public static int Execute(this IDbConnection connection,
                                  string sql,
                                  object parameters = null)
        {
            return Execute(connection, null, sql, parameters);
        }

        private static IEnumerable<object[]> Query(IDbConnection connection,
                                                   IDbTransaction transaction,
                                                   string sql,
                                                   object parameters = null)
        {
            using (var command = connection.CreateCommand())
            {
                command.Transaction = transaction;
                command.CommandText = sql;
                foreach (var p in ConvertToDictionary(parameters))
                {
                    var param = command.CreateParameter();
                    param.ParameterName = p.Key;
                    param.Value = p.Value;
                    command.Parameters.Add(param);
                }

                using (var reader = command.ExecuteReader())
                {
                    var values = new object[reader.FieldCount];
                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        yield return values;
                    }
                }
            }
        }

        public static IEnumerable<object[]> Query(this IDbTransaction transaction,
                                                  string sql,
                                                  object parameters = null)
        {
            return Query(transaction.Connection, transaction, sql, parameters);
        }

        public static IEnumerable<object[]> Query(this IDbConnection connection,
                                                  string sql,
                                                  object parameters = null)
        {
            return Query(connection, null, sql, parameters);
        }
    }
}
