using System;
using System.Collections.Generic;
using System.Collections;
using CaboodleES.Interface;
using CaboodleES.Utils;


namespace CaboodleES.Manager
{
    internal sealed class GroupManager
    {
        private Caboodle _world;
        private Dictionary<ulong, Group> groupCache;

        internal GroupManager(Caboodle world)
        {
            this._world = world;
        }

        internal Group GetGroup(params Component[] group)
        {
            ulong key = 0ul;
            for(int i = 0; i < group.Length; i++)
            {
                ulong a = _world.Entities.Components.GetFilter(group[i].GetType());
                key = key | a;
            }

            return groupCache[key];
        }
    }
}
