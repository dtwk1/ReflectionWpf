using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionWpf
{
    public class MethodInfoControl : ReflectionControl
    {

        public MethodInfoControl()
        {
            this.Reflection = Reflection.Method;
        }

    }
}
