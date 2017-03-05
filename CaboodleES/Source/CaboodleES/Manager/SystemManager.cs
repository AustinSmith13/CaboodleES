using System;
using System.Collections.Generic;
using CaboodleES.System;
using System.Reflection;
using System.Linq;

namespace CaboodleES.Manager
{
    /// <summary>
    /// Extension
    /// </summary>
    public static class TypeLoaderExtensions
    {

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }

    /// <summary>
    /// Manages systems.
    /// </summary>
    public sealed class SystemManager : Interface.IManager
    {
        private readonly List<SystemInfo> systems;
        private readonly Utils.Table<Utils.Table<Entity>> entityCache;
        private readonly Caboodle caboodle;
        private int nextSystemId;


        public SystemManager(Caboodle caboodle)
        {
            this.caboodle = caboodle;
            this.caboodle.Entities.OnChange += OnEntityChange;
            this.caboodle.Entities.OnRemoved += OnEntityRemoved;
            this.systems = new List<SystemInfo>();
            this.entityCache = new Utils.Table<Utils.Table<Entity>>();
            nextSystemId = 0;
        }

        /// <summary>
        /// Removes the entity from every system.
        /// </summary>
        public void OnEntityRemoved(Entity entity)
        {
            for (int i = 0; i < entityCache.Count; i++)
            {
                if (entityCache.Has(i))
                {
                    if (entityCache.Get(i).Has(entity.Id))
                    {
                        systems[i].entities.Remove(entity.Id);
                        entityCache.Get(i).Remove(entity.Id);
                    }
                }
            }
        }

        /// <summary>
        /// Re-catigorize the entity to keep system entity collections up to date and organized.
        /// </summary>
        public void OnEntityChange(int eid)
        {
            Entity entity = this.caboodle.Entities.Get(eid);

            for(int i = 0; i < systems.Count; i++)
            {
                var systemMask = systems[i].mask;

                // match entity mask and system mask
                if(AspectMatcher.Match(systems[i].processor.Aspect, entity.mask, systemMask) && entity.Active)
                {
                    // If the system does not have the entity then add it
                    if (!entityCache.Get(i).Has(entity.Id))
                    {
                        entityCache.Get(systems[i].processor.Id).Set(entity.Id, entity);
                        systems[i].entities.Add(entity.Id, entity);
                    }
                }
                // The entity does not belong in its collection of entities
                else
                {
                    // Check if there is an entry
                    if(entityCache.Has(i))
                    {
                        // Remove the entity from the systems collection if it exists
                        if (entityCache.Get(i).Has(entity.Id))
                        {
                            systems[i].entities.Remove(entity.Id);
                            entityCache.Get(i).Remove(entity.Id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Registers all systems in the assembly.
        /// </summary>
        public void Add(Assembly asm)
        {
            var types = GetTypesWithInterface<object>(asm);

            foreach (Type type in types)
            {
                if (type.BaseType != typeof(Processor)) continue;
                this.Add(type);
            }
        }

        /// <summary>
        /// Instantiates the system-type's class and adds it to the manager along with its attributes.
        /// </summary>
        public void Add<T>() where T : Processor { Add(typeof(T)); }

        /// <summary>
        /// Retrieves a system. Note that systems aka processors should never know about eachother.
        /// Use events for inter system communication.
        /// </summary>
        public T Get<T>() where T : Processor
        {
            foreach(SystemInfo info in systems)
                if (info.processor.GetSystemType() == typeof(T)) return info.processor as T;

            return null;
        }

        /// <summary>
        /// Checks if the Processor of the specified type exists within the systems collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool Has<T>() where T : Processor
        {
            foreach (SystemInfo info in systems)
                if (info.processor.GetSystemType() == typeof(T)) return true;

            return false;
        }

        /// <summary>
        /// Instantiates the system-type's class and adds it to the manager along with its attributes.
        /// </summary>
        public void Add(Type stype)
        {
            // Instantiate system class
            var system = Activator.CreateInstance(stype) as Processor;
            var attr = stype.GetCustomAttributes(typeof(Attributes.ComponentUsageAttribute), true);
            uint priority = 0;
            Aspect aspect = Aspect.Has;
            Attributes.LoopType loop = Attributes.LoopType.Update;
            Type[] comps = null;
            Utils.BitMask systemMask = new Utils.BitMask();

            // Search through attributes
            for (int i = 0; i < attr.Length; i++)
            {
                if (attr[i] is Attributes.ComponentUsageAttribute)
                {
                    Attributes.ComponentUsageAttribute attribute = (Attributes.ComponentUsageAttribute) attr[i];
                    priority = (uint)attribute.priority;
                    aspect = attribute.aspect;
                    comps = attribute.types;
                    loop = attribute.loopType;
                    break;
                }
            }

            if(comps != null)
            {
                for(int i = 0; i < comps.Length; i++)
                {
                    var componentType = comps[i];
                    var classInstance = Activator.CreateInstance(componentType, null);
                    MethodInfo methodInfo = typeof(ComponentManager).GetMethod("RegisterComponent");
                    var m = methodInfo.MakeGenericMethod(componentType);
                    var result = m.Invoke(this.caboodle.Entities.Components, null);
                    int id = this.caboodle.Entities.Components.Get(componentType).GetId();
                    systemMask.Set(id, true);
                }
            }


            system.SetAttr(caboodle, nextSystemId++, priority, aspect, loop, stype);
            SystemInfo info = new SystemInfo(systemMask, system, new Dictionary<int, Entity>());
            entityCache.Set(nextSystemId - 1, new Utils.Table<Entity>());
            systems.Add(info);
        }

        private IEnumerable<Type> GetTypesWithInterface<T>(Assembly asm)
        {
            var it = typeof(T);
            return asm.GetLoadableTypes().Where(it.IsAssignableFrom).ToList();
        }

        public void Process()
        {
            SystemInfo info;
            for (int i = 0; i < systems.Count; i++)
            {
                info = systems[i];
                info.processor.Process(info.entities);
            }

            caboodle.Events.Invoke();
        }

        public void Init()
        {
            systems.Sort((x, y) => x.processor.Priority.CompareTo(y.processor.Priority));

            for (int i = 0; i < systems.Count; i++)
            {
                systems[i].processor.Start();

                if (systems[i].processor.Loop == Attributes.LoopType.Once) {
                    systems[i].processor.Process(systems[i].entities);
                    systems.RemoveAt(i--);
                }

            }
        }

        struct SystemInfo
        {
            public readonly Utils.BitMask mask;
            public readonly Processor processor;
            public readonly Dictionary<int, Entity> entities;

            public SystemInfo(Utils.BitMask mask,
                Processor processor, Dictionary<int,Entity> entities)
            {
                this.mask = mask;
                this.processor = processor;
                this.entities = entities;
            }
        }
    }
}
