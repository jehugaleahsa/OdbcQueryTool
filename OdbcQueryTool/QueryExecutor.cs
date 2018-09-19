using System;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OdbcQueryTool
{
    public class QueryExecutor
    {
        public static readonly DataTable EmptyDataTable = GetEmptyDataTable();

        private static DataTable GetEmptyDataTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("There are no results...");
            return table;
        }

        private readonly OdbcConnection connection;

        public QueryExecutor(OdbcConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public TimeSpan Timeout { get; set; }

        public async Task<DataTable> ExecuteQueryAsync(string commandText, CancellationToken token)
        {
            //await Task.Delay(TimeSpan.FromSeconds(5), token);
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(token);
            }
            using (var command = connection.CreateCommand())
            {
                command.CommandTimeout = (int)Timeout.TotalSeconds;
                command.CommandText = commandText;

                using (var reader = await command.ExecuteReaderAsync(token))
                {
                    DataTable table = new DataTable();
                    await Task.Run(() =>
                    {
                        table.Load(reader);
                        foreach (var dataColumn in table.Columns.Cast<DataColumn>())
                        {
                            dataColumn.ColumnName = $"{dataColumn.ColumnName} ({dataColumn.DataType.Name})";
                        }
                    }, token);
                    return table;
                }
            }
        }
    }
}
