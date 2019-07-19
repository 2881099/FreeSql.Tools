using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.FreeSqlTools.ViewModel
{
    public class ViewMessageBoxModel:ViewBaseModel
    {
        private string _message;
        /// <summary>
        ///  
        /// </summary>
        public string Message
        {
            get => _message;
            set => OnPropertyChanged(ref _message, value, RaisePropertyChanged());
        }

        public ViewMessageBoxModel(string msg)
        {
            this.Message = msg;
        }
    }
}
