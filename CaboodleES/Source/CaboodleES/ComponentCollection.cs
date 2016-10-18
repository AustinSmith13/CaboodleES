using System.Collections.Generic;
using CaboodleES.Utils;
using CaboodleES.Interface;


namespace CaboodleES.Manager
{
    /// <summary>
    /// Keeps track of a list of components of type T.
    /// </summary>
    internal sealed class ComponentCollection<T> : IComponentCollection
        where T : Component, new()
    {
        public ulong Filter
        {
            get { return _filter; }
        }

        private Dictionary<ulong, T> componentCache;
        private Bag<T> components;
        private Caboodle caboodle;
        private ulong _filter;

        internal IEnumerable<T> Components
        {
            get { return components; }
        }

        public ComponentCollection(Caboodle world, ulong filter)
        {
            components = new Bag<T>();
            componentCache = new Dictionary<ulong, T>();
            this.caboodle = world;
            this._filter = filter;
        }

        /// <summary>
        /// Adds a component to an entity. If the component already exists a new one will be created.
        /// </summary>
        public Component Add(ulong eid)
        {
            if (this.Has(eid)) return this.Get(eid);
            var c = caboodle.Pool.CreateComponent<T>();

            c.SetEntityContext(eid);
            components.Add(c);
            componentCache.Add(eid, c);

            return c;
        }

        /// <summary>
        /// Retrives a component associated with the given Entity Id.
        /// </summary>
        public Component Get(ulong eid)
        {
            T comp = null;
            try
            {
                comp = componentCache[eid];
            }
            catch (System.Exception e) { throw new NoSuchComponentException(e.Message, typeof(T).ToString()); }
           
            return comp;
        }

        public bool Has(ulong eid)
        {
            T comp = null;
            return componentCache.TryGetValue(eid, out comp);
        }

        /// <summary>
        /// Removes a component associated with the Entity Id.
        /// </summary>
        public Component Remove(ulong eid)
        {
            //if (!componentCache.ContainsKey(eid)) return null;
            componentCache.Remove(eid);

            for(int i = 0; i < components.Count; i++)
            {
                if(components[i].Id == eid)
                {
                    return caboodle.Pool.ReleaseComponent(components.Remove(i));
                }
            }

            return null;
        }

        /// <summary>
        /// Removes all components from the collection.
        /// </summary>
        public void Clear()
        {
            componentCache.Clear();
            components.Clear();
        }

        /// <summary>
        /// Component Type manager manages.
        /// </summary>
        /// <returns>The type of component the manager is interested in</returns>
        public System.Type GetCType()
        {
            return typeof(T);
        }

        public ulong GetFilter()
        {
            return Filter;
        }
    }
}
