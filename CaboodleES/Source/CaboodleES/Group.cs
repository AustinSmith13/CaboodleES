using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CaboodleES.Interface;

namespace CaboodleES
{
    public class Group
    {
        private IEnumerable<Component>[] _buffer;
        private Type[] _group;
        private Caboodle _world;

        public Group(Caboodle world, params Type[] c)
        {
            this._world = world;
            this._buffer = new IEnumerable<Component>[c.Length];
            this._group = c;
        }
        
        public IEnumerable<Component> this[int i]
        {
            get
            {
                return _buffer[i];
            }
        }

    }
}
