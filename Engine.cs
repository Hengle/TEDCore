using UnityEngine;
using TEDCore;
using TEDCore.StateManagement;
using TEDCore.Startup;
using TEDCore.Input;
using TEDCore.Utils;

public class Engine : MonoBehaviour
{
	private StateManager _stateManager;
	private StartupTaskManager _startupTaskManager;

	private static Engine _instance;
	public static Engine Instance { get { return _instance; } }

	void Awake()
	{
		#if DEVELOP
		Debugger.EnableLog = true;
		#else
		Debugger.EnableLog = false;
		#endif

		_instance = this;
		Time.timeScale = 1.0f;
	}


	void Start()
	{
		_stateManager = new StateManager();
        _startupTaskManager = new StartupTaskManager();
	}


	void Update()
	{
        if (!_startupTaskManager.IsDone())
        {
            return;
        }

		Services.Get<InputManager> ().Update (Time.deltaTime);

		#if ASSET_BUNDLE
		Services.Get<AssetBundleManager> ().Update (Time.deltaTime);
		#endif

		_stateManager.Update(Time.deltaTime);
	}
}