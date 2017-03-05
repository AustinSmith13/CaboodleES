using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaboodleES.Attributes
{
    public class AddHandleAttribute : Attribute
    {
        public global::System.Type[] Handle {  get{ return handleArgumentTypes; }  }

        private global::System.Type[] handleArgumentTypes;

        public AddHandleAttribute() { }

        public AddHandleAttribute(Type t1)
        {
            handleArgumentTypes = new global::System.Type[1] { t1 };
        }

        public AddHandleAttribute(Type t1, Type t2)
        {
            handleArgumentTypes = new global::System.Type[2] { t1, t2 };
        }

        public AddHandleAttribute(Type t1, Type t2, Type t3)
        {
            handleArgumentTypes = new global::System.Type[3] { t1, t2, t3 };
        }

        public AddHandleAttribute(Type t1, Type t2, Type t3, Type t4)
        {
            handleArgumentTypes = new global::System.Type[4] { t1, t2, t3, t4 };
        }
    }
}
