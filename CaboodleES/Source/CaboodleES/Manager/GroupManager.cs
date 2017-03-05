using System;
using System.Collections.Generic;
using CaboodleES.Utils;


namespace CaboodleES.Manager
{
    internal sealed class GroupManager
    {
        private Caboodle caboodle;

        internal GroupManager(Caboodle caboodle)
        {
            this.caboodle = caboodle;
        }

        internal void Add(Type[] types)
        {

        }

        internal IList<Entity> Get(int i)
        {
            return null;
        }

        internal void Process(Entity ent)
        {
            var comps = ent.GetComponents();
            //for(int i = 0; i < masks.Count; i++)
          //  {

          //  }
        }
    }
}
