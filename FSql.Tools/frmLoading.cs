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
    public partial class FrmLoading : DevComponents.DotNetBar.Office2007Form
    {
        public FrmLoading(string txt = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(txt)) labelX1.Text = txt;
            this.Load += FrmLoading_Load;
        }

        private void FrmLoading_Load(object sender, EventArgs e)
        {
            circularProgress1.Show();
            circularProgress1.IsRunning = true;
        }
    }
}
