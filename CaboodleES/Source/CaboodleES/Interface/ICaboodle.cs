using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CaboodleES.Interface
{
    public interface ICaboodle
    {
        Manager.EntityManager GetEntityManager();
        Manager.SystemsManager GetSystemsManager();
        void Union(ICaboodle caboodle);
        void Clear();
    }
}
