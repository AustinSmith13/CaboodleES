using CaboodleES.Interface;

namespace CaboodleES
{
    public abstract class Component : ICloneable
    {
        public virtual object Clone()
        {
            return this;
        }
    }
}
