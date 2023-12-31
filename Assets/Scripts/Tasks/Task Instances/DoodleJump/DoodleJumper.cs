using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class DoodleJumper : MonoBehaviour
{
    [Header("GameBuddy Console")]
    [SerializeField]
    private Transform _gameBuddyBorder;
    [SerializeField]
    private Transform _screenMask;

    [Header("Game Rules")]
    [SerializeField]
    private float _winHeight;
    [SerializeField]
    private float _loseHeight;

    [Header("Movement Parameters")]
    [SerializeField]
    private float _jumpForce = 10f;
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _maxHorizontalPos = 5f;

    private Rigidbody2D _rigidbody;
    private bool _isGrounded = false;

    private bool _active = true;

    private Vector3 _initCameraPos;
    private Camera _gameCamera;

    private void Start()
    {
        RefreshComponents();
        SubscribeToInput();
        ChangeMusic(true);
        _initCameraPos = _gameCamera.transform.position;
    }

    private void ChangeMusic(bool state)
    {
        AudioSource source = AudioManager.Source.transform.GetChild(0).GetComponent<AudioSource>();
        source.clip = state? 
            AudioManager.Clips["GB"] :
            AudioManager.Clips["Lobby"];

        source.Play();
    }
    private void RefreshComponents()
    {
        _gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void SubscribeToInput()
    {
        Task.Instance.OnCompleted += ctx => StopGame();
        InputSubscriber.InputEvents[(int)BoundKeys.ForwardKey] += ctx => TryJump(ctx);
        InputSubscriber.InputEvents[(int)BoundKeys.LeftKey] += ctx => MoveHorizontally(false);
        InputSubscriber.InputEvents[(int)BoundKeys.RightKey] += ctx => MoveHorizontally(true);
    }
    private void StopGame()
    {
        Task.Instance.OnCompleted -= ctx => StopGame();

        _gameCamera.transform.position = _initCameraPos;

        if(_rigidbody is not null)
            _rigidbody.Sleep();
        
        _active = false;

        InputSubscriber.InputEvents[(int)BoundKeys.ForwardKey] -= ctx => TryJump(ctx);
        InputSubscriber.InputEvents[(int)BoundKeys.LeftKey] -= ctx => MoveHorizontally(false);
        InputSubscriber.InputEvents[(int)BoundKeys.RightKey] -= ctx => MoveHorizontally(true);
    }

    private void MoveHorizontally(bool isRight)
    {
        if (!_active)
            return;

        float horizontal = isRight ? 1 : -1;
        _rigidbody.velocity = new Vector2(horizontal * _moveSpeed, _rigidbody.velocity.y);

       if (transform.localPosition.x > _maxHorizontalPos) 
            transform.localPosition -= new Vector3(_maxHorizontalPos * 2, 0);
        if (transform.localPosition.x < -_maxHorizontalPos)
            transform.localPosition += new Vector3(_maxHorizontalPos * 2, 0);
    }

    private void TryJump(ButtonState state)
    {
        if (!_active)
            return;

        if (_isGrounded && state == ButtonState.Press)
        {
            AudioManager.Source.PlayOneShot(AudioManager.Clips["GBJump"], 1f);
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }

    private void Update()
    {
        if (!_active)
            return;

        _isGrounded = CheckGround();

        if (transform.localPosition.y >= _winHeight) 
            StartCoroutine(LagBeforeFinish(TaskStarter.Availability.Done));
        else if (transform.localPosition.y <= _loseHeight)
            StartCoroutine(LagBeforeFinish(TaskStarter.Availability.Scheduled));

        UpdateConsole();
    }

    private void UpdateConsole()
    {
        _gameCamera.transform.position = new Vector3(_gameCamera.transform.position.x, transform.position.y, _gameCamera.transform.position.z);
        _gameBuddyBorder.position = new Vector3(_gameBuddyBorder.transform.position.x, transform.position.y, _gameBuddyBorder.transform.position.z);
        _screenMask.position = new Vector3(_screenMask.transform.position.x, transform.position.y, _screenMask.transform.position.z);
    }

    private bool CheckGround()
    {
        float rayLength = 1f;

        RaycastHit2D[] hitsInfoLeft = Physics2D.RaycastAll(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), Vector2.down, rayLength);
        if (hitsInfoLeft.Length > 0)
        {
            foreach (RaycastHit2D hit in hitsInfoLeft)
            {
                if (hit.collider.CompareTag("Platform"))
                    return true;
            }
        }
        RaycastHit2D[] hitsInfoRight = Physics2D.RaycastAll(transform.position + new Vector3(transform.localScale.x / 2, 0, 0), Vector2.down, rayLength);
        if (hitsInfoRight.Length > 0)
        {
            foreach (RaycastHit2D hit in hitsInfoRight)
            {
                if (hit.collider.CompareTag("Platform"))
                    return true;
            }
        }

        return false;
    }

    private IEnumerator LagBeforeFinish(TaskStarter.Availability state)
    {
        _rigidbody.Sleep();
        _active = false;
        ChangeMusic(false);
        AudioManager.Source.PlayOneShot(AudioManager.Clips["GBWin"], 1f);
        yield return new WaitForSeconds(2f);
        Task.Instance.ForcefullyStop(state);
    }
}
