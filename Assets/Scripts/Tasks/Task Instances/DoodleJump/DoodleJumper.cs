using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class DoodleJumper : MonoBehaviour
{
    [Header("World Coordinates Space")]
    [SerializeField]
    private Vector3 _eulerRotation;
    [SerializeField]
    private GameObject _world;
    [SerializeField]
    private float _worldOffset;
    [SerializeField]
    private float _winHeight;
    [SerializeField]
    private float _loseHeight;

    [Header("Movement Parameters")]
    [SerializeField]
    private float _jumpForce = 10f;
    [SerializeField]
    private float _gravityForce = 9.8f;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _maxHorizontalPos = 5f;

    [SerializeField]
    private Renderer _lag;
    private float _lagTimeCounter;

    private Rigidbody _rigidbody;
    private bool _isGrounded = false;

    private bool _active = true;

    private void Start()
    {
        RefreshComponents();
        SubscribeToInput();
    }

    private void LateUpdate()
    {
        _world.transform.localPosition = new Vector3(_world.transform.localPosition.x, -transform.localPosition.y + _worldOffset, _world.transform.localPosition.z);
        transform.GetChild(0).localPosition = new Vector3(transform.localPosition.x * transform.localScale.x, -transform.localPosition.y / transform.localScale.y, transform.localPosition.z * transform.localScale.z);
    }

    private void RefreshComponents()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void SubscribeToInput()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.ForwardKey] += ctx => TryJump(ctx);
        InputSubscriber.InputEvents[(int)BoundKeys.LeftKey] += ctx => MoveHorizontally(false);
        InputSubscriber.InputEvents[(int)BoundKeys.RightKey] += ctx => MoveHorizontally(true);
    }

    private void MoveHorizontally(bool isRight)
    {
        if (!_active)
            return;

        float horizontal = isRight ? 1 : -1;
        transform.position = transform.position + Quaternion.Euler(_eulerRotation) * new Vector3(horizontal * _moveSpeed * Time.deltaTime, 0, 0);
        transform.worldToLocalMatrix.MultiplyPoint(new Vector3(_maxHorizontalPos * 2, 0, 0));

        if (transform.localPosition.x > _maxHorizontalPos) 
            transform.localPosition -= new Vector3(_maxHorizontalPos * 2, 0, 0);
        if (transform.localPosition.x < -_maxHorizontalPos)
            transform.localPosition += new Vector3(_maxHorizontalPos * 2, 0, 0);
    }

    private void TryJump(ButtonState state)
    {
        if (!_active)
            return;

        if (_isGrounded && state == ButtonState.Press)
        {
            Vector3 localJumpForce = Vector3.up * _jumpForce;
            Vector3 worldJumpForce = transform.TransformDirection(localJumpForce);
            _rigidbody.AddForce(worldJumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void TryApplyGravitation()
    {
        if (_isGrounded || !_active) 
            return;

        Vector3 localJumpForce = Vector3.down * _jumpForce;
        Vector3 worldJumpForce = transform.TransformDirection(localJumpForce);
        _rigidbody.AddForce(worldJumpForce, ForceMode.Force);
        _isGrounded = false;
    }

    private void FixedUpdate()
    {
        TryLag();
        TryApplyGravitation();
    }
    private void Update()
    {
        bool isGrounded = CheckGround();
        _isGrounded = isGrounded;

        if (transform.localPosition.y >= _winHeight)
            StartCoroutine(LagBeforeFinish(true));
        else if (transform.localPosition.y <= _loseHeight)
            StartCoroutine(LagBeforeFinish(false));
    }

    private void TryLag()
    {
        _lagTimeCounter++;
        if (_lagTimeCounter % 240 == 0 && Random.Range(0, 10) == 5)
            StartCoroutine(Lag());
    }

    private bool CheckGround()
    {
        float rayLength = 1.0f;

        if (Physics.Raycast(new Ray(transform.position 
            - transform.TransformDirection(new Vector3(transform.localScale.x / 2, 0, 0)), 
            transform.TransformDirection(Vector3.down)), 
            out RaycastHit hitInfoLeft, rayLength))
        {
            if (hitInfoLeft.collider.CompareTag("Platform"))
                return true;
        }
        else if (Physics.Raycast(new Ray(transform.position 
            + transform.TransformDirection(new Vector3(transform.localScale.x/2, 0, 0)), 
            transform.TransformDirection(Vector3.down)), 
            out RaycastHit hitInfoRight, rayLength))
        {
            if (hitInfoRight.collider.CompareTag("Platform"))
                return true;
        }

        return false;
    }

    private IEnumerator LagBeforeFinish(bool result)
    {
        _rigidbody.Sleep();
        _active = false;
        yield return new WaitForSeconds(1f);
        Task.Instance.ForcefullyStop(result);
    }

    private IEnumerator Lag()
    {
        _rigidbody.Sleep();
        _active = false;
        _lag.enabled = true;
        yield return new WaitForSeconds(Random.Range(4, 24) / 10f);
        _lag.enabled = false;
        _active = true;
        _rigidbody.WakeUp();
    }
}
