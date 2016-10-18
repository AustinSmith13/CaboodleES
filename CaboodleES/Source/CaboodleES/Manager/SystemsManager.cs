using System;
using System.Collections.Generic;
using CaboodleES.SubSystem;

namespace CaboodleES.Manager
{
    public sealed class SystemsManager
    {
        private List<CaboodleSystem> _systems = new List<CaboodleSystem>();
        private Caboodle caboodle;


        public SystemsManager(Caboodle caboodle)
        {
            this.caboodle = caboodle;
        }

        public void Register(CaboodleSystem system, params Type[] group)
        {
            _systems.Add(system);
            _systems.Sort((x, y) => x.Priority.CompareTo(y.Priority));
        }

        /// <summary>
        /// Must be called first before use.
        /// </summary>
        public void Start()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].Start();
            }
        }


        public void Update()
        {
            for (int i = 0; i < _systems.Count; i++)
            {
                _systems[i].Update();
            }
        }
    }
}
