using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraActions : MonoBehaviour
{
    #region MainFields
    public static CameraActions Main { get; private set; }
    private Transform _target, _player;

    [Header("States")]
    [SerializeField]
    private Vector3 _startPoint;
    [SerializeField]
    private Vector3 _position1, _rotation1;
    [SerializeField]
    private Vector3 _position2, _rotation2;
    [SerializeField]
    private float _maxDist;

    public Transform Target
    {
        get => _target;
        set
        {
            if (value is null)
                return;

            try
            {
                _target = value;
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }
    }
    public Transform Player
    {
        get 
        {
            if (_player is null)
                _player = GameObject.FindGameObjectWithTag("Player").transform;

            return _player; 
        }
    }

    #endregion

    #region Setup
    private void Awake() => Main = this;
    #endregion

    private void LateUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        Vector3 mappedPosition = Vector3.Lerp(
            _position1,
            _position2,
            Remap(Vector3.Distance(Player.position, _startPoint), 0, _maxDist)
        );
        Vector3 interpolatedPosition = Vector3.Lerp(
            transform.position,
            mappedPosition,
            1 - Mathf.Pow(0.5f, 2 * Time.deltaTime)
        );
        transform.position =interpolatedPosition;

        float Remap(float value, float minValue, float maxValue)
        {
            return Mathf.Clamp01((value - minValue) / (maxValue - minValue));
        }
    }

    private void Rotate()
    {
        Vector3 mappedRotation = Vector3.Lerp(
            _rotation1,
            _rotation2,
            Remap(Vector3.Distance(_player.position, _startPoint), 0, _maxDist)
        );

        transform.rotation = Quaternion.Euler(mappedRotation);

        float Remap(float value, float minValue, float maxValue)
        {
            return Mathf.Clamp01((value - minValue) / (maxValue - minValue));
        }
    }


    /// <summary>
    /// Shake the camera with the specified amount and time.
    /// </summary>
    /// <param name="amount">The amount of shake.</param>
    /// <param name="time">The duration of the shake.</param>
    public void Shake(float amount, float time) => StartCoroutine(Shaker(amount, time));

    /// <summary>
    /// Coroutine for shaking the camera.
    /// </summary>
    /// <param name="amount">The amount of shake.</param>
    /// <param name="time">The duration of the shake.</param>
    private IEnumerator Shaker(float amount, float time)
    {
        float timer = 0;
        while (time != 0 ? timer < time : amount > 0.01f)
        {
            transform.position += new Vector3(Random.Range(-amount, amount), 0, Random.Range(-amount, amount));
            yield return new WaitForSeconds(0.04f);
            amount *= 0.8f;
            timer += 0.04f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_startPoint, 0.5f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_position1, 0.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_position2, 0.5f);
    }

}
