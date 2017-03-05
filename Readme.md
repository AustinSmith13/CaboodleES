# CaboodleES

A fast and simple **Entity System Framework** library for C# using .Net Framework 3.5.


## How to use?
Components are mostly organized sequentially in memory for fast access.

* **Entities** (*Entity*) - internally are integers (*ulong*) that act as a container for components. An entity can not have multilpe components of the same type.
* **Components** (*Component*) - Hold data.
* **Systems** (*Processor*) - Processes components and events.
* **Caboodle** - A collection of entities, components, and systems. Can communicate with other caboodle objects.

A **caboodle** is an entity world that encapsulates entities, components, and systems. Systems can only process entities within the same caboodle they exist in, however you can copy/union data between caboodle objects. 

###Getting started
First include the library.


 ```cs
 using CaboodleES;
 ```

Next, you will want to instantiate a Caboodle object to manage your components, entities, and systems.

 ```cs
 var caboodle = new Caboodle();
 ```

Call union to unify two caboodle's.

 ```cs
 caboodle.Union(anotherCaboodle);
 ```

####Adding Entities
You can think of entities as containers of components. Internally they do not actually contain any components they only contain an integer id that can be used as a key for accessing components from component collections. 

To create an entity you must call the create method inside the caboodle object.

 ```cs
 var entity = caboodle.Entities.Create();
 ```

####Removing Entities
Removing an entity will release all of its components back to the pool. 

Entities can be removed two ways.

 ```cs
 entity.Destroy();
     // or
 caboodle.Entities.Remove(entity)
 ```

####Adding Components

When you create a new entity and add a new component, a component collection is allocated for storing that specific type of components. Systems choose the component collections they are interested in.

Components can be added two ways.

 ```cs
 var c = entity.AddComponent<Rigid>();
     // or
 var c = caboodle.Entities.Components.AddComponent<Rigid>(entity.Id);
 ```

#### Removing Components

Components can be removed two ways.
 ```cs
 var c = entity.RemoveComponent<Rigid>();
     // or
 var c = caboodle.Entities.Components.RemoveComponent<Rigid>(entity.Id);
 ```
 
 #### Processors (Systems)
 Processors (Systems) process (Update) a set of entities that contain the components of interest.
 
 ##### Attributes
 [CaboodleES.Attributes.ComponentUsageAttribute(
 priority, loopType, aspect, components...)]
 * Priority - Used to organize the execution of systems
 * loopType - Determines how the processor will be executed (e.g. Once, Update, Reactive)
 * Aspect - The type of interest in the set of components (e.g. Complement, Has, Match)
 * Components - The set of components of interest in respect to the aspect
 
 
 ##### Inter-System-Communication
 Processors can communicate with each other via the EventManager. Use the AddEvent<>(myEvent) method to add a new event. Newly added events
 are handled at the end of an update (process). To add a handler to handle the event(s) use AddHandler<>(myHandler) method.
 
 ##### Usage
 ```cs
 public class ExampleEvent : IEventArg {
    public string exampleVariable;
 }
 
 [CaboodleES.Attributes.ComponentUsageAttribute(1, Attributes.LoopType.Update, Aspect.Match, typeof(ExampleComponent))]
 public class ExampleSystem : Processor
 {
    public override void Start() 
    {
        AddHandler<ExampleEvent>(ExampleHandler);
    }
    
    public override void Process(IDictionary<int, Entity> entities) 
    {
        foreach(var entity in entities.Values) 
        {
            // entities to process
        }
        
        // Add a new event for testing purposes
        AddEvent<ExampleEvent>(new ExampleEvent());
    }
    
    // The handler is called when an event of its type is added
    public void ExampleHandler(ExampleEvent e) 
    {
    
    }
 }
 ```
 
 
## Compiling & Installation

Build the CaboodleES project library then use the .dll in the bin folder.

**You will need Visual Studio 2015.**



#### Unity3D Setup
Place the CaboodleES.dll into Unity3D's plugin folder within your project or place the source code inside your assets folder.
Inside of your project create an empty GameObject and add a new monobehavior.

  ```cs
  using UnityEngine;
  using CaboodleES;

  // The new monobehavior that will interface with CaboodleES
  public class CaboodleFoo : MonoBehavior
  {
      // Entity world
      private Caboodle world;

      public CaboodleFoo()
      {
          this.world = new Caboodle();
      
          // Registers all systems within the calling assembly
          world.Systems.Add(Assembly.GetCallingAssembly());
          
          // To add individual systems
          world.Systems.Add<MyProcessor>();
      
          // Creating an entity
          CaboodleES.Entity ent = word.Entities.Create();
          FooComp c = ent.AddComponent<FooComp>();
          c.data = "Hello, world";
      }
  
      void Start()
      {
          world.Systems.Init();
      }
  
      void Update()
      {
          world.Systems.Process();
      }
  }

  public class FooComp : CaboodleES.Component
  {
      public string data;
  }

  ```


## TODO List
* Optimize memory usage
* system loop-type attribute
* update documentation

