using System.Collections.Generic;
using CaboodleES.Utils;
using CaboodleES.Interface;


namespace CaboodleES.Manager
{
    /// <summary>
    /// Manages all entities (entities are simply ulong that point to components).
    /// </summary>
    public sealed class EntityManager
    {
        #region Properties

        /// <summary>
        /// The number of entities in the collection.
        /// </summary>
        public int Count
        {
            get { return _entities.Count; }
        }

        /// <summary>
        /// The component manager.
        /// </summary>
        public ComponentManager Components
        {
            get { return _components; }
        }

        #endregion

        private ComponentManager _components;
        private Dictionary<ulong, Entity> _entities;
        private Caboodle world;
        private ulong next;

        public EntityManager(Caboodle world)
        {
            _entities = new Dictionary<ulong, Entity>();
            this._components = new ComponentManager(world);
            this.world = world;
        }

        /// <summary>
        /// Creates an entity.
        /// </summary>
        /// <returns></returns>
        public Entity Create()
        {
            var entity = new Entity(world, next++);
            _entities.Add(entity.Id, entity);
            return entity;
        }

        /// <summary>
        /// Gets the entity associated with the eid.
        /// </summary>
        public Entity Get(ulong eid)
        {
            Entity ent = null;
            _entities.TryGetValue(eid, out ent);

            if (ent == null)
                throw new NoSuchEntityException("No such entity, " + eid, eid);

            return ent;
        }

        /// <summary>
        /// Returns true if the entity id exists.
        /// </summary>
        public bool Has(ulong eid)
        {
            return _entities.ContainsKey(eid);
        }

        /// <summary>
        /// Returns true if the entity exists.
        /// </summary>
        public bool Has(Entity entity)
        {
            return _entities.ContainsKey(entity.Id);
        }

        /// <summary>
        /// Removes an entity and its components (slow).
        /// </summary>
        public void Remove(ulong eid)
        {
            _entities.Remove(eid);
            _components.RemoveComponents(eid);
        }

        /// <summary>
        /// Removes an entity and its components (slow).
        /// </summary>
        public void Remove(ref Entity entity)
        {
            _entities.Remove(entity.Id);
            _components.RemoveComponents(entity.Id);
            entity = null;
        }

        /// <summary>
        /// Removes all entities and there associated componenets.
        /// </summary>
        public void Clear()
        {
            _entities.Clear();
            _components.Clear();
        }
    }
}