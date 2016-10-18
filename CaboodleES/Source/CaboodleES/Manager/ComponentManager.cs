using System;
using System.Collections.Generic;
using CaboodleES.Interface;

namespace CaboodleES.Manager
{
    public sealed class ComponentManager
    {
        public event Action<ulong, Component> OnComponentRemoved;
        public event Action<ulong, Component> OnComponentAdded;

        private List<IComponentCollection> componentCollections;
        private Dictionary<System.Type, IComponentCollection> componentCollectionCache;
        private Caboodle world;
        private byte cfilter;


        public ComponentManager(Caboodle world)
        {
            componentCollections = new List<IComponentCollection>();
            componentCollectionCache = new Dictionary<System.Type, IComponentCollection>();
            this.world = world;
        }

        /// <summary>
        /// Adds/Creates a component associated with the given entity.
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="eid"></param>
        /// <returns></returns>
        public C AddComponent<C>(ulong eid)
            where C : Component, new()
        {
            IComponentCollection collection = null;
            componentCollectionCache.TryGetValue(typeof(C), out collection);

            if (collection == null)
            {
                // Assigns the component collection a filter identifier
                if (cfilter > 64) throw new System.Exception("Component type cap reached.");
                ulong filter = (1ul << cfilter++);
                collection = new ComponentCollection<C>(world, filter);
                componentCollections.Add(collection);
                componentCollectionCache.Add(typeof(C), collection);
            }

            var c = collection.Add(eid) as C;
            OnComponentAdded?.Invoke(eid, c);
            
            return c;
        }

        /// <summary>
        /// Gets the component associated with
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="eid"></param>
        /// <returns></returns>
        public C GetComponent<C>(ulong eid)
            where C : Component
        {
            return GetCollection(typeof(C)).Get(eid) as C;
        }

        /// <summary>
        /// Removes the component
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="eid"></param>
        /// <returns></returns>
        public C RemoveComponent<C>(ulong eid)
            where C : Component
        {
            var c = GetCollection(typeof(C)).Remove(eid) as C;
            OnComponentRemoved?.Invoke(eid, c);

            return c;
        }

        /// <summary>
        /// Removes all of the entity id's components.
        /// </summary>
        /// <param name="eid"></param>
        public void RemoveComponents(ulong eid)
        {
            for (int i = 0; i < componentCollections.Count; i++)
            {
                componentCollections[i].Remove(eid);
            }
        }

        /// <summary>
        /// Checks if the entity id has the component C.
        /// </summary>
        public bool HasComponent<C>(ulong eid)
            where C : Component
        {
            IComponentCollection collection = null;
            componentCollectionCache.TryGetValue(typeof(C), out collection);

            if (collection == null)
                return false;

            return collection.Has(eid);
        }

        public ulong GetFilter(System.Type c)
        {
            return GetCollection(c).GetFilter();
        }

        /// <summary>
        /// Clears all components in the caboodle. Remvoes all components from each and every entity.
        /// </summary>
        public void Clear()
        {
            foreach (var manager in componentCollections)
            {
                manager.Clear();
            }
            componentCollectionCache.Clear();
            componentCollections.Clear();
            cfilter = 0x00;
        }

        private IComponentCollection GetCollection(System.Type ctype)
        {
            IComponentCollection cm = null;
            componentCollectionCache.TryGetValue(ctype, out cm);
            if(cm == null)
                throw new NoSuchComponentException("No such component, " + ctype.Name, ctype.Name);

            return cm;
        }
    }
}
