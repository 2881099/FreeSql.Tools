using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeSqlTools
{
    public partial class FrWebMain : DSkin.Forms.MiniBlinkForm
    {
        public FrWebMain(): base("res://FreeSqlTools/Views/mainframe.html")
        {
           
            InitializeComponent();
            Width = (int) (1366 * ZoomFactor);
            Height = (int) (700 * ZoomFactor);

            InnerDuiControl.Borders.AllWidth = 5;

       
        }
}
}
