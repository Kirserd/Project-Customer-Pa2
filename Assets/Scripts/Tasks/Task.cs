using UnityEngine;

public abstract class Task
{
    public static Task Instance;

    public delegate void OnCompletedHandler(bool result);
    public OnCompletedHandler OnCompleted;
    public OnCompletedHandler OnForcefullyStopped;

    public delegate void OnStartedHandler();
    public OnStartedHandler OnStarted;
    
    protected TaskStarter _caller;
    protected GameObject _prefabInstance;

    protected static Dad _dad;
    protected static Transform _root;
    public static Transform TaskGUI;

    protected Task()
    {
        OnStarted += HandleOnStarted;
        OnCompleted += ctx => HandleOnCompleted(ctx);

        InitializeReferences();
    }
    private static void InitializeReferences()
    {
        if (_dad is null)
            _dad = GameObject.FindGameObjectWithTag("Player").GetComponent<Dad>();

        if (_root is null)
            _root = GameObject.FindGameObjectWithTag("SceneObjects").transform;

        if (TaskGUI is null)
            TaskGUI = GameObject.FindGameObjectWithTag("TaskGUI").transform;
    }
    private void HandleOnStarted()
    {
        Instance = this;
        _dad.PlayerStateMachine.UpdateState(new TaskState(_dad, _caller.Data));
        Setup();
    }
    private void HandleOnCompleted(bool result)
    {
        if(_caller.Interval == DayCycle.TimeInterval.All)
        {
            Finalize();
            return;
        }

        if (result)
        {
            _dad.AddPoints(_caller.Data.IsGame, _caller.Data.Points);
            _caller.SetAvailabilityState(TaskStarter.Availability.Done);
        }
        else
            _caller.SetAvailabilityState(TaskStarter.Availability.Late);
        
        Finalize();

        void Finalize()
        {
            _dad.PlayerStateMachine.UpdateState(_dad.MovingState);

            Clear();
        }
    }
    protected void HandleOnStateChanged(TaskStarter.Availability state)
    {
        _caller.OnStateChanged -= HandleOnStateChanged;
        if (state == TaskStarter.Availability.Late)
            ForcefullyStop(false);
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
    public virtual void Stop(TaskStarter caller, bool result) => OnCompleted?.Invoke(result);
    public virtual void ForcefullyStop(bool result)
    {
        OnForcefullyStopped?.Invoke(result);
        Stop(_caller, result);
    }
    public void Reset() => _caller = null;
}