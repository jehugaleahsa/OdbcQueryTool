using System.ComponentModel;
using System.Data;

namespace OdbcQueryTool
{
    public class QueryModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isExecuting;
        public bool IsExecuting
        {
            get { return isExecuting; }
            set
            {
                isExecuting = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsExecuting"));
            }
        }

        private string commandText;
        public string CommandText
        {
            get { return commandText; }
            set
            {
                commandText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CommandText"));
            }
        }

        private DataView dataView;
        public DataView DataView
        {
            get { return dataView; }
            set
            {
                dataView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataView"));
            }
        }
    }
}
