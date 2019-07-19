using System;
using System.Collections.Generic;
using System.Linq;
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
using Wpf.FreeSqlTools.ViewModel;

namespace Wpf.FreeSqlTools.View
{
    /// <summary>
    /// SampleSuccessDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SampleSuccessDialog : UserControl
    {

   
        public SampleSuccessDialog(string msg)
        {
            InitializeComponent();    
            this.DataContext = new ViewMessageBoxModel(msg);
        }
    }

  
}
