using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CaboodleES.Interface;
using CaboodleES;


namespace CaboodleEsTest
{
    [TestClass]
    public class EntityComponentTest
    {
        Caboodle caboodle;

        [TestInitialize]
        public void InitTest()
        {
            try
            {
                caboodle = new Caboodle();
            }
            finally { Assert.IsNotNull(caboodle, "Failed to initialize ECS."); }
        }

        [TestMethod]
        public void TestEntityComponentsData()
        {
            var ent = caboodle.Entities.Create();
            var c1 = ent.AddComponent<MockTransform>();
            c1.x = 123;
            c1.y = 3210;

            var c2 = ent.GetComponent<MockTransform>();

            Assert.AreEqual(123, c2.x);
            Assert.AreEqual(3210, c2.y);
        }

        [TestMethod]
        public void TestGetComponentData()
        {
            float expected = 51f;
            Entity entityToTest = null;
            for(int i = 0; i < 10000; i++)
            {
                Entity newEnt = caboodle.Entities.Create();
                newEnt.AddComponent<MockTransform>().x = i;
                if (i == expected)
                    entityToTest = newEnt;
            }

            MockTransform comp = entityToTest.GetComponent<MockTransform>();

            Assert.AreEqual(expected, comp.x);
        }

        [TestMethod]
        public void TestAAEngineStressEntityComponentAddRemoval()
        {
            List<Entity> ents = new List<Entity>();
            int iterations = 3000;

            for (int i = 0; i < iterations; i++)
            {
                var ent = caboodle.Entities.Create();
                ent.AddComponent<Mock1>();
                var c = ent.AddComponent<MockTransform>();
                c.x = 234;
                c.y = 3434;
                ent.AddComponent<Mock3>();
                ents.Add(ent);
            }

            for (int i = 0; i < 15000; i++)
            {
                var ent = caboodle.Entities.Create();
                ent.AddComponent<Mock1>();
                ent.AddComponent<Mock2>();
                ent.AddComponent<Mock3>();
                ent.AddComponent<Mock4>();
                ent.AddComponent<Mock5>();
                ent.AddComponent<Mock6>();
                ent.AddComponent<MockTransform>();
            }

            for (int i = 0; i < iterations; i++)
            {
                var ent = ents[i];
                ent.GetComponent<Mock1>();
                ent.Destroy();
            }

            Assert.AreEqual(15000, caboodle.Entities.Count);
        }

        [TestMethod]
        public void TestEntityAddRemoveComponents()
        {
            var ent = caboodle.Entities.Create();
            ent.AddComponent<Mock1>();
            ent.AddComponent<Mock2>();
            ent.AddComponent<Mock3>();
            ent.AddComponent<Mock4>();
            ent.AddComponent<Mock5>();
            ent.AddComponent<Mock6>();

            var c1 = ent.GetComponent<Mock1>();
            var c2 = ent.GetComponent<Mock2>();
            var c3 = ent.GetComponent<Mock3>();
            var c4 = ent.GetComponent<Mock4>();
            var c5 = ent.GetComponent<Mock5>();
            var c6 = ent.GetComponent<Mock6>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            Assert.IsNotNull(c4);
            Assert.IsNotNull(c5);
            Assert.IsNotNull(c6);

            ent.RemoveComponent<Mock1>();
            ent.RemoveComponent<Mock2>();
            ent.RemoveComponent<Mock3>();
            ent.RemoveComponent<Mock4>();
            ent.RemoveComponent<Mock5>();
            ent.RemoveComponent<Mock6>();
            ent.RemoveComponent<Mock1>();

           
        }

        [TestMethod]
        public void TestAddRemoveEntity()
        {
            var ent = caboodle.Entities.Create();

            ent.Destroy();
        }

        

        private class Mock1 : Component { }
        private class Mock2 : Component { }
        private class Mock3 : Component { }
        private class Mock4 : Component { }
        private class Mock5 : Component { }
        private class Mock6 : Component { }

        private class MockTransform : Component
        {
            public float x;
            public float y;
        }
    }
}
