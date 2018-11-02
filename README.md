# TEDCore

TEDCore is a Unity framework.
The major purpose is to provide modules and tools for game developing in convenient ways.

## Modules

### Singleton
Singleton is based on **Singleton design pattern**.
There are two class of it, Singleton and MonoSingleton.

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
The major concept is based on **State design pattern**.
Each State is a single hierarchy and processing Task that is registered inside it.
State Manager consists two parts, **State**, and **StateManager**.
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
Task Manager need to cooperate with with **State Manager**.
In each State, you can register multiple Task inside it and each Task works as a single station of the game logic which means we need to follow **Single Responsibility Principle** in each specific Task.
Then change the task state in TaskManager can process the specific Task.
Task Manager consists two parts, **Task**, and **TaskManager**.

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
The concept is based on **Observer design pattern**.
The main purpose of Event Manager is to send events to other parts of the script without being dependant on their interface or having the explicit of it.
It can help us to prevent involving in the dependency hell.
It consists parts, **EventResult**, **EventListener**, and **EventManager**.

#### Namespace
TEDCore.Event

#### Public Methods
| Name                                                                     | Parameters                                                                                                                           | Description                                               |
|--------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------|
| RegisterListener(int eventId, IEventListener listener, int priority = 0) | eventId: The event id you want to register<br>listener: The listener you want to register<br> priority: The priority of the listener | Register the listener on the event id                     |
| RemoveListener(int eventId, IEventListener listener)                     | eventId: The event id you want to remove<br>listener: The listener you want to remove                                                | Remove the listener on the event id                       |
| SendEvent(int eventId, object eventData = null)                          | eventId: The event id you want to send<br>eventData: The event data you want to send                                                 | Send the event data to the event listener on the event id |

#### Examples
```
using UnityEngine;
using TEDCore;
using TEDCore.Event;

public class ExampleClass : MonoBehaviour
{
    private enum EExampleEvent
    {
        ExampleEvent1,
        ExampleEvent2,
        ExampleEvent3,
    }

    private EventListener m_eventListener;

    private void Awake()
    {
        m_eventListener = new EventListener();
        m_eventListener.ListenForEvent(EExampleEvent.ExampleEvent1.GetHashCode(), OnExampleEvent1);
        m_eventListener.ListenForEvent(EExampleEvent.ExampleEvent2.GetHashCode(), OnExampleEvent2);
        m_eventListener.ListenForEvent(EExampleEvent.ExampleEvent3.GetHashCode(), OnExampleEvent3);
    }

    private void Start()
    {
        EventManager.Instance.SendEvent(EExampleEvent.ExampleEvent1.GetHashCode());
        EventManager.Instance.SendEvent(EExampleEvent.ExampleEvent2.GetHashCode(), 0);
        EventManager.Instance.SendEvent(EExampleEvent.ExampleEvent3.GetHashCode(), "example");
    }

    private EventResult OnExampleEvent1(object eventData)
    {
        TEDDebug.Log("Received ExampleEvent1 event.");
        return null;
    }

    private EventResult OnExampleEvent2(object eventData)
    {
        TEDDebug.LogFormat("Received ExampleEvent2 event with int '{0}'.", (int)eventData);
        return null;
    }

    private EventResult OnExampleEvent3(object eventData)
    {
        TEDDebug.LogFormat("Received ExampleEvent3 event with string '{0}'.", (string)eventData);
        return null;
    }
}
```

### AssetBundle Manager
An AssetBundle is an archive file containing platform specific Assets(Model, Textures, Prefabs, Audio clips, and even entire Scenes) that can be loaded at runtime.
AssetBundle Manager is designed to access AssetBundle and **you don’t need to use AssetBundle Manager because Resource Manager would handle everything for you directly**.

#### Namespace
TEDCore.AssetBundle

#### Public Methods
| Name                                                                               | Parameters                                                                                          | Description                                                              |
|------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------|
| Initialize(AssetBundleInitializeData initializeData)                               | initializaData: The initialize data for AssetBundleManager                                          | Initialize AssetBundleManager with the data.                             |
| Download(Action<AssetBundleDownloadProgress> onAssetBundleDownloadProgressChanged) | onAssetBundleDownloadProgressChanged: The callback method for AssetBundle download progress changed | Start downloading all AssetBundle files which don't cache in the device. |

#### AssetBundleInitializeData
| Type                | Parameters         | Description                                           |
|---------------------|--------------------|-------------------------------------------------------|
| int                 | MaxDownloadRequest | The maximum amount of the download request.           |
| string              | DownloadURL        | The URL path for downloading AssetBundles             |
| AssetBundleLoadType | LoadType           | The load type, Simulate, StreamingAssets, and Network |
| Action<bool>        | onInitializeFinish | The initialize callback method                        |

### Resource Manager
Resource Manager is designed to get the asset from Resources folder or AssetBundle automatically.
You can simply load or load asynchronously the asset to memory through Resource Manager.

#### Namespace
TEDCore.Resource

#### Public Methods
| Name                                                                                                                 | Parameters                                                                                                                                                                                                                                                                       | Description                      |
|----------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------|
| LoadAsync<T>(string assetName, Action<T> callback, bool unloadAutomatically = true)                                  | T: The load type of the asset<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not                                                           | Asynchronously loads asset.      |
| LoadAsync<T>(string assetBundleName, string assetName, Action<T> callback, bool unloadAutomatically = true)          | T: The load type of the asset<br>assetBundleName: The AssetBundle name you want to load<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not | Asynchronously loads asset.      |
| LoadAllAsync<T>(string assetName, Action<List<T>> callback, bool unloadAutomatically = true)                         | T: The load type of the asset<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not                                                           | Asynchronously loads all assets. |
| LoadAllAsync<T>(string assetBundleName, string assetName, Action<List<T>> callback, bool unloadAutomatically = true) | T: The load type of the asset<br>assetBundleName: The AssetBundle name you want to load<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not | Asynchronously loads all assets. |
| Unload<T>(string assetName)                                                                                          | T: The load type of the asset<br>assetName: The asset name or path you want to unload                                                                                                                                                                                            | Unload asset                     |
| Unload<T>(string assetBundleName, string assetName)                                                                  | T: The load type of the asset<br>assetBundleName: The AssetBundle name you want to unload<br>assetName: The asset name or path you want to unload                                                                                                                                | Unload asset                     |
| Release()                                                                                                            |                                                                                                                                                                                                                                                                                  | Release memory.                  |

#### Examples
```
using UnityEngine;
using TEDCore.Resource;

public class ExampleClass : MonoBehaviour
{
    private void Start()
    {
        ResourceManager.Instance.LoadAsync<GameObject>("EmptyAsset", OnLoadResourceAssetComplete);
        ResourceManager.Instance.LoadAsync<GameObject>("assetbundle", "EmptyAsset", OnLoadResourceAssetComplete);
    }

    private void OnLoadResourceAssetComplete(GameObject cache)
    {
        GameObject.Instantiate(cache);
    }

    private void OnLoadAssetBundleAssetComplete(GameObject cache)
    {
        GameObject.Instantiate(cache);
    }  
}
```

### ObjectPool Manager
ObjectPool Manager is in charge of **Object Pools**.
It can help the developers create object pool easily.

#### Namespace
TEDCore.ObjectPool

#### Public Methods
| Name                                                                 | Parameters                                                                                                        | Description                                          |
|----------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------|------------------------------------------------------|
| RegisterPool(string key, GameObject referenceAsset, int initialSize) | key: The key for the pool<br>referenceAsset: The asset for the pool<br>initialSize: The initial size for the pool | Create a new object pool for the asset with the key. |
| Get(string key)                                                      | key: The key for the pool                                                                                         | Get the asset from the object pool with the key.     |
| Recycle(string key, GameObject instance)                             | key: The key for the pool<br>instance: The asset you want to recycle                                              | Recycle the asset to the object pool with the key.   |
| Clear()                                                              |                                                                                                                   | Remove all existing object pools.                    |

#### Examples
```
using UnityEngine;
using System.Collections.Generic;
using TEDCore.ObjectPool;

public class ExampleClass : MonoBehaviour
{
    [SerializeField] private GameObject m_emptyAsset;
    private Queue<GameObject> m_emptyAssets = new Queue<GameObject>();

    private void Start()
    {
        ObjectPoolManager.Instance.AddPool("EmptyAsset", m_emptyAsset, 5);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_emptyAssets.Enqueue(ObjectPoolManager.Instance.Get("EmptyAsset"));
        }

        if (Input.GetMouseButtonDown(1) && m_emptyAssets.Count > 0)
        {
            ObjectPoolManager.Instance.Recycle("EmptyAsset", m_emptyAssets.Dequeue());
        }
    }
}
```

### Timer Manager
Timer Manager is design for scheduling.
It help the developer create a sequence schedule with easy steps.
It consists of **BaseTimer** and **TimerManager**.

#### Namespace
TEDCore.Timer

#### Public Methods
| Name                                                                | Parameters                                                                                                                              | Description       |
|---------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|-------------------|
| Schedule(float duration, Action onTimerFinished)                    | duration: The duration of the timer<br>onTimerFinished: The callback method when the timer was finished                                 | Setup a new timer |
| Schedule<T>(float duration, Action<T> onTimerFinished, T timerData) | duration: The duration of the timer<br>onTimerFinished: The callback method when the timer was finished<br>timerData: The callback data | Setup a new timer |
| Add(BaseTimer timer)                                                | timer: The timer you want to add                                                                                                        | Add a new timer.  |
| Remove(BaseTimer timer)                                             | timer: The timer you want to remove                                                                                                     | Remove the timer. |

#### Examples
```
using UnityEngine;
using TEDCore;
using TEDCore.Timer;

public class ExampleClass : MonoBehaviour
{
    private void Start()
    {
        TimerManager.Instance.Schedule(1.0f, OnOneSecondTimerFinished);

        TimerManager.Instance.Schedule(2.0f, OnTwoSecondsTimerFinished, "Two Seconds");

        NormalTimer normalTimer = new NormalTimer(3.0f, OnThreeSecondsTimerFinished);
        TimerManager.Instance.Add(normalTimer);

        NormalTimer<string> stringTimer = new NormalTimer<string>(4.0f, OnFourSecondsTimerFinished, "Four Seconds");
        TimerManager.Instance.Add(stringTimer);
    }

    private void OnOneSecondTimerFinished()
    {
        TEDDebug.Log("OnOneSecondTimerFinished");
    }

    private void OnTwoSecondsTimerFinished(string timerData)
    {
        TEDDebug.Log("OnTwoSecondsTimerFinished = " + timerData);
    }

    private void OnThreeSecondsTimerFinished()
    {
        TEDDebug.Log("OnThreeSecondsTimerFinished");
    }

    private void OnFourSecondsTimerFinished(string timerData)
    {
        TEDDebug.Log("OnFourSecondsTimerFinished = " + timerData);
    }
}
```

### BGM Manager
BGM Manager is designed for playgin BGM.
The developer could play the BGM and adjust the volumn of it.

#### Namespace
TEDCore.Audio

#### Public Methods
| Name                                      | Parameters                                                                                      | Description                                            |
|-------------------------------------------|-------------------------------------------------------------------------------------------------|--------------------------------------------------------|
| SetVolume(float volume)                   | volumn: The volumn value                                                                       | Set the BGM volumn direclty.                           |
| SetVolume(float volume, float duration)   | volumn: The volumn value<br>duration: The duration to the target volumn                        | Set the BGM volumn with the fading duration.           |
| Play(string assetName)                    | assetName: The asset name you want to play                                                      | Play the BGM with the asset name.                      |
| Play(string bundleName, string assetName) | bundleName: The AssetBundle name you want to play<br>assetName: The asset name you want to play | Play the BGM with the AssetBundle name and asset name. |
| Stop()                                    |                                                                                                 | Stop playing the BGM.                                  |

#### Examples
```
using UnityEngine;
using TEDCore.Audio;

public class ExampleClass : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            BGMManager.Instance.Play("BGM1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BGMManager.Instance.Play("assetbundle", "BGM2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BGMManager.Instance.SetVolume(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BGMManager.Instance.SetVolume(0, 2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            BGMManager.Instance.Stop();
        }
    }
}
```

### SFX Manager
SFX Manager is designed for playgin SFX.
The developer could play the SFX and adjust the volumn of it.
It handle the object pool for it, so would recycle the sfx object once the audio clip was finished.

#### Namespace
TEDCore.Audio

#### Public Methods
| Name                                      | Parameters                                                                                      | Description                                            |
|-------------------------------------------|-------------------------------------------------------------------------------------------------|--------------------------------------------------------|
| SetVolume(float volume)                   | volumn: The volumn value                                                                        | Set the SFX volumn direclty.                           |
| Play(string assetName)                    | assetName: The asset name you want to play                                                      | Play the SFX with the asset name.                      |
| Play(string bundleName, string assetName) | bundleName: The AssetBundle name you want to play<br>assetName: The asset name you want to play | Play the SFX with the AssetBundle name and asset name. |

#### Examples
```
using UnityEngine;
using TEDCore.Audio;

public class ExampleClass : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SFXManager.Instance.Play("SFX1");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SFXManager.Instance.Play("assetbundle", "SFX2");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SFXManager.Instance.SetVolume(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SFXManager.Instance.SetVolume(0);
        }
    }
}

```

### UI Manager
UI Manager is designed to the UI prefab in Unity.
It could help the developers load, show, hide and destroy the UI prefabs.

#### Namespace
TEDCore.UI

#### Public Methods
| Name                                                                    | Parameters                                                                                                                                                            | Description                       |
|-------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------|
| LoadUIAsync<T>(string assetName, Action<T> callback)                    | assetName: The asset name you want to load with an UI<br>callback: The callback method when the asset was loaded                                                      | Asynchronously load the UI prefab |
| LoadUIAsync<T>(string bundleName, string assetName, Action<T> callback) | bundleName: The AssetBundle name you want to load<br>assetName: The asset name you want to load with an UI<br>callback: The callback method when the asset was loaded | Asynchronously load the UI prefab |
| DestroyUI(GameObject ui)                                                | ui: The UI you want to destroy                                                                                                                                        | Destory the UI prefab             |
| SetUIActive(GameObject ui, bool active)                                 | ui: The UI you want to set<br>active: Active or deactive value                                                                                                        | Set the UI active or deactive     |

#### Examples
```
using UnityEngine;

public class EmptyUIView : MonoBehaviour
{

}
```
```
using UnityEngine;
using TEDCore.UI;

public class ExampleClass : MonoBehaviour
{
    private EmptyUIView m_view;

    private void Start()
    {
        UIManager.Instance.LoadUIAsync<EmptyUIView>("EmptyUI", OnEmptyUILoaded);
    }


    private void Update()
    {
        if (null == m_view)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UIManager.Instance.SetUIActive(m_view.gameObject, true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UIManager.Instance.SetUIActive(m_view.gameObject, false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UIManager.Instance.DestroyUI(m_view.gameObject);
        }
    }


    private void OnEmptyUILoaded(EmptyUIView view)
    {
        m_view = view;
    }
}
```

### HttpRequest Manager
HttpRequest Manager is designed to implement the RESTful API.

#### Namespace
TEDCore.Http

#### Public Methods
| Name                                                                                                                      | Parameters                                                                                                                                                                                                                                                     | Description                                                                                                  |
|---------------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------|
| Init(string generalServerUrl, string gameServerUrl, string appId, string apiVersion, string gameVersion)                  | generalServerUrl: The url of the general backend server<br>gameServerUrl: The url of the game backend server<br>apiVersion: The API version<br>gameVersion: The game version                                                                                   | Need to use this method when the game start. It would set up the backend data, API version and game version. |
| Auth(object jsonData, WebRequest.OnDataCallback callback)                                                                 | jsonData: The auth data<br>callback: The callback method when the request finish                                                                                                                                                                               | After Init(), you can use this method to authorize and get the account session.                              |
| SetSession(string session)                                                                                                | session: The account session                                                                                                                                                                                                                                   | Set the session.                                                                                             |
| Post(BackendServerType serverType, string endPoint, object jsonData, WebRequest.OnDataCallback callback, object userData) | serverType: The backend server type<br>endPoint: The API url<br>jsonData: The data you want to post<br>callback: The callback method when the POST request finish<br>userData: The user data you want to pass it on to the callback method                     | After SetSession(), you can use this method to do RESTful POST.                                              |
| Get(BackendServerType serverType, string endPoint, WebRequest.OnDataCallback callback, object userData)                   | serverType: The backend server type<br>endPoint: The API url<br>callback: The callback method when the GET request finish<br>userData: The user data you want to pass it on to the callback method                                                             | After SetSession(), you can use this method to do RESTful GET.                                               |
| Delete(BackendServerType serverType, string endPoint, WebRequest.OnDataCallback callback, object userData)                | serverType: The backend server type<br>endPoint: The API url<br>callback: The callback method when the request finish<br>callback: The callback method when the DELETE request finish<br>userData: The user data you want to pass it on to the callback method | After SetSession(), you can use this method to do RESTful DELETE.                                            |
| GetTexture(string url, WebRequest.OnTextureCallback callback, object userData)                                            | url: The url of the texture you want to get<br>callback: The callback method when the request finish<br>userData: The user data you want to pass it on to the callback method                                                                                  | You can use this method directly to get the texture from the url.                                            |

#### Examples
```
using UnityEngine;  
using UnityEngine.UI;  
using TEDCore.Http;  

public class ExampleClass : MonoBehaviour  
{  
    [SerializeField] private RawImage m_rawImage;  

    private void Awake()  
    {  
        HttpRequestManager.Instance.Init(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);  
        HttpRequestManager.Instance.SetSession("test");  
    }  

    private void Update()  
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            HttpRequestManager.Instance.Get(HttpRequestManager.BackendServerType.Game, "https://www.bitoex.com/api/v1/get_rate");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HttpRequestManager.Instance.Get(HttpRequestManager.BackendServerType.Game, "https://www.binance.com/api/v1/ticker/allPrices");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HttpRequestManager.Instance.GetTexture("https://secure.gravatar.com/avatar/3914e31d1985733a5f56605f19ce2f61", OnTextureLoaded);
        }
    }  

    private void OnTextureLoaded(int id, Texture data, object userData)  
    {  
        m_rawImage.texture = data;  
    }  
}  
```

### Notification Manager
Notification Manager is designed to implement **Local Notifications on the iOS platform**.

#### Namespace
TEDCore.Notification

#### Public Methods
| Name                                      | Parameters                                                                                                     | Description                                                |
|-------------------------------------------|----------------------------------------------------------------------------------------------------------------|------------------------------------------------------------|
| Schedule(int seconds, string description) | seconds: After X seconds, the local notification will show.<br>description: The local notification description | Setup the local notification with seconds and descriptoin. |

#### Examples
```
using UnityEngine;
using TEDCore.Notification;

public class ExampleClass : MonoBehaviour
{
    private void Awake()
    {
        NotificationManager.Instance.Schedule(60, "Display the local notifcation after 60 seconds");
        NotificationManager.Instance.Schedule(120, "Display the local notifcation after 120 seconds");
        NotificationManager.Instance.Schedule(300, "Display the local notifcation after 300 seconds");
    }
}
```

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
