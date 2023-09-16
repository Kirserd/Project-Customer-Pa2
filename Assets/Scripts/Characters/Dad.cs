using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Movement))]
public class Dad : MonoBehaviour
{
    [Header("Debug")]
    public bool _Debug;

    [Header("Checks")]
    [SerializeField]
    private float _castRadius;
    [SerializeField]
    private Vector3 _castOrigin;
    private IInteractable _closestInteractable;

    [Header("Gauges")]
    [SerializeField]
    private float _chorePoints;
    [SerializeField]
    private float _childPoints;
    [SerializeField]
    private float _stressPoints;

    private const float MAX_STRESS = 100f;

    public float ChorePoints
    {
        get => _chorePoints;
        set
        {
            _chorePoints = value;
        }
    }
    public float ChildPoints
    {
        get => _childPoints;
        set
        {
            _childPoints = value;
        }
    }
    public float StressPoints
    {
        get => _stressPoints;
        set
        {
            _stressPoints = value;
            if(_stressPoints >= MAX_STRESS)
            {
                _stressPoints = 0;
                
                Shout();
                ChildPoints -= ChildPoints / 3f;
            }
        }
    }

    [HideInInspector]
    public IInteractable ClosestInteractable
    {
        get => _closestInteractable;
        set
        {
            if (value == _closestInteractable)
                return;

            _closestInteractable = value;
        }
    }
    #region StateMachine
    public PlayerStateMachine PlayerStateMachine { get; private set; }
    public MovingState MovingState { get; private set; }
    public PhoneState PhoneState { get; private set; }
    public DialogState DialogState { get; private set; }
    #endregion

    private void Awake()
    {
        PlayerStateMachine = new PlayerStateMachine();

        MovingState = new MovingState(this, gameObject.GetComponent<Movement>());
        PhoneState = new PhoneState(this);
        DialogState = new DialogState(this);
    }
    private void Start() 
    {
        PlayerStateMachine.Initialize(MovingState);
        RefreshSubscriptions(); 
    }

    public void AddPoints(bool isGame, byte amount)
    {
        if (isGame)
            _childPoints += amount;
        else
            _chorePoints += amount;
    }

    public void RefreshSubscriptions()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.Interact] += ctx => Interact(ctx);
    }

    private void FixedUpdate()
    {
        GetClosestInteractable();
    }
    private void Update()
    {
        PlayerStateMachine.UpdateState();
    }
    private void GetClosestInteractable()
    {
        SelectionManager selectionManager = SelectionManager.Instance;
        selectionManager.TrySelectClosestFromPoint(_castOrigin + transform.position, _castRadius);

        if (selectionManager.GetSelectedObject() is IInteractable interactable && interactable.IsActive)
            _closestInteractable = interactable;
        else
            _closestInteractable = null;
    }

    /// <summary>
    /// Interacts with the closest interactable object based on the button state.
    /// </summary>
    /// <param name="state">The button state.</param>
    private void Interact(ButtonState state)
    {
        if (ClosestInteractable is null || state == ButtonState.Hold || PlayerStateMachine.CurrentState != MovingState)
            return;

        ClosestInteractable.Interact();
    }
    private void Shout()
    {

    }

    private void OnDrawGizmos()
    {
        if (_Debug)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_castOrigin + transform.position, _castRadius);
        }
    }
}
