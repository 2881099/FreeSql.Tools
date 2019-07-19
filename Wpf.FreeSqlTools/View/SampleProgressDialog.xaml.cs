using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf.FreeSqlTools.Model;

namespace Wpf.FreeSqlTools.View
{
    /// <summary>
    /// SampleProgressDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SampleProgressDialog : UserControl
    {
        DatabaseModel databaseModel;
        public SampleProgressDialog(DatabaseModel database)
        {
            InitializeComponent();
            if (database != null)
            {
                databaseModel = database;
                Loaded += SampleProgressDialog_Loaded;
            }
        }
        private void SampleProgressDialog_Loaded(object sender, RoutedEventArgs e)
        {          

             Task.Run(async () => {

                 var fag = databaseModel.GetConnection(out string messages);
                 DialogHost.CloseDialogCommand.CanExecute(false, null);
                 if (fag)
                {
                    await DialogHost.Show(new SampleSuccessDialog(messages),
                         delegate (object s, DialogClosingEventArgs args)
                         {
                             if ((bool)args.Parameter == false) return;
                         });
                }
                else
                {
                    await DialogHost.Show(new SampleErrorDialog(messages),
                                delegate (object s, DialogClosingEventArgs args)
                                {
                                    if ((bool)args.Parameter == false) return;
                                });
                }
            });

        }
    }
}
