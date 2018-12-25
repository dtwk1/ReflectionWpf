using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionWpf
{
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;

    public class ReflectionDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MethodTemplate { get; set; }
        public DataTemplate PropertyTemplate { get; set; }
        public DataTemplate FieldTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {

            if (((dynamic)item).Info is MethodInfo)
            {
                return MethodTemplate;
            }
            else if (((dynamic)item).Info is PropertyInfo)
            {
                return PropertyTemplate;
            }
            else if (((dynamic)item).Info is FieldInfo)
            {
                return this.FieldTemplate;
            }

            return null;
        }
    }
}
