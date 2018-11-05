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

### State
State and Task modules are the most important parts in this library.
The major concept is based on **State design pattern**.
Each State is a single hierarchy and processing Task that is registered inside it.
State module consists two parts, **State**, and **StateManager**.
State just likes the scenes in Unity Engine and have the higher control level than Unity scene.

#### Namespace
TEDCore.StateManagement

#### Public Methods - StateManager
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

### Task
Task module need to cooperate with with **State Manager**.
In each State, you can register multiple Task inside it and each Task works as a single station of the game logic which means we need to follow **Single Responsibility Principle** in each specific Task.
Then change the task state in TaskManager can process the specific Task.
Task Manager consists two parts, **Task**, and **TaskManager**.

#### Namespace
TEDCore.StateManagement

#### Public Methods - TaskManager
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

### Event
Event is designed to a central event receiver and transmitter.
The concept is based on **Observer design pattern**.
The main purpose of Event Manager is to send events to other parts of the script without being dependant on their interface or having the explicit of it.
It can help us to prevent involving in the dependency hell.
It consists parts, **EventResult**, **EventListener**, and **EventManager**.

#### Namespace
TEDCore.Event

#### Public Methods - EventManager
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

### AssetBundle
An AssetBundle is an archive file containing platform specific Assets(Model, Textures, Prefabs, Audio clips, and even entire Scenes) that can be loaded at runtime.
The module is designed to access AssetBundle and **you don’t need to use AssetBundle Manager because Resource Manager would handle everything for you directly**.

#### Namespace
TEDCore.AssetBundle

#### Public Methods - AssetBundleManager
| Name                                                                               | Parameters                                                                                          | Description                                                              |
|------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------|
| Initialize(AssetBundleInitializeData initializeData)                               | initializaData: The initialize data for AssetBundleManager                                          | Initialize AssetBundleManager with the data.                             |
| Download(Action&lt;AssetBundleDownloadProgress&gt; onAssetBundleDownloadProgressChanged) | onAssetBundleDownloadProgressChanged: The callback method for AssetBundle download progress changed | Start downloading all AssetBundle files which don't cache in the device. |

#### AssetBundleInitializeData
| Type                | Parameters         | Description                                           |
|---------------------|--------------------|-------------------------------------------------------|
| int                 | MaxDownloadRequest | The maximum amount of the download request.           |
| string              | DownloadURL        | The URL path for downloading AssetBundles             |
| AssetBundleLoadType | LoadType           | The load type, Simulate, StreamingAssets, and Network |
| Action&lt;bool&gt;        | onInitializeFinish | The initialize callback method                        |

### Resource
The module is designed to get the asset from Resources folder or AssetBundle automatically.
You can simply load or load asynchronously the asset to memory through Resource Manager.

#### Namespace
TEDCore.Resource

#### Public Methods - ResourceManager
| Name                                                                                                                 | Parameters                                                                                                                                                                                                                                                                       | Description                      |
|----------------------------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|----------------------------------|
| LoadAsync&lt;T&gt;(string assetName, Action&lt;T&gt; callback, bool unloadAutomatically = true)                                  | T: The load type of the asset<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not                                                           | Asynchronously loads asset.      |
| LoadAsync&lt;T&gt;(string assetBundleName, string assetName, Action&lt;T&gt; callback, bool unloadAutomatically = true)          | T: The load type of the asset<br>assetBundleName: The AssetBundle name you want to load<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not | Asynchronously loads asset.      |
| LoadAllAsync&lt;T&gt;(string assetName, Action&lt;List&lt;T&gt;&gt; callback, bool unloadAutomatically = true)                         | T: The load type of the asset<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not                                                           | Asynchronously loads all assets. |
| LoadAllAsync&lt;T&gt;(string assetBundleName, string assetName, Action&lt;List&lt;T&gt;&gt; callback, bool unloadAutomatically = true) | T: The load type of the asset<br>assetBundleName: The AssetBundle name you want to load<br>assetName: The asset name or path you want to load<br>callback: The callback when the asynchronously loads is completed<br>unloadAutomatically: Unload the asset automatically or not | Asynchronously loads all assets. |
| Unload&lt;T&gt;(string assetName)                                                                                          | T: The load type of the asset<br>assetName: The asset name or path you want to unload                                                                                                                                                                                            | Unload asset                     |
| Unload&lt;T&gt;(string assetBundleName, string assetName)                                                                  | T: The load type of the asset<br>assetBundleName: The AssetBundle name you want to unload<br>assetName: The asset name or path you want to unload                                                                                                                                | Unload asset                     |
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

### ObjectPool
ObjectPool module is in charge of **Object Pools**.
It can help the developers create object pool easily.

#### Namespace
TEDCore.ObjectPool

#### Public Methods - ObjectPoolManager
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

### Timer
Timer module is design for scheduling the logic.
It help the developer create a sequence schedule with easy steps.
It consists of **BaseTimer** and **TimerManager**.

#### Namespace
TEDCore.Timer

#### Public Methods - TimerManager
| Name                                                                | Parameters                                                                                                                              | Description       |
|---------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------|-------------------|
| Schedule(float duration, Action onTimerFinished)                    | duration: The duration of the timer<br>onTimerFinished: The callback method when the timer was finished                                 | Setup a new timer |
| Schedule&lt;T&gt;(float duration, Action&lt;T&gt; onTimerFinished, T timerData) | duration: The duration of the timer<br>onTimerFinished: The callback method when the timer was finished<br>timerData: The callback data | Setup a new timer |
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

### BGM
BGM module is designed for playgin BGM.
The developer could play the BGM and adjust the volumn of it.

#### Namespace
TEDCore.Audio

#### Public Methods - BGMManager
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

### SFX
SFX module is designed for playgin SFX.
The developer could play the SFX and adjust the volumn of it.
It handle the object pool for it, so would recycle the sfx object once the audio clip was finished.

#### Namespace
TEDCore.Audio

#### Public Methods - SFXManager
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

### UI
UI module is designed to the UI prefab in Unity.
It could help the developers load, show, hide and destroy the UI prefabs.

#### Namespace
TEDCore.UI

#### Public Methods - UIManager
| Name                                                                    | Parameters                                                                                                                                                            | Description                       |
|-------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------|
| LoadUIAsync&lt;T&gt;(string assetName, Action&lt;T&gt; callback)                    | assetName: The asset name you want to load with an UI<br>callback: The callback method when the asset was loaded                                                      | Asynchronously load the UI prefab |
| LoadUIAsync&lt;T&gt;(string bundleName, string assetName, Action&lt;T&gt; callback) | bundleName: The AssetBundle name you want to load<br>assetName: The asset name you want to load with an UI<br>callback: The callback method when the asset was loaded | Asynchronously load the UI prefab |
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

### HttpRequest
HttpRequest module is designed to implement the RESTful API.

#### Namespace
TEDCore.Http

#### Public Methods - HttpRequestManager
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

### Notification
Notification module is designed to implement **Local Notifications on the iOS platform**.

#### Namespace
TEDCore.Notification

#### Public Methods - NotificationManager
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

### CoroutineChain
CoroutineChain module would help developers to handle Unity Coroutine with dependencies.
It utilize the method chain for allowing the coroutines to be chained together in a single statement without requiring variables to store the intermediate results.

#### Namespace
TEDCore.Coroutine

#### Public Methods - CoroutineChainManager
| Name                                   | Parameters                                       | Description                                          |
|----------------------------------------|--------------------------------------------------|------------------------------------------------------|
| Create()                      |                                                  | Create a new empty coroutine chain.              |
| Create(IEnumerator coroutine) | coroutine: The coroutine you want to add default | Create a new coroutine chain with the coroutine. |

#### Public Methods - CoroutineChain
| Name                                 | Parameters                                                          | Description                                                 |
|--------------------------------------|---------------------------------------------------------------------|-------------------------------------------------------------|
| Enqueue(IEnumerator coroutine)       | coroutine: The coroutine you want to add                            | Add a coroutine to the coroutine chain.                     |
| Enqueue(Action action)               | action: The action you want to add                                  | Add a action callback to the coroutine chain.               |
| Enqueue&lt;T&gt;(Action&lt;T&gt; action, T data) | action: The action you want to add<br>data: The data for the action | Add a action callback to the coroutine chain with the data. |
| StartCoroutine()                     |                                                                     | Start the coroutine chain.                                  |
| StopCoroutine()                      |                                                                     | Stop the coroutine chain.                                   |

#### Public Methods - CoroutineUtils
| Name                                | Parameters                                       | Description                                          |
|-------------------------------------|--------------------------------------------------|------------------------------------------------------|
| WaitForSeconds(float seconds)       | seconds: The seconds you want to wait            | Create a new WaitForSeconds coroutine.               |
| WaitForEndOfFrame()                 |                                                  | Create a new WaitForEndOfFrame coroutine.            |
| WaitUntil(Func&lt;bool&gt; predicate)     | predicate: The predicate you want to wait        | Create a new WaitUtil coroutine.                     |
| WaitWhile(Func&lt;bool&gt; predicate)     | predicate: The predicate you want to wait        | Create a new WaitWhile coroutine.                    |

#### Examples
```
using UnityEngine;
using TEDCore;
using TEDCore.Coroutine;

public class ExampleClass : MonoBehaviour
{
    [SerializeField] private bool m_waitUntil1;
    [SerializeField] private bool m_waitUntil2;
    [SerializeField] private bool m_waitWhile;
    
    private CoroutineChain m_coroutineChain;

    private void Start()
    {
        m_coroutineChain = CoroutineChainManager.Instance.Create()
                                                .Enqueue(CoroutineUtils.WaitForSeconds(1.0f))
                                                .Enqueue(WaitForSeconds)
                                                .Enqueue(CoroutineUtils.WaitForEndOfFrame())
                                                .Enqueue(WaitForEndOfFrame)
                                                .Enqueue(CoroutineUtils.WaitUntil(() => m_waitUntil1))
                                                .Enqueue(WaitUntil1)
                                                .Enqueue(CoroutineUtils.WaitUntil(() => m_waitUntil2))
                                                .Enqueue(WaitUntil2, "finish")
                                                .Enqueue(CoroutineUtils.WaitWhile(() => !m_waitWhile))
                                                .Enqueue(WaitWhile, 1)
                                                .Enqueue(Finish)
                                                .StartCoroutine();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            m_coroutineChain.StopCoroutine();
            m_coroutineChain = null;
        }
    }

    private void WaitForSeconds()
    {
        TEDDebug.Log("Call WaitForSeconds()");
    }

    private void WaitForEndOfFrame()
    {
        TEDDebug.Log("Call WaitForEndOfFrame()");
    }

    private void WaitUntil1()
    {
        TEDDebug.Log("Call WaitUntil1()");
    }

    private void WaitUntil2(string value)
    {
        TEDDebug.LogFormat("Call WaitUntil2() with string '{0}'", value);
    }

    private void WaitWhile(int value)
    {
        TEDDebug.LogFormat("Call WaitWhile() with int '{0}'", value);
    }

    private void Finish()
    {
        TEDDebug.Log("Call Finish()");
    }
}
```

### Cipher
Cipher module is in charge of encryption and decryption.
It can help developers to encypt and decrypt the text.

#### Namespace
TEDCore.Cipher

#### Public Methods - CipherManager
| Name                       | Parameters                               | Description       |
|----------------------------|------------------------------------------|-------------------|
| Encrypt(string plainText)  | plainText: The text you want to encrypt  | Encrypt the text. |
| Decrypt(string cipherText) | cipherText: The text you want to decrypt | Decrypt the text. |

#### Examples
```
using UnityEngine;
using TEDCore;
using TEDCore.Cipher;

public class ExampleClass : MonoBehaviour
{
    [SerializeField] private string m_plainText = "Example";
    [SerializeField] private string m_cipherText;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            m_cipherText = CipherManager.Instance.Encrypt(m_plainText);
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            TEDDebug.Log(CipherManager.Instance.Decrypt(m_cipherText));
        }
    }
}
```

### PlayerData
PlayerData module could hlep developers to save the player data in the client side.
It would also cipher the file name and the data inside it.

#### Namespace
TEDCore.PlayerData

#### Public Methods - PlayerDataUtils
| Name                                  | Parameters                                                       | Description    |
|---------------------------------------|------------------------------------------------------------------|----------------|
| Save<T>(T data) where T : PlayerData  | T: The class type of the data<br>data: The data you want to save | Save the data. |
| Load<T>() where T : PlayerData, new() | T: The class type of the data                                    | Load the data. |

#### Examples
```
using UnityEngine;
using TEDCore;
using TEDCore.PlayerData;

public class ExampleClass : MonoBehaviour
{
    public class TestData : PlayerData
    {
        public override string FileName
        {
            get
            {
                return "TestData.dat";
            }
        }

        public int id;
        public string name;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            TestData testData = new TestData();
            testData.id = 1;
            testData.name = "ExampleName";

            PlayerDataUtils.Save(testData);
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            TestData testData = PlayerDataUtils.Load<TestData>();

            TEDDebug.Log("id = " + testData.id);
            TEDDebug.Log("name = " + testData.name);
        }
    }
}
```

### Log
Log module would be in charge of recording the logs during the project.
It could help the developers to debug the game with log error and exception.
The log file would be generated at **ProjectPath/LogData/**.

#### Namespace
TEDCore.Log

#### Public Methods - LogManager
| Name             | Parameters | Description                                  |
|------------------|------------|----------------------------------------------|
| StartRecording() |            | Start recording the logs during the project. |

#### Examples
```
using UnityEngine;
using TEDCore;
using TEDCore.Log;

public class ExampleClass : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            LogManager.Instance.StartRecording();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            TEDDebug.Log("Log");
            TEDDebug.LogWarning("LogWarning");
            TEDDebug.LogError("LogError");
            TEDDebug.LogException(new System.Exception("LogException"));
        }
    }
}
```

### Network
#### Namespace
#### Public Methods
#### Examples

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

#### Menu Items
| Item                                          | Description                       |
|-----------------------------------------------|-----------------------------------|
| TEDCore/AssetBundles/AssetBundle Build Window | Open the AssetBundle build window |
<br>
<img src="https://github.com/ted10401/TEDCore/blob/master/Modules/AssetBundle/Editor/GithubResources/AssetBundleTool_Window.png" width="400" />

### Build Tool
Build Tool can help the developers to build the project with the file name automatically.
Currently, it support to build StanlaloneOSX and Android version.

#### Menu Items
| Item                                      | Description                               |
|-------------------------------------------|-------------------------------------------|
| TEDCore/Build/Build StandaloneOSX Develop | Build StandaloneOSX with development mode |
| TEDCore/Build/Build StandaloneOSX Release | Build StandaloneOSX with release mode     |
| TEDCore/Build/Build Android Develop       | Build Android with development mode       |
| TEDCore/Build/Build Android Release       | Build Android with release mode           |

### DefineSymbol Tool
The main purpose of DefineSymbol Tool is to make sure all developers are on the same page of the scripting define symbol in the project they are working on.

It would create a file in DefineSymbol/DefineSymbolSettings.asset when the developers save the define symbol at the first time.
And the DefineSymbolSettings.asset should be committed to Git, then other developers could get the same DefineSymbolSettings.asset and they should be on the same page of the scripting define symbol with its description.

### TemplateScript Tool
TemplateScript Tool could help the developers create the template script.
It supports you to create State, Task, View and NetworkMessage template script in the current version.

### ClientDatabase Tool
ClientDatabase Tool is designed to load the content from **Google Sheets** and generate the script automatically.
The designers could modify the content in **Google Sheets** and then export the content with **csv** format.
Then, the engineers could generate the referenced scripts by single click.

#### Support Data Type
* bool
* float
* int
* string
* bool[]
* float[]
* int[]
* string[]

#### Steps
1. Modify datas in **Google Sheet** **<font color=red>(PS. The first row should be DataType/DataName)</font>**
2. Save the table to .csv format
3. Click **TEDCore/Client Database/Initialize Plugin** to generate the default folders
4. Put .csv files to **Assets/ClientDatabase/CsvResources** folder
5. Click **TEDCore/Client Database/Generate Scripts** to generate the scripts automatically
6. The scripts and scriptable objects would generate to **ClientDatabase/GenerateScripts** and **ClientDatabase/Resources**

#### Public Methods
| Name             | Parameters                           | Description                                              |
|------------------|--------------------------------------|----------------------------------------------------------|
| Initialize()     |                                      | Initialize the manager, load the database from Resources |
| GetDatabase<T>() | T: The Database type you want to get | Get the database of type                                 |

#### Examples
| string/Key | string/Name | int/Hp |
|:----------:|:-----------:|:------:|
|      1     |  Monster_1  |   10   |
|      2     |  Monster_2  |   20   |
|      3     |  Monster_3  |   30   |
|      4     |  Monster_4  |   40   |
|      5     |  Monster_5  |   50   |
|      6     |  Monster_6  |   60   |
|      7     |  Monster_7  |   70   |
|      8     |  Monster_8  |   80   |
|      9     |  Monster_9  |   90   |
|     10     |  Monster_10 |   100  |
```
using UnityEngine;
using TEDCore;
using TEDCore.ClientDatabase;

public class ExampleClass : MonoBehaviour
{
    private void Awake()
    {
        ClientDatabaseManager.Instance.Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MonsterDatabase testDatabase = ClientDatabaseManager.Instance.GetDatabase<MonsterDatabase>();
            MonsterData testData = null;

            for (int i = 1; i <= testDatabase.GetCount(); i++)
            {
                testData = testDatabase.GetData(i);
                TEDDebug.LogFormat("id {0}, name {1}, hp {2}", testData.Key, testData.Name, testData.Hp);
            }
        }
    }
}
```

### Performance Tool
Performance Tool can help developers to handle the fps during the project.
The only thing need to do is add component **PerformanceHUD** to the gameobject inside the scene, then, the HUD would display direclty.
<br>
<img src="https://github.com/ted10401/TEDCore/blob/master/Tools/PerformanceTool/GithubResources/PerformanceTool_HUD.png" width="200" />
