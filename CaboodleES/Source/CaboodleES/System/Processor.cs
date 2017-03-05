using System;
using System.Collections.Generic;
using CaboodleES.Interface;


namespace CaboodleES.System
{
    /// <summary>
    /// Processes entities.
    /// </summary>
    public abstract class Processor
    {
        public uint Priority {  get { return _priority; } set { _priority = value; } }
        public bool Active { get { return _active; } set { _active = value; } }
        public Caboodle Caboodle {  get { return _caboodle; } }
        public int Id {  get { return _systemId; } }
        public Aspect Aspect { get { return _aspect; } }
        public Attributes.LoopType Loop { get { return _loopType; } }

        private bool _active;
        private int _systemId;
        private uint _priority;
        private Aspect _aspect;
        private Attributes.LoopType _loopType;
        private Caboodle _caboodle;
        private global::System.Type systemType;

        internal void SetAttr(Caboodle c, int id, uint priority, Aspect aspect, 
            Attributes.LoopType loopType, global::System.Type systemType)
        {
            this._caboodle = c;
            this._systemId = id;
            this._priority = priority;
            this._aspect = aspect;
            this._loopType = loopType;
            this.systemType = systemType;
        }

        public abstract void Start();
        public abstract void Process(IDictionary<int, Entity> entities);

        public void AddHandler<E>(Action<E> handler) where E : IEventArg { _caboodle.Events.AddHandler<E>(handler); }

        public void AddEvent<E>(E @event) where E : IEventArg { _caboodle.Events.AddEvent<E>(@event); }

        public global::System.Type GetSystemType()
        {
            return systemType;
        }
    }
}
