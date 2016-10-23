using System.Collections.Generic;
using System;
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

        private class CompTracker
        {
            public uint liveId;
            public readonly T component;
            public readonly ulong entity;

            public CompTracker(ulong entity, T component, uint liveId)
            {
                this.entity = entity;
                this.component = component;
                this.liveId = liveId;
            }
        }

        private Dictionary<ulong, CompTracker> componentTrackerCache;

        private T[] components;
        private CompTracker[] componentTrackers;

        //private Bag<T> components;
        private Caboodle caboodle;
        private ulong _filter;
        private uint topIndice = 0;

        internal IEnumerable<T> Components
        {
            get { return components; }
        }

        public ComponentCollection(Caboodle world, ulong filter)
        {
            //components = new Bag<T>();

            components = new T[128];
            componentTrackers = new CompTracker[128];
            componentTrackerCache = new Dictionary<ulong, CompTracker>();
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
            var newTracker = new CompTracker(eid, c, topIndice);

            if(topIndice >= components.Length - 1)
            {
                var oldComps = this.components;
                var oldTrackers = this.componentTrackers;

                components = new T[components.Length * 2];
                componentTrackers = new CompTracker[components.Length * 2];

                Array.Copy(oldComps, 0, this.components, 0, oldComps.Length);
                Array.Copy(oldTrackers, 0, this.componentTrackers, 0, oldTrackers.Length);
            }

            c.SetEntityContext(eid);
            componentTrackers[topIndice] = newTracker;
            components[topIndice++] = c;
            componentTrackerCache.Add(eid, newTracker);


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
                comp = componentTrackerCache[eid].component;
            }
            catch (System.Exception e) { throw new NoSuchComponentException(e.Message, typeof(T).ToString()); }
           
            return comp;
        }

        public bool Has(ulong eid)
        {
            CompTracker comp = null;
            return componentTrackerCache.TryGetValue(eid, out comp);
        }

        /// <summary>
        /// Removes a component associated with the Entity Id.
        /// </summary>
        public Component Remove(ulong eid)
        {
            if (!Has(eid)) return null;

            var trackerRemoved = componentTrackerCache[eid];
            var trackerTop = componentTrackers[topIndice - 1];
            T removedComp = components[trackerRemoved.liveId];
            componentTrackerCache.Remove(eid);

            // swap top element with removed element then make the top null
            components[trackerRemoved.liveId] = components[topIndice - 1];
            components[topIndice - 1] = null;

            // update the tracker info and order
            trackerTop.liveId = trackerRemoved.liveId;
            componentTrackers[trackerRemoved.liveId] = trackerTop;
            componentTrackers[topIndice - 1] = null;
            topIndice--;

            return caboodle.Pool.ReleaseComponent(removedComp);

            /*
            for(int i = 0; i < components.Count; i++)
            {
                if(components[i].Id == eid)
                {
                    return caboodle.Pool.ReleaseComponent(components.Remove(i));
                }
            }*/
        }

        /// <summary>
        /// Removes all components from the collection.
        /// </summary>
        public void Clear()
        {
            componentTrackerCache.Clear();
            components = new T[128];
            //components.Clear();
        }

        /// <summary>
        /// Component Type manager manages. [DEPRECATED]
        /// </summary>
        /// <returns>The type of component the manager is interested in</returns>
        [Obsolete("This method is deprecated, Please use its type instead.")]
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
