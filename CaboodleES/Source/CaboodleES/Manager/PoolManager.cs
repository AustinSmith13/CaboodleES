using System.Collections.Generic;
using CaboodleES.Interface;


namespace CaboodleES.Manager
{
    internal sealed class PoolManager
    {
        private Dictionary<System.Type, Stack<Component>> reusableComponents =
            new Dictionary<System.Type, Stack<Component>>();
        private readonly int maxCompPerType = 100;

        public PoolManager()
        {

        }

        public PoolManager(int maxComps)
        {
            this.maxCompPerType = maxComps;
        }

        public Component ReleaseComponent(Component component)
        {
            return null;
            Stack<Component> cs = null;
            if(!reusableComponents.TryGetValue(component.GetType(), out cs))
            {
                cs = new Stack<Component>();
                reusableComponents.Add(component.GetType(), cs);
            }

            if (cs.Count > maxCompPerType)
                return component;

            cs.Push(component);
            return component;
        }

        public T CreateComponent<T>() where T : Component, new()
        {
            Stack<Component> cstack = null;
            if(!reusableComponents.TryGetValue(typeof(T), out cstack))
            {
                cstack = new Stack<Component>();
                for(int i = 0; i < maxCompPerType; i++)
                {
                    cstack.Push(new T());
                }

                reusableComponents.Add(typeof(T), cstack);
            }

            else if(cstack.Count == 0)
            {
                for (int i = 0; i < maxCompPerType; i++)
                {
                    cstack.Push(new T());
                }
            }

            return (T) cstack.Pop();
        }

        public void Clear()
        {
            reusableComponents.Clear();
        }
    }
}