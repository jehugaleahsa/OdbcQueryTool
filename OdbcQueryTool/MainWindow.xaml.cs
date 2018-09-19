using System;
using System.Data;
using System.Data.Odbc;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OdbcQueryTool
{
    public partial class MainWindow : Window
    {
        private readonly QueryExecutor executor;
        private CancellationTokenSource tokenSource;

        public MainWindow()
        {
            var settings = new SettingsHelper();
            var connection = new OdbcConnection(settings.ConnectionString);
            executor = new QueryExecutor(connection) { Timeout = settings.Timeout };
            Model = new QueryModel() { DataView = QueryExecutor.EmptyDataTable.AsDataView() };
            InitializeComponent();
        }

        public QueryModel Model { get; }

        private async void ExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            if (Model.IsExecuting)
            {                
                tokenSource?.Cancel();
                return;
            }
            try
            {
                Model.IsExecuting = true;
                tokenSource?.Dispose();
                tokenSource = new CancellationTokenSource();
                var dataTable = await ExecuteQuery(Model.CommandText, tokenSource.Token);
                Model.DataView = dataTable.AsDataView();
            }
            finally
            {
                Model.IsExecuting = false;
            }
        }

        private async Task<DataTable> ExecuteQuery(string commandText, CancellationToken token)
        {
            if (String.IsNullOrWhiteSpace(commandText))
            {
                return QueryExecutor.EmptyDataTable;
            }
            try
            {
                var dataTable = await executor.ExecuteQueryAsync(Model.CommandText, token);
                return dataTable;
            }
            catch (OperationCanceledException)
            {
                return QueryExecutor.EmptyDataTable;
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return QueryExecutor.EmptyDataTable;
            }
        }
    }
}
