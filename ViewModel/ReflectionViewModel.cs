using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionWpf
{
    using inpce.core.Library.Extensions;
    using System.CodeDom;
    using System.ComponentModel;

    public class ReflectionViewModel:INotifyPropertyChanged
    {
        private object _value;

        public ExecuteCommand ExecuteCommand { get; set; }

        public MemberInfo Info { get; internal set; }

        public string Name { get; set; }

        public object Value
        {
            get{return _value;}
            set{this.SetProperty(ref _value, value, PropertyChanged);}
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
