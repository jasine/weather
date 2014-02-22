using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    public class City : INotifyPropertyChanged
    {
        private string name;
        private string code;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }
        }
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Code"));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
