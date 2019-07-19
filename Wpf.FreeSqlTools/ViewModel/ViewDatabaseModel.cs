using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
 
using MaterialDesignThemes.Wpf;
using Wpf.FreeSqlTools.Model;
using Wpf.FreeSqlTools.View;

namespace Wpf.FreeSqlTools.ViewModel
{
    public class ViewDatabaseModel:ViewBaseModel
    {
        AsyncObservableCollection<DatabaseModel> _databasesSource;
        public AsyncObservableCollection<DatabaseModel> DatabasesSource {
            get => _databasesSource;
            set => OnPropertyChanged(ref _databasesSource, value, RaisePropertyChanged());
         }



        /// <summary>
        ///  
        /// </summary>    
        public ICommand AddDataBaseDialogCommand => new DelegateCommand(async obj =>
        {       
            var view = new DatabaseDialog
            {
                DataContext = new DatabaseModel() { Name="xxxx"}
            };
            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog", ExtendedOpenedEventHandler, ExtendedClosingEventHandler);
        });

        private void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
        }

        private void ExtendedClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((bool)eventArgs.Parameter == false) return;
            eventArgs.Cancel();
            eventArgs.Session.UpdateContent(new SampleProgressDialog());   
            Task.Delay(TimeSpan.FromSeconds(3))
                .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                    TaskScheduler.FromCurrentSynchronizationContext());
        }



        public ViewDatabaseModel()
        {
            DatabasesSource = new AsyncObservableCollection<DatabaseModel>();
            DatabasesSource.Add(new DatabaseModel {
                Id =1, DataType = FreeSql.DataType.MySql, Name ="测试"
            });
        }
    }
}
