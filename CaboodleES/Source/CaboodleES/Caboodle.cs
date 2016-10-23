using CaboodleES.Manager;


namespace CaboodleES
{
    /// <summary>
    /// Caboodle holds the entity world : Version 0.6.5
    /// </summary>
    public sealed class Caboodle : Interface.ICaboodle
    {
        #region Properties

        public EntityManager Entities { get { return _entityManager; } }
        public SystemsManager Systems { get { return _systemsManager; } }
        internal PoolManager Pool { get { return _poolManager; } }
        
        #endregion

        private EntityManager _entityManager;
        private SystemsManager _systemsManager;
        private PoolManager _poolManager;


        public Caboodle()
        {
            _entityManager = new EntityManager(this); 
            _systemsManager = new SystemsManager(this);
            _poolManager = new PoolManager(8191);
        }

        public EntityManager GetEntityManager()
        {
            return _entityManager;
        }

        public SystemsManager GetSystemsManager()
        {
            return _systemsManager;
        }

        public void Union(Interface.ICaboodle caboodle)
        {
            //this.Entities.Has
        }

        public void Clear()
        {
            _entityManager.Clear();
        }

        public override string ToString()
        {
            return _entityManager.ToString();
        }
    }
}
