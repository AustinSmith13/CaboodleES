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

			Stopwatch watch = new Stopwatch();

			var world = new Caboodle();
			world.Systems.Add(Assembly.GetAssembly(typeof(Program)));
			int iterations = 1000;
			List<Entity> buffer = new List<Entity>();


			while (true)
			{
				world.Systems.Update();
				var a = Console.ReadKey(true);

				if (a.KeyChar == 'q')
					global::System.GC.Collect();

				if (a.KeyChar == 'c')
				{
					watch.Reset();
					watch.Start();

					for (int i = 0; i < iterations; i++)
					{
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
					for (int i = 0; i < buffer.Count; i++)
					{
						buffer[i].GetComponent<Test>();

					}
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

	[Attributes.SystemAttr(100, typeof(Test))]
	public class MockSystem1 : System.SystemBase
	{
		public MockSystem1() { }

	public override void Process(IList<Entity> entities)
	{
		Console.WriteLine("MockSystem1 processing...");
	}
	}

		public class MockSystem2 : System.SystemBase
	{
		public MockSystem2() { }

	public override void Process(IList<Entity> entities)
	{
		Console.WriteLine("MockSystem2 processing...");
	}
	}

#pragma warning restore
}