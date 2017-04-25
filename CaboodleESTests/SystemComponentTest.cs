using System;
using System.Collections.Generic;
using CaboodleES;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuroraTests
{
    [TestClass]
    public class SystemComponentTest
    {
        Caboodle caboodle;

        [TestInitialize]
        public void TestInit()
        {
            caboodle = new Caboodle();
            caboodle.Systems.Add<MockSystem1>();
            caboodle.Systems.Add<MockSystem2>();
            caboodle.Systems.Add<MockSystem3>();
            caboodle.Systems.Init();

            Assert.IsNotNull(caboodle, "Failed to initialize.");
        }

        [TestMethod]
        public void TestSystemComponentContinuity()
        {
            var ent = caboodle.Entities.Create();
            ent.AddComponent<Mock1>();
            ent.AddComponent<Mock2>();
            caboodle.Systems.Update();
        }

        [TestMethod]
        public void TestSystemComponentRedundancy()
        {
            List<Entity> ents = new List<Entity>();
            for (int i = 0; i < 100; i++)
            {
                var ent = caboodle.Entities.Create();
                ent.AddComponent<Mock3>();
                ents.Add(ent);
            }

            caboodle.Systems.Update();
            caboodle.Systems.Update();
            
            caboodle.Systems.Update();
        }


        [CaboodleES.Attributes.ComponentUsageAttribute(1,
           CaboodleES.Attributes.LoopType.Update, CaboodleES.System.Aspect.Match, typeof(Mock1), typeof(Mock2))]
        public class MockSystem1 : CaboodleES.System.Processor
        {
            public override void Start()
            {
            }

            public override void Process(IDictionary<int, Entity> entities)
            {
                //if (entities.Count > 0) Assert.Fail();
                foreach (var entity in entities.Values)
                {

                    if (entity == null)
                        Assert.Fail();

                    //entity.RemoveComponent<Mock2>();
                    RemoveEntity(entity);
                    RemoveEntity(entity); RemoveEntity(entity);
                    RemoveEntity(entity);

                    Caboodle.Entities.Get(entity.Id); // This statment should never throw an exception within a System loop
                }
            }
        }

        private class Mock1 : Component { public override void Reset() { } }
        private class Mock2 : Component { public override void Reset() { } }
        private class Mock3 : Component { public override void Reset() { } }

        [CaboodleES.Attributes.ComponentUsageAttribute(2,
           CaboodleES.Attributes.LoopType.Update, CaboodleES.System.Aspect.Has, typeof(Mock2))]
        public class MockSystem2 : CaboodleES.System.Processor
        {
            public override void Start()
            {
            }

            public override void Process(IDictionary<int, Entity> entities)
            {
                foreach (var entity in entities.Values)
                {
                    var c = entity.GetComponent<Mock2>();
                    if (c == null)
                        Assert.Fail();

                    if (entity == null)
                        Assert.Fail();

                

                    Caboodle.Entities.Get(entity.Id); // This statment should never throw an exception within a System loop
                }
            }
        }

        [CaboodleES.Attributes.ComponentUsageAttribute(5,
           CaboodleES.Attributes.LoopType.Update, CaboodleES.System.Aspect.Has, typeof(Mock3))]
        public class MockSystem3 : CaboodleES.System.Processor
        {
            public override void Start()
            {
            }

            public override void Process(IDictionary<int, Entity> entities)
            {
                foreach (var entity in entities.Values)
                {
                    var c = entity.GetComponent<Mock3>();
                    if (c == null)
                        Assert.Fail();

                    if (entity == null)
                        Assert.Fail();

                    Caboodle.Entities.Get(entity.Id); // This statment should never throw an exception within a System loop
                }
            }
        }
    }
}
