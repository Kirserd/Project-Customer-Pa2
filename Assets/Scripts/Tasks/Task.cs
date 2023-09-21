using System.Collections;
using UnityEngine;

public abstract class Task
{
    public static Task Instance;

    public delegate void OnCompletedHandler(TaskStarter.Availability state);
    public OnCompletedHandler OnCompleted;
    public OnCompletedHandler OnForcefullyStopped;

    public delegate void OnStartedHandler();
    public OnStartedHandler OnStarted;

    protected TaskStarter _caller;
    protected GameObject _prefabInstance;

    public static Dad Dad;
    protected static Transform _root;
    public static Transform TaskGUI;

    private bool _stressAccumulation;

    protected Task()
    {
        InitializeSubscriptions();
        RefreshReferences();
    }
    private void InitializeSubscriptions()
    {
        OnStarted += HandleOnStarted;
        OnCompleted += HandleOnCompleted;
    }
    public static void RefreshReferences()
    {
        Dad = GameObject.FindGameObjectWithTag("Player").GetComponent<Dad>();
        _root = GameObject.FindGameObjectWithTag("SceneObjects").transform;
        TaskGUI = GameObject.FindGameObjectWithTag("TaskGUI").transform;
    }
    private void HandleOnStarted()
    {
        Instance = this;
        Dad.PlayerStateMachine.UpdateState(new TaskState(Dad, _caller.Data));
        ClearNotifications();

        if (!_caller.Data.IsGame)
        {
            _stressAccumulation = true;
            _caller.StartCoroutine(StressAccumulation());
        }
        Setup();

    }
    private void ClearNotifications()
    {
        Transform notifications = GameObject.FindGameObjectWithTag("Notifications").transform;
        for (int i = 0; i < notifications.childCount; i++)
            Object.Destroy(notifications.GetChild(i).transform);
    }
    private void HandleOnCompleted(TaskStarter.Availability state)
    {
        if (_caller is null)
            return;

        if (_caller.Interval == DayCycle.TimeInterval.All)
        {
            Finalize();
            return;
        }
        _caller.SetAvailabilityState(state);
        Finalize();

        void Finalize()
        {
            _stressAccumulation = false;
            Dad.PlayerStateMachine.UpdateState(Dad.MovingState);

            Clear();
            Reset();
        }
    }
    protected void HandleOnStateChanged(TaskStarter.Availability state)
    {
        if (_caller is null)
            return;

        _caller.OnStateChanged -= HandleOnStateChanged;
        if (state != TaskStarter.Availability.Late)
            return;

        ForcefullyStop(state);
    }
    public virtual void Start(TaskStarter caller)
    {
        if (_caller == caller)
            return;

        _caller = caller;
        _caller.TryHideHint();

        if (_caller.Interval != DayCycle.TimeInterval.All)
            _caller.OnStateChanged += HandleOnStateChanged;

        OnStarted?.Invoke();
    }
    protected virtual void Setup() => _prefabInstance = _caller.InstantiatePrefab(_root);
    protected virtual void Clear()
    {
        Object.Destroy(_prefabInstance);
        for (int i = 0; i < TaskGUI.childCount; i++)
            Object.Destroy(TaskGUI.GetChild(i).gameObject);
    }
    public virtual void Stop(TaskStarter caller, TaskStarter.Availability state) => OnCompleted?.Invoke(state);
    public virtual void ForcefullyStop(TaskStarter.Availability state)
    {
        OnForcefullyStopped?.Invoke(state);
        Stop(_caller, state);
    }
    public virtual void ForcefullyStop(TaskStarter.Availability state, ButtonState buttonState)
    {
        if (buttonState == ButtonState.Hold)
            return;

        ForcefullyStop(state);
    }
    public virtual void Reset()
    {
        OnStarted = null;
        OnCompleted = null;
        OnForcefullyStopped = null;
        _caller = null;

        AudioManager.Source.clip = null;
        AudioManager.Source.volume = 1f;
        AudioManager.Source.loop = false;
        AudioManager.Source.Pause();

        InitializeSubscriptions();
    }

    private IEnumerator StressAccumulation()
    {
        while (_stressAccumulation)
        {
            PointManager.StressPoints += 1f;
            yield return new WaitForSeconds(0.5f);
        }
    }
}