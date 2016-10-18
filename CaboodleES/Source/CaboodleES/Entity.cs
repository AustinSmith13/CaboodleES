using CaboodleES.Interface;


namespace CaboodleES
{
    /// <summary>
    /// Simple handle to manipulate components.
    /// </summary>
    public sealed class Entity
    {
        public ulong Id { get { return _id; } }
        public Caboodle World { get { return _world; } }

        private readonly ulong _id;
        private readonly Caboodle _world;

        /// <summary>
        /// Constructor should only be called by CaboodleES.
        /// </summary>
        /// <param name="entityWorld"></param>
        /// <param name="id"></param>
        internal Entity(Caboodle entityWorld, ulong id)
        {
            this._world = entityWorld;
            this._id = id;
        }

        /// <summary>
        /// Gets the component.
        /// </summary>
        public C GetComponent<C>() where C : Component
        {
            return _world.Entities.Components.GetComponent<C>(_id);
        }

        /// <summary>
        /// Adds the component.
        /// </summary>
        public C AddComponent<C>() where C : Component, new()
        {
            return _world.Entities.Components.AddComponent<C>(_id);
        }

        /// <summary>
        /// Removes the component.
        /// </summary>
        public C RemoveComponent<C>() where C : Component
        {
            return _world.Entities.Components.RemoveComponent<C>(_id);
        }

        /// <summary>
        /// Checks if the entity has a component of type {C}.
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <returns></returns>
        public bool HasComponent<C>() where C : Component
        {
            return _world.Entities.Components.HasComponent<C>(_id);
        }

        /// <summary>
        /// Deletes all components associated with the entity.
        /// </summary>
        public void Destroy()
        {
            _world.Entities.Remove(_id);
        }

        public override string ToString()
        {
            return string.Format("[Entity - {0}] ", this._id);
        }
    }
}
