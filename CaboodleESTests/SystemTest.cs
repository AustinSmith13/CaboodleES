using global::System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CaboodleES;
using CaboodleES.System;
using CaboodleES.Interface;
using System;

namespace CaboodleEsTest
{
    [TestClass]
    public class SystemTest
    {
        Caboodle caboodle;

        [TestInitialize]
        public void TestInit()
        {
            caboodle = new Caboodle();
            //caboodle.Systems.Add(Assembly.GetAssembly(typeof(SystemTest)));
            Assert.IsNotNull(caboodle, "Failed to initialize.");
        }

        [TestMethod]
        public void TestSystem()
        {
            caboodle.Systems.Add<MockSystem1>();
            caboodle.Systems.Add<MockSystem2>();
            var entity = caboodle.Entities.Create();
            entity.AddComponent<Rendering>();
            entity.AddComponent<Transform>();
            //caboodle.Systems.Process();
            
            Assert.AreEqual(0, entity.GetComponent<Transform>().z);
            caboodle.Systems.Init();

            Assert.AreEqual(3, entity.GetComponent<Transform>().z);

            caboodle.Systems.Process();
            var entity2 = caboodle.Entities.Create();
            entity2.AddComponent<Rendering>();
            entity.Destroy();
            ////caboodle.Systems.Process();
            //Assert.AreEqual(true, entity2.GetComponent<Rendering>().test);
            //Assert.AreEqual(true, entity.GetComponent<Rendering>().test);
        }

        public class Transform : Component { public int x, y, z; }
        public class Rendering : Component { public bool test; }

        public class ExampleEvent : IEventArg
        {
            public string message;
        }

        [CaboodleES.Attributes.AddHandle(typeof(int))]
        [CaboodleES.Attributes.ComponentUsageAttribute(1, CaboodleES.Attributes.LoopType.Once,
            Aspect.Match, typeof(Transform), typeof(Rendering))]
        public class MockSystem1 : Processor
        {

            public override void Start()
            {
                var e = new ExampleEvent();
                e.message = "hello, world";
                AddEvent<ExampleEvent>(e);
            }

            public override void Process(IDictionary<int,Entity> entities)
            {
                if (entities == null) return;
                
                foreach(var entity in entities.Values) {

                    entity.GetComponent<Transform>().x = 1;
                    entity.GetComponent<Transform>().y = 2;
                    entity.GetComponent<Transform>().z = 3;
                    entity.GetComponent<Rendering>().test = true;
                }

            }

            public bool TestMethod()
            {
                return true;
            }
        }

        [CaboodleES.Attributes.ComponentUsageAttribute(2, Aspect.Match, typeof(Rendering))]
        public class MockSystem2 : Processor
        {
            public override void Start()
            {
                AddHandler<ExampleEvent>(ExampleHandler);
            }

            public override void Process(IDictionary<int, Entity> entities)
            {
                if (entities == null) return;

                foreach (var entity in entities.Values)
                {
                    //entity.GetComponent<Rendering>().test = true;
                }
            }

            public void ExampleHandler(ExampleEvent e)
            {
                Assert.AreEqual("hello, world", e.message);
            }
        }
    }
}
