using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.SQLite;
using CaboodleES.Interface;


namespace CaboodleES
{
    class Program
    {
        static void Main(string[] args)
        {

            Stopwatch watch = new Stopwatch();

            var world = new Caboodle();
            int iterations = 1000;

            while (true)
            {
                var a = Console.ReadKey(true);
                watch.Reset();
                watch.Start();

                List<Entity> buffer = new List<Entity>();

                for (int i = 0; i < iterations; i++)
                {
                    var ent = world.Entities.Create();
                    ent.AddComponent<Test>();
                    MockTransform c = ent.AddComponent<MockTransform>();
                    ent.AddComponent<Mock1>();
                    ent.AddComponent<Mock2>();
                    ent.AddComponent<Mock3>();
                    ent.AddComponent<Mock4>();
                    ent.AddComponent<Mock5>();
                    ent.AddComponent<Mock6>();
                    c.x = 234;
                    c.y = 3434;
                    buffer.Add(ent);

                }

                Console.WriteLine("created ents/comps - TIME : " + watch.Elapsed.TotalMilliseconds + " ms");

                watch.Reset();
                watch.Start();

                for(int i = 0; i < buffer.Count; i++)
                {
                    buffer[i].Destroy();
                }

                Console.WriteLine("deleted ents/comps - TIME : " + watch.Elapsed.TotalMilliseconds + " ms");

            }
        }


        

        public class Mock1 : Component { public ulong x; public ulong y; public ulong z; }
        public class Mock2 : Component { public ulong x; public ulong y; public ulong z; }
        public class Mock3 : Component { public ulong x; public ulong y; public ulong z; }
        public class Mock4 : Component { public ulong x; public ulong y; public ulong z; }
        public class Mock5 : Component { public ulong x; public ulong y; public ulong z; }
        public class Mock6 : Component { public ulong x; public ulong y; public ulong z; }
        public class MockTransform : Component
        {
            public float x;
            public float y;
        }
    }

    public class Test : Component
    {
        public float x;
        public float y;

    }
}
