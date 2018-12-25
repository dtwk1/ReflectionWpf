


namespace ReflectionWpf
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Markup;


    public class MethodInfoToCommandConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(values.Length>1)
            if (values[0] is Type && values[1] is MethodInfo)
            {
                MethodInfo data = values[1] as MethodInfo;
                Type type = values[0] as Type;
                var instance = Activator.CreateInstance(type);
                if (values.Length == 2)
                    return new ExecuteCommand(() => data.Invoke(instance, new object[] { }));
                else if (values.Length == 3)
                    return new ExecuteCommand(() => (values[2] as Action<object>).Invoke(data.Invoke(instance, new object[] { })));
                else
                {
                    return null;
                }
            }
   
                return null;
            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class ExecuteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        private Action f = null;
        public ExecuteCommand(Action f)
        {
            this.f = f;
        }


        public void Execute(object parameter)
        {
            f.Invoke();
        }
    }


}
