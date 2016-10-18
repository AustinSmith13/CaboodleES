# CaboodleES

*Version 0.5.4 alpha*

A fast and simple **Entity System Framework** library for C# using .Net Framework 3.5.


## How to use?
Components are mostly organized sequentially in memory for fast access.

* **Entities** (*Entity*) - internally are integers (*ulong*) that act as a container for components. An entity can not have multilpe components of the same type.
* **Components** (*Component*) - Hold data.
* **Systems** (*CaboodleSystem*) - Processes components and events.
* **Caboodle** - A collection of entities, components, and systems. Can communicate with other caboodle objects.

A **caboodle** is an entity world that encapsulates entities, components, and systems. Systems can only process entities within the same caboodle they exist in, however you can copy/union data between caboodle objects. 

###Getting started
First include the library.


> ```cs
> using CaboodleES;
> ```

Next, you will want to instantiate a Caboodle object to manage your components, entities, and systems.

> ```
> var caboodle = new Caboodle();
> ```

Call union to unify two caboodle's.

> ```
> caboodle.Union(anotherCaboodle);
> ```

####Adding Entities
You can think of entities as containers of components. Internally they do not actually contain any components they only contain an integer id that can be used as a key for accessing components from component collections. 

To create an entity you must call the create method inside the caboodle object.

> ```cs
> var entity = caboodle.Entities.Create();
> ```

####Removing Entities
Removing an entity will release all of its components back to the pool. 

Entities can be removed in two ways.

> ```cs
> entity.Destroy();
>     // or
> caboodle.Entities.Remove(entity)
> ```

####Adding Components

When you create a new entity and add a new component, a component collection is allocated for storing that specific type of components. Systems choose the component collections they are interested in.

Components can be added in two ways.

> ```cs
> var c = entity.AddComponent<Rigid>();
>     // or
> var c = caboodle.Entities.Components.AddComponent<Rigid>(entity.Id);
> ```

#### Removing Components

Removing a component is slower than adding a component. Each component inside of the component collection that matches its type must be visited to find the indice for removal, this will be optimized in the future. On removal components are released to a pool.

Components can be removed in two ways.
> ```cs
> var c = entity.RemoveComponent<Rigid>();
>     // or
> var c = caboodle.Entities.Components.RemoveComponent<Rigid>(entity.Id);
> ```

## Compiling & Installation

Build the CaboodleES project library then simply use the .dll in the bin folder.

**You will need Visual Studio 2015.**



#### Unity3D Setup
Place the CaboodleES.dll into Unity3D's plugin folder within your project or place the source code inside your assets folder.
Inside of your project create an empty GameObject and add a new monobehavior.

>  ```cs
>  using UnityEngine;
>  using CaboodleES;

>  // The new monobehavior that will interface with CaboodleES
>  public class CaboodleFoo : MonoBehavior
>  {
>      // Entity world
>      private Caboodle world;

>      public CaboodleFoo()
>      {
>          this.world = new Caboodle();
       
>          // Registering a system
>          world.Systems.Register(new FooSystem<FooComp>());
       
>          // Creating an entity
>          CaboodleES.Entity ent = word.Entities.Create();
>          FooComp c = ent.AddComponent<FooComp>();
>          c.data = "Hello, world";
>      }
   
>      void Start()
>      {
>          world.Systems.Start();
>      }
   
>      void Update()
>      {
>          world.Systems.Update();
>      }
>  }

>  public class FooComp : CaboodleES.Component
>  {
>      public string data;
>  }

>  ```


## TODO List
* Remove entities using cache
* System component masks
* inter-system communication using events
* Database for state storage (SQL)
* Net code
* Documentation

