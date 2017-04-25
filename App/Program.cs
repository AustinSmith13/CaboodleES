using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using CaboodleES.Interface;
using System.Reflection;


namespace CaboodleES
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Caboodle.GetVersion());

            Stopwatch watch = new Stopwatch();

            var world = new Caboodle();
           
            //world.Systems.Add(Assembly.GetAssembly(typeof(Program)));
            world.Systems.Add<MockSystem1>();
            world.Systems.Add<MockSystem2>();

            var system = world.Systems.Get<MockSystem1>();
            Console.WriteLine(system);
            var system2 = world.Systems.Get<MockSystem2>();
            Console.WriteLine(system2);
            world.Systems.Init();
            int iterations = 1000;
            List<Entity> buffer = new List<Entity>();

            while (true)
            {
                world.Systems.Update();
                var a = Console.ReadKey(true);

                if(a.KeyChar == 'a')
                {

                }

                if (a.KeyChar == 'q')
                {
                    watch.Reset();
                    watch.Start();

                    world.Systems.Update();

                    Console.WriteLine("Systems Processed : " + watch.Elapsed.TotalMilliseconds + " ms");
                }

                if(a.KeyChar == 'w')
                {
                    watch.Reset();
                    watch.Start();

                    var ent = world.Entities.Create();
                    var c1 = ent.AddComponent<Test>();
                    c1.x = 33434334343;
                    c1.y = 34234234234;
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

                    Console.WriteLine("created ents/comps - TIME : " + watch.Elapsed.TotalMilliseconds + " ms");

                }

                if (a.KeyChar == 'c')
                {
                    watch.Reset();
                    watch.Start();

                    for (int i = 0; i < iterations; i++)
                    {
                        var ent = world.Entities.Create();
                        var c1 = ent.AddComponent<Test>();
                        c1.x = 1;
                        c1.y = 1;
                        MockTransform c = ent.AddComponent<MockTransform>();
                        ent.AddComponent<Mock1>();
                        ent.AddComponent<Mock2>();
                        ent.AddComponent<Mock3>();
                        ent.AddComponent<Mock4>();
                        ent.AddComponent<Mock5>();
                        ent.AddComponent<Mock6>();
                        ent.AddComponent<Mock7>();
                        c.x = 234;
                        c.y = 3434;
                        buffer.Add(ent);

                        ent = world.Entities.Create();
                        ent.AddComponent<Mock1>().x = 9999;
                        buffer.Add(ent);
                    }

                    Console.WriteLine("created ents/comps - TIME : " + watch.Elapsed.TotalMilliseconds + " ms");
                }

                if (a.KeyChar == 'i')
                {
                    Console.WriteLine("Entity Count - " + world.Entities.Count);
                }

                if (a.KeyChar == 'g')
                {
                    watch.Reset();
                    watch.Start();

                    var c = buffer[750].GetComponent<Test>();

                    Console.WriteLine(c.x);
                    
                    // if (buffer.Count > 0)
                    //Console.WriteLine("Value = "+buffer[765].GetComponent<Test>().x);

                    Console.WriteLine("get ents comp - TIME : " + watch.Elapsed.TotalMilliseconds + " ms");
                }

                if (a.KeyChar == 'r')
                {
                    watch.Reset();
                    watch.Start();

                    Console.WriteLine(buffer.Count);

                    for (int i = 0; i < buffer.Count; i++)
                    {
                        buffer[i].Destroy();
                    }

                    buffer.Clear();

                    Console.WriteLine("deleted ents/comps - TIME : " + watch.Elapsed.TotalMilliseconds + " ms");
                }

            }
        }



#pragma warning disable 0649 

        public class Mock1 : Component { public ulong x; public ulong y; public ulong z; public override void Reset() { } }
        public class Mock2 : Component { public ulong x; public ulong y; public ulong z; public override void Reset() { } }
        public class Mock3 : Component { public ulong x; public ulong y; public ulong z; public override void Reset() { } }
        public class Mock4 : Component { public ulong x; public ulong y; public ulong z; public override void Reset() { } }
        public class Mock5 : Component { public ulong x; public ulong y; public ulong z; public override void Reset() { } }
        public class Mock6 : Component { public ulong x; public ulong y; public ulong z; public override void Reset() { } }
        public class Mock7 : Component { public ulong x; public ulong y; public ulong z; public override void Reset() { } }
        public class MockTransform : Component
        {
            public float x;
            public float y;
            public override void Reset() { x = 0f; y = 0f; }
        }
    }

    public class Test : Component
    {
        public float x;
        public float y;
        public override void Reset() { x = 0f; y = 0f; }
    }

    public class Trans : Component
    {
        public float x;
        public float y;
        public override void Reset() { x = 0f; y = 0f; }
    }

    public class ExampleEvent : IEvent
    {
        string message;
    }

    [Attributes.ComponentUsageAttribute(100, System.Aspect.Has, typeof(Program.Mock1))]
    public class MockSystem1 : System.Processor
    {
        Stopwatch watch = new Stopwatch();

        public override void Start()
        {
            Console.WriteLine("Adding Event!");
        }

        public override void Process(IDictionary<int, Entity> entities)
        {
            watch.Reset();
            watch.Start();
            foreach (var entity in entities.Values)
            {
                entity.Destroy();
            }
            AddEvent<ExampleEvent>(new ExampleEvent());
            Console.WriteLine("MockSystem1 processing..." + entities.Count);
            Console.WriteLine("MockSystem1 processing - TIME : " + watch.Elapsed.TotalMilliseconds + " ms");
        }


    }

    [Attributes.ComponentUsageAttribute(99, Attributes.LoopType.Once, System.Aspect.Has, typeof(Test))]
    public class MockSystem2 : System.Processor
    {
        public override void Start()
        {
            Console.WriteLine("Adding Handler");
            AddHandler<ExampleEvent>(ExampleHandler);
        }

        public override void Process(IDictionary<int, Entity> entities)
        {
            Console.WriteLine("MockSystem2 processing..." + entities.Count);
        }

        public void ExampleHandler(ExampleEvent eventArg)
        {
            Console.WriteLine("Recieved Message!!!");
        }
    }

#pragma warning restore
}