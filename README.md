# TEDCore

TEDCore is a Unity framework.
The major purpose is to provide the features and tools for developing in a stable and convenient ways.

## Features

### Singleton
Singleton is based on Singleton pattern.
It does force you to inherit MonoBehaviour when you want to register a new service in Services.

#### Namespace
TEDCore

#### Examples
```
using UnityEngine;  
using TEDCore;  

public class EventManager : Singleton<EventManager>  
{  
    public void SendEvent(){}  
}  

public class ResourceSystem : MonoSingleton<ResourceSystem>  
{  
    public void Load() { }  
}  

public class ExampleClass : MonoBehaviour  
{  
    private void Awake()  
    {  
        EventManager.Instance.SendEvent();  
        ResourceSystem.Instance.Load();  
    }  
}  
```

### State Manager
State and Task Manager are the most important parts in this library.
The major concept is based on State pattern.
Each State is a single hierarchy and processing Task that is registered inside it.
State Manager consists two parts State and StateManager.
State just likes the scenes in Unity Engine and have the higher control level than Unity scene.

#### Namespace
TEDCore.StateManagement

#### Public Methods
| Name                         | Parameters                                                  | Description                                                                                                                        |
|------------------------------|-------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------|
| ChangeState(State newState)  | newState: The new state you want to switch to               | ChangeState is called when you want to switch from current state to a new state                                                    |
| Update(float deltaTime)      | deltaTime: The Time.deltaTime for the game environment      | Update is called every frame, it should connect with MonoBehaviour.Update() and sync Time.deltaTime for it.                        |
| FixedUpdate(float deltaTime) | deltaTime: The Time.fixedDeltaTime for the game environment | FixedUpdate is called every physics frame, it should connect with MonoBehaviour.FixedUpdate() and sync Time.fixedDeltaTime for it. |
| LateUpdate(float deltaTime)  | deltaTime: The Time.deltaTime for the game environment      | LateUpdate is called every frame, it should connect with MonoBehaviour.LateUpdate() and sync Time.deltaTime for it.                |

#### Examples
```
using TEDCore.StateManagement;

public class NewState : State
{
    private enum ENewState
    {

    }

    public NewState(StateManager stateManager) : base(stateManager)
    {

    }
}
```
```
public class ExampleClass : MonoBehaviour
{
    private StateManager m_stateManager;

    private void Awake()
    {
        m_stateManager = new StateManager();
        m_stateManager.ChangeState(new EmptyState(m_stateManager));
    }

    private void Update()
    {
        m_stateManager.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        m_stateManager.FixedUpdate(Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        m_stateManager.LateUpdate(Time.deltaTime);
    }
}
```

### Task Manager
Task Manager need to cooperate with with State Manager.
In each State you can register multiple Task inside it and each Task works as a single station of the game logic which means we need to follow Single Responsibility Principle in each specific Task.
Then change the task state in TaskManager can process the specific Task.
Task Manager consists two parts Task and TaskManager.

#### Namespace
TEDCore.StateManagement

#### Public Methods
| Name                                           | Parameters                                                                                                | Description                                                                                                                        |
|------------------------------------------------|-----------------------------------------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------|
| AddTask(Task task, params Enum[] activeStates) | task: The task you want to register<br>activeStates: The activie states you want to register for the task | Register the task to the active states with enum type                                                                              |
| AddTask(Task task, long activeStates)          | task: The task you want to register<br>activeStates: The activie states you want to register for the task | Register the task to the active states with long type                                                                              |
| ChangeState(Enum activeState)                  | activeState: The state you want to active                                                                 | Change the state to the active state                                                                                               |
| Update (float deltaTime)                       | deltaTime: The Time.deltaTime for the game environment                                                    | Update is called every frame, it should connect with MonoBehaviour.Update() and sync Time.deltaTime for it.                        |
| FixedUpdate(float deltaTime)                   | deltaTime: The Time.fixedDeltaTime for the game environment                                               | FixedUpdate is called every physics frame, it should connect with MonoBehaviour.FixedUpdate() and sync Time.fixedDeltaTime for it. |
| LateUpdate(float deltaTime)                    | deltaTime: The Time.deltaTime for the game environment                                                    | LateUpdate is called every frame, it should connect with MonoBehaviour.LateUpdate() and sync Time.deltaTime for it.                |

#### Examples
```
using TEDCore.StateManagement;  

public class NewTask : Task  
{  
    public NewTask() : base()  
    {  

    }  

    public override void Show (bool show)  
    {  

    }  

    public override void Update (float deltaTime)  
    {  

    }  

    public override void FixedUpdate (float deltaTime)  
    {  

    }  

    public override void LateUpdate (float deltaTime)  
    {  

    }  

    public override void Destroy()  
    {  
        base.Destroy();  
    }  
}  
```
```
using TEDCore.StateManagement;

public class NewState : State
{
    private enum ENewState
    {
        New,
    }

    public NewState(StateManager stateManager) : base(stateManager)
    {
        TaskManager.AddTask(new NewTask(), ENewState.New);
        TaskManager.ChangeState(ENewState.New);
    }
}
```

### Event Manager
Event Manager is designed to a central event receiver and transmitter.
The concept is based on Observer pattern.
The main purpose of Event Manager is to send events to other parts of the script without being dependant on their interface or having the explicit of it.
It can help us to prevent involving in the dependency hell.
It consists of EventResult, EventListener and EventManager.

#### Namespace
TEDCore.Event

### AssetBundle Manager
An AssetBundle is an archive file containing platform specific Assets(Model, Textures, Prefabs, Audio clips, and even entire Scenes) that can be loaded at runtime.
AssetBundle Manager is designed to access AssetBundle and you don’t need to use AssetBundle Manager because Resource Manager would handle everything for you directly.

#### Namespace
TEDCore.AssetBundle

### Resource Manager
Resource Manager is designed to get the asset from Resources folder or AssetBundle automatically.
You can simply load or load asynchronously the asset to memory through Resource Manager.

#### Namespace
TEDCore.Resource

### Pool Manager
Pool Manager is based on Object Pool.
It can help the developer create object pool repeatedly.
Currently, it only support to create a GameObject object pool and the asset need to be in the Resources folder.
It consists of Pool and PoolManager.

#### Namespace
TEDCore.Pool

### Timer Manager
Timer Manager is design for scheduling.
It help the developer create a sequence schedule with easy steps.
It consists of BaseTimer and TimerManager.

#### Namespace
TEDCore.Timer

### Audio Manager
Audio Manager is design for playing BGM and SFX.
The developer could play BGM and SFX while using it and adjust the volume of BGM and SFX on it.
It also handle the memory load and unload for developers automatically. But it only support to load the audio clip synchronously from Resources folder currently.

#### Namespace
TECore.Audio

### UI Manager
UI Manager is designed to the UI prefab in Unity.
It could help the developers load, show, hide and destroy the UI prefabs.

#### Namespace
TEDCore.UI

### HttpRequest Manager
HttpRequest Manager is designed to implement the RESTful API.

#### Namespace
TEDCore.Http

## Tools
### TEDDebug DLL
TEDDebug.dll repackage UnityEngine.Debug so that the developer can turn on/off the log with a simply parameter EnableLog.

#### Namespace
TEDCore

#### Assembly Info
Target .NET 3.5

#### Source Script
```
using UnityEngine;

namespace TEDCore  
{  
    public class TEDDebug  
    {  
        public static bool EnableLog = true;  

        public static void Log(object message)  
        {     
            if (!EnableLog)  
            {  
                return;
            }  

            Debug.Log(message);  
        }  


        public static void Log(object message, Object context)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.Log(message, context);  
        }  


        public static void LogFormat(string format, params object[] args)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogFormat(format, args);  
        }  


        public static void LogFormat(Object context, string format, params object[] args)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogFormat(context, format, args);  
        }  


        public static void LogWarning(object message)  
        {     
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogWarning(message);  
        }  


        public static void LogWarning(object message, Object context)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogWarning(message, context);  
        }  


        public static void LogWarningFormat(string format, params object[] args)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogWarningFormat(format, args);  
        }  


        public static void LogWarningFormat(Object context, string format, params object[] args)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogWarningFormat(context, format, args);  
        }  


        public static void LogError(object message)  
        {     
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogError(message);  
        }  


        public static void LogError(object message, Object context)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogError(message, context);  
        }  


        public static void LogErrorFormat(string format, params object[] args)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogErrorFormat(format, args);  
        }  


        public static void LogErrorFormat(Object context, string format, params object[] args)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogErrorFormat(context, format, args);  
        }  


        public static void LogException(System.Exception exception)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogException(exception);  
        }  


        public static void LogException(System.Exception exception, Object context)  
        {  
            if (!EnableLog)  
            {  
                return;  
            }  

            Debug.LogException(exception, context);  
        }  
    }  
}
```

#### Examples
```
using UnityEngine
using TEDCore;  

public class ExampleClass : MonoBehaviour  
{  
    private void Awake()  
    {  
        TEDDebug.Log("Test TEDDebug.Log");  
        TEDDebug.LogFormat("Test TEDDebug.LogFormat {0}", "finished");  

        TEDDebug.LogWarning("Test TEDDebug.LogWarning");  
        TEDDebug.LogWarningFormat("Test TEDDebug.LogWarningFormat {0}", "finished");  

        TEDDebug.LogError("Test TEDDebug.LogError");  
        TEDDebug.LogErrorFormat("Test TEDDebug.LogErrorFormat {0}", "finished");  
    }  
}
```

### AssetBundle Tool
AssetBundle Tool would handle everything for you to build AssetBundle.
What you need to do is to drag the asset which would like to be an AssetBundle asset in AssetBundleResources folder and then click the Build AssetBundles menu item.
The AssetBundle would be built to StreamingAssets folder in currernt version.

### Build Tool
Build Tool can help the developers to build the package file with the auto file name.
It just support you to build Android apk in the current version.

### DefineSymbol Tool
The main purpose of DefineSymbol Tool is to make sure all developers are on the same page of the scripting define symbol in the project they are working on.

It would create a file in DefineSymbol/DefineSymbolSettings.asset when the developers save the define symbol at the first time.
And the DefineSymbolSettings.asset should be committed to Git, then other developers could get the same DefineSymbolSettings.asset and they should be on the same page of the scripting define symbol with its description.

### TemplateScript Tool
TemplateScript Tool could help the developers create the template script.
It supports you to create State, Task, View and NetworkMessage template script in the current version.

### ClientDatabase Tool
ClientDatabase Tool is designed to load the content from Google Sheets and generate the script automatically.
The designers could modify the content in Google Sheets and then export the content with csv format.
Then, the engineers could generate the referenced scripts by single click.

Support Data Type
bool
float
int
string
bool[]
float[]
int[]
string[]
