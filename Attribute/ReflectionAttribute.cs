using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionWpf
{
    using Attribute = System.Attribute;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ReflectionAttribute : Attribute
    {
        public string X;

        public bool Include;
        public ReflectionAttribute(string x, bool include = true)
        {
            X = x;
            Include = include;
        }
    }
}
