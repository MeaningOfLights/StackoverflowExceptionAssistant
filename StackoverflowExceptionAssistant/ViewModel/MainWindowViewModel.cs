using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using EnvDTE;

namespace JeremyThompsonLabs.StackoverflowExceptionAssistant
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SOPage> _SOPageCollection;

        public MainWindowViewModel()
        {
            _SOPageCollection = new ObservableCollection<SOPage>();
        }

        public MainWindowViewModel(DTE dte, LastException lastException) : this()
        {                
            BindData(SearchHelper.SearchQueryBasedOnExceptionDescription(dte, lastException));
        }

        public ObservableCollection<SOPage> SOPageCollection
        {
            get { return _SOPageCollection; }
            set
            {
                _SOPageCollection = value;
                RaisePropertyChanged("SOPageCollection");
            }
        }
        
        private void BindData(List<SOPage> SOPageList)
        {
            foreach(var stackoverflowPage in SOPageList) SOPageCollection.Add(stackoverflowPage);
            RaisePropertyChanged("SOPageCollection");
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
