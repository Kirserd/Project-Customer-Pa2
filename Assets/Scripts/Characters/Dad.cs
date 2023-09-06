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

    private void Start() => RefreshSubscriptions();

    public void RefreshSubscriptions()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.Interact] += ctx => Interact(ctx);
    }

    private void FixedUpdate()
    {
        GetClosestInteractable();
    }

    private void GetClosestInteractable()
    {
        SelectionManager selectionManager = SelectionManager.Instance;
        selectionManager.TrySelectClosestFromPoint(_castOrigin + transform.position, _castRadius);

        if (selectionManager.GetSelectedObject() is IInteractable interactable)
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
        if (ClosestInteractable is null || state == ButtonState.Hold)
            return;

        ClosestInteractable.Interact();
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
