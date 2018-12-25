using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ReflectionWpf
{
    using System.Windows.Controls;

    public class ReflectionItemsControl:Control
    {

        public object Class
        {
            get { return (object)GetValue(ClassProperty); }
            set { SetValue(ClassProperty, value); }
        }


        // Using a DependencyProperty as the backing store for Class.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ClassProperty = DependencyProperty.Register("Class", typeof(object), typeof(ReflectionItemsControl), new PropertyMetadata(null,Changed));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
          
        }

        public ReflectionItemsControl()
        {
            Uri resourceLocater = new Uri("/ReflectionWpf;component/Themes/ReflectionAttributeControl.xaml",System.UriKind.Relative);
            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            Style = resourceDictionary["ReflectionItemsControlStyle"] as Style;


        }
    }
}
