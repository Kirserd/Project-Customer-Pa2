using System.Collections;
using UnityEngine;

/// <summary>
/// Movement class that handles physical object's movement.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _acceleration;
    [SerializeField]
    private float _rotationSpeed;

    private Rigidbody _rigidbody;
    private Animator _animator;
    private Vector3 _direction;

    private float _stepCounter;
    private bool _stepRight;
    private void Start()
    {
        RefreshComponents();
        RefreshSubscriptions();
    }

    /// <summary>
    /// Refreshes the components attached to Movement.
    /// </summary>
    public void RefreshComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Refreshes the event subscriptions for input handling.
    /// </summary>
    public void RefreshSubscriptions()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.ForwardKey] += ctx => _direction = new Vector3(_direction.x, _direction.y, 1);
        InputSubscriber.InputEvents[(int)BoundKeys.BackwardKey] += ctx => _direction = new Vector3(_direction.x, _direction.y, -1);
        InputSubscriber.InputEvents[(int)BoundKeys.LeftKey] += ctx => _direction = new Vector3(-1, _direction.y, _direction.z);
        InputSubscriber.InputEvents[(int)BoundKeys.RightKey] += ctx => _direction = new Vector3(1, _direction.y, _direction.z);
    }

    private void FixedUpdate()
    {
        Animate();

        if (_direction == Vector3.zero)
            return;

        MoveForwards();
        RotateTowardsDirection(_direction);
        _direction = Vector3.zero;
    }

    private void Step()
    {
        _stepCounter += Time.fixedDeltaTime;
        if(_stepCounter >= 0.35f)
        {
            _stepCounter = 0f;
            if(_stepRight)
                AudioManager.Source.PlayOneShot(AudioManager.Clips["FootstepRight"], 1f);
            else
                AudioManager.Source.PlayOneShot(AudioManager.Clips["FootstepLeft"], 1f);

            _stepRight = !_stepRight;
        }
    }

    /// <summary>
    /// Moves the player character forwards based on the current velocity.
    /// </summary>
    private void MoveForwards()
    {
        _rigidbody.velocity += _acceleration * Time.fixedDeltaTime * (Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0) * Vector3.forward);
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _maxSpeed);

        if(_rigidbody.velocity.magnitude >= 0.25f)
            Step();
    }

    /// <summary>
    /// Rotates the player character towards the given direction.
    /// </summary>
    /// <param name="direction">The direction to rotate towards.</param>
    private void RotateTowardsDirection(Vector3 direction) => transform.rotation = Quaternion.RotateTowards(
        transform.rotation, Quaternion.LookRotation(direction, Vector3.up),
        _rotationSpeed / _rigidbody.velocity.magnitude
    );

    /// <summary>
    /// Updates the animator based on the current velocity.
    /// </summary>
    private void Animate()
    {
       // _animator.SetFloat("Speed", _rigidbody.velocity.magnitude);
    }
}
