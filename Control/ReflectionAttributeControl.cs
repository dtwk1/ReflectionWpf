using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ReflectionWpf
{
    using System.CodeDom;
    using System.Collections;
    using System.ComponentModel;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Reflection;

    public class ReflectionControl : Control
    {


        //public IEnumerable Items
        //{
        //    get { return (IEnumerable)GetValue(ItemsProperty); }
        //    set { SetValue(ItemsProperty, value); }
        //}

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(ReflectionControl), new PropertyMetadata(null));


        public Reflection Reflection
        {
            get { return (Reflection)GetValue(ReflectionProperty); }
            set { SetValue(ReflectionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Reflection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReflectionProperty =
            DependencyProperty.Register("Reflection", typeof(object), typeof(ReflectionControl), new PropertyMetadata(null, ReflectionChanged));

        private static void ReflectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ReflectionControl).ReflectionChanges.OnNext(e.NewValue);
        }

        public object Type
        {
            get { return (object)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public object Predicate
        {
            get { return (object)GetValue(PredicateProperty); }
            set { SetValue(PredicateProperty, value); }
        }
        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(object), typeof(ReflectionControl), new PropertyMetadata(null, TypeChanged));

        // Using a DependencyProperty as the backing store for Predicate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PredicateProperty = DependencyProperty.Register("Predicate", typeof(object), typeof(ReflectionControl), new PropertyMetadata(null, PredicateChanged));


        private static void PredicateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ReflectionControl).PredicateChanges.OnNext(e.NewValue);
        }
        private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ReflectionControl).TypeChanges.OnNext(e.NewValue);
        }


        ISubject<object> TypeChanges = new Subject<object>();

        ISubject<object> PredicateChanges = new Subject<object>();

        ISubject<object> ReflectionChanges = new Subject<object>();

        static ReflectionControl()
        {


        }

        public ReflectionControl()
        {
            Uri resourceLocater = new Uri(
                "/ReflectionWpf;component/Themes/ReflectionAttributeControl.xaml",
                System.UriKind.Relative);
            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            Style = resourceDictionary["ReflectionControlStyle"] as Style;

            this.TypeChanges.CombineLatest(this.PredicateChanges.StartWith(GetDefaultPredicate()), this.ReflectionChanges, (a, b, c) => new { a, b, c })
                .Subscribe(
                    _ => this.Dispatcher.InvokeAsync(
                                () => this.SetValue(ItemsProperty, GetPredicate(this.GetMembers(_.a as Type,(Reflection)_.c), (System.Predicate<MemberInfo>)_.b)),
                                System.Windows.Threading.DispatcherPriority.Background, 
                                default(System.Threading.CancellationToken)));
        }

        protected virtual Predicate<MemberInfo> GetDefaultPredicate()
        {
            return mi => true;
        }
        

        private ReflectionViewModel[] GetPredicate(Tuple<Type, MemberInfo>[] getmembers, System.Predicate<MemberInfo> predicate)
        {
            return getmembers?
                .Where(__ => ((System.Predicate<MemberInfo>)predicate).Invoke(__.Item2))
                .Select(__ =>
                    {
                        var r = new ReflectionViewModel
                        {
                            Name = __.Item2.Name,
                            Info = __.Item2,
                            Value = (__.Item2 as PropertyInfo)?.GetValue(Activator.CreateInstance(__.Item1))
                        };
                        if (__.Item2 is MethodInfo)
                            r.ExecuteCommand = (ExecuteCommand)new MethodInfoToCommandConverter().Convert(new object[] { __.Item1, __.Item2, new Action<object>((a) => r.Value = a) },null,null,null);
                        return r;
                    }).ToArray();
        }



        private Tuple<Type, MemberInfo>[] GetMembers(Type type,Reflection _c)
        {

            Tuple<Type, MemberInfo>[] getmembers = null;

            if (!type.IsSubclassOf(typeof(Attribute)))
            {
                switch (_c)
                {
                    case Reflection.Property:
                        {
                            return type?.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(m => Tuple.Create(type, (MemberInfo)m)).ToArray();
                           
                        }
                    case Reflection.Method:
                        {
                            return type?.GetMethods().Where(m => m.DeclaringType != typeof(object)).Select(m => Tuple.Create(type, (MemberInfo)m)).ToArray();
                        }
                    default:
                        return null;
                }
            }
            else
            {
                switch (_c)
                {
                    case Reflection.Property:
                        {
                            return AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(__ => ReflectionHelper.GetPropertiesByAttribute(__, type)).ToArray();
                          
                        }
                    case Reflection.Method:
                        {
                            return AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(__ => ReflectionHelper.GetMethodsByAttribute(__, type))
                                .ToArray();
                        }
                    default:
                        return null;
                }
            }

        }
    }
}
