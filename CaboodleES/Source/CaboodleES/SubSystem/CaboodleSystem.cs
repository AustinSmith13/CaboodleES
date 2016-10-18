using System;
using System.Collections.Generic;
using CaboodleES.Interface;
using CaboodleES.Utils;


namespace CaboodleES.SubSystem
{
    /// <summary>
    /// Base system.
    /// </summary>
    public abstract class CaboodleSystem
    {
        public uint Priority {  get { return _priority; } set { _priority = value; } }
        public Group Aspect { get { return _aspect; } }
        public Caboodle Caboodle {  get { return _caboodle; } }

        private uint _priority;
        private Group _aspect;
        private Caboodle _caboodle;

        public CaboodleSystem(Caboodle caboodle) { _caboodle = caboodle; }

        public virtual void Start() { }

        public virtual void Update() { }
    }
}
