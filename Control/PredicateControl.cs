
namespace ReflectionWpf
{
    using System;
    using System.Linq;
    using System.Reflection;

 

    public class PredicateControl : ReflectionControl
    {



        public PredicateControl()
        {
            Type = typeof(ReflectionAttribute);

        }

        protected override Predicate<MemberInfo> GetDefaultPredicate()
        {
            return mi =>
                {
                    var attribute = mi.GetCustomAttributes(typeof(ReflectionAttribute), true).FirstOrDefault();
                    return (attribute != null) ? ((ReflectionAttribute)attribute).Include : false;
                };
        }



    }
}
