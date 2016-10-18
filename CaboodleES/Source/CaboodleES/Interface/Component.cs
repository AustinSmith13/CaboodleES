using CaboodleES.Interface;

namespace CaboodleES
{
    public abstract class Component : ICloneable
    {
        public ulong Id { get { return _entityID; } } // Deprecated

        private ulong _entityID; // Deprecated

        internal void SetEntityContext(ulong eid) // Deprecated
        {
            _entityID = eid;
        }
        
        public virtual object Clone()
        {
            return this;
        }
    }
}
