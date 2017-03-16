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
            caboodle.Systems.Add<MockSystem2>();
            caboodle.Systems.Add<MockSystem1>();
            caboodle.Systems.Init();
            //caboodle.Systems.Add(Assembly.GetAssembly(typeof(SystemTest)));
            Assert.IsNotNull(caboodle, "Failed to initialize.");
        }

        [TestMethod]
        public void TestSystemRuntimeChanges()
        {
            //var entity = caboodle.Entities.Create();
            //entity.AddComponent<Rendering>();
            //entity.AddComponent<Transform>();

           //caboodle.Entities.Remove(0);

            var count = caboodle.Entities.Count;

            for(int i = 0; i < 1000; i++)
            {
                var e = caboodle.Entities.Create();
                e.AddComponent<Transform>();
                caboodle.Systems.Update();
                if (!e.HasComponent<Rendering>())
                    Assert.Fail();
                if (e.HasComponent<Transform>())
                    Assert.Fail();

                caboodle.Systems.Update();

                if (!e.HasComponent<Transform>())
                    Assert.Fail();
                if (e.HasComponent<Rendering>())
                    Assert.Fail();

            }



           // Assert.AreEqual(count, caboodle.Entities.Count);

        }

        public class Transform : Component { public int x, y, z; }
        public class Rendering : Component { public bool test; }

        public class ExampleEvent : IEventArg
        {
            public string message;
        }
    }

    [CaboodleES.Attributes.ComponentUsageAttribute(1,
           CaboodleES.Attributes.LoopType.Update, Aspect.Match, typeof(SystemTest.Transform))]
    public class MockSystem2 : Processor
    {
        public override void Start()
        {
        }

        public override void Process(IDictionary<int, Entity> entities)
        {

            foreach (var entity in entities.Values)
            {
                entity.RemoveComponent<SystemTest.Transform>();
                entity.AddComponent<SystemTest.Rendering>();
            }
        }
    }

    [CaboodleES.Attributes.ComponentUsageAttribute(2,
           CaboodleES.Attributes.LoopType.Update, Aspect.Match, typeof(SystemTest.Rendering))]
    public class MockSystem1 : Processor
    {
        public override void Start()
        {
        }

        public override void Process(IDictionary<int, Entity> entities)
        {

            foreach (var entity in entities.Values)
            {
                entity.RemoveComponent<SystemTest.Rendering>();
                entity.AddComponent<SystemTest.Transform>();
            }
        }
    }
}
