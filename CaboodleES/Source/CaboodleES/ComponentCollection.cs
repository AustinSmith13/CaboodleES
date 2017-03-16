using global::System.Collections.Generic;
using global::System;
using CaboodleES.Interface;
using CaboodleES.Utils;


namespace CaboodleES.Manager
{
    /// <summary>
    /// Keeps track of a list of components of type T.
    /// </summary>
    public sealed class ComponentCollection<T> : IComponentCollection
        where T : Component, new()
    {
        public int Id
        {
            get { return id; }
        }

      
        private Table<T> components;
       
        //private Bag<T> components;
        private Caboodle caboodle;
        private int id;

        public IEnumerable<T> Components
        {
            get { return null; }
        }

        public ComponentCollection(Caboodle world, int id)
        {
            this.caboodle = world;
            this.id = id;
            this.components = new Table<T>();
        }

        public int GetId()
        {
            return id;
        }

        /// <summary>
        /// Adds a component to an entity. If the component already exists a new one will be created.
        /// </summary>
        public Component Add(int eid)
        {
            if (this.Has(eid)) return this.Get(eid);
            var c = caboodle.Pool.CreateComponent<T>();

            components.Set(eid, c);
            
            return c;
        }

        /// <summary>
        /// Retrives a component associated with the given Entity Id.
        /// </summary>
        public Component Get(int eid)
        {
            return components.Get(eid);
        }

        public bool Has(int eid)
        {
            return components.Get(eid) != null;
        }

        /// <summary>
        /// Removes a component associated with the Entity Id.
        /// </summary>
        public Component Remove(int eid)
        {
            var removedComp = components.Get(eid);
            components.Remove(eid);
            if (removedComp == null)
                return null;
            return caboodle.Pool.ReleaseComponent(removedComp);
        }

        /// <summary>
        /// Removes all components from the collection.
        /// </summary>
        public void Clear()
        {
            components.Clear();
        }

        /// <summary>
        /// Component Type manager manages. [DEPRECATED]
        /// </summary>
        /// <returns>The type of component the manager is interested in</returns>
        [Obsolete("This method is deprecated, Please use its type instead.")]
        public global::System.Type GetCType()
        {
            return typeof(T);
        }
    }
}
