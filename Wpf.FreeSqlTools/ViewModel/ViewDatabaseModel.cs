using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

using MaterialDesignThemes.Wpf;
using Wpf.FreeSqlTools.Model;
using Wpf.FreeSqlTools.View;

namespace Wpf.FreeSqlTools.ViewModel
{
    public class ViewDatabaseModel : ViewBaseModel
    {
        AsyncObservableCollection<DatabaseModel> _databasesSource;
        public AsyncObservableCollection<DatabaseModel> DatabasesSource
        {
            get => _databasesSource;
            set => OnPropertyChanged(ref _databasesSource, value, RaisePropertyChanged());
        }

        /// <summary>
        ///  
        /// </summary>    
        public ICommand AddDataBaseDialogCommand => new DelegateCommand(async obj =>
        {
            var model = new DatabaseModel();
            var view = new DatabaseDialog
            {
                DataContext = model
            };

            //show the dialog
            var result = await DialogHost.Show(view, "RootDialog",
                delegate (object sender, DialogOpenedEventArgs args)
                {
                    Console.WriteLine("You could intercept the open and affect the dialog using eventArgs.Session.");
                }, 
                async delegate (object sender, DialogClosingEventArgs eventArgs)
                {
                    if ((bool)eventArgs.Parameter == false) return;
                    eventArgs.Cancel();
                    eventArgs.Session.UpdateContent(new SampleProgressDialog(null));
                    _ = await App.fsql.Insert(model).ExecuteInsertedAsync();
             
                InitDatabaseList();
                    await Task.CompletedTask
                        .ContinueWith((t, _) => eventArgs.Session.Close(false), null,
                            TaskScheduler.FromCurrentSynchronizationContext());

                });
        });


        /// <summary>
        ///  
        /// </summary>    
        public ICommand TestConnectionCommand => new DelegateCommand(async obj =>
        {
            if (obj is DatabaseModel model)
            {                
               var result = await DialogHost.Show(new SampleProgressDialog(model), 
                    delegate (object sender, DialogClosingEventArgs args)
                {
                    if ((bool)args.Parameter == false) return;
                });
            }   

        });
        /// <summary>
        /// Del
        /// </summary>    
        public ICommand DelConnectionCommand => new DelegateCommand(obj =>
        {

        });
        /// <summary>
        /// Edit
        /// </summary>    
        public ICommand EditConnectionCommand => new DelegateCommand(obj =>
        {



        });



        public ViewDatabaseModel()
        {
            DatabasesSource = new AsyncObservableCollection<DatabaseModel>();
            InitDatabaseList();
        }

        void InitDatabaseList()
        {
            DatabasesSource.Clear();
            var list = App.fsql.Select<DatabaseModel>().ToList();
            list.ForEach(a => DatabasesSource.Add(a));
        }
    }
}
