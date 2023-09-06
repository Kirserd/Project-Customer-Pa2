using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CameraState
{
    PLAYER,
    TARGET,
    FREE
};
public enum CameraMovementType
{
    INTERPOLATION,
    LINEAR,
    TELEPORT
};

public struct SequencePoint
{
    public SequencePoint(Vector3 point, CameraMovementType movementType)
    {
        Point = point;
        Rotation = new Vector3(-1,-1,-1);
        MovementType = movementType;
        ZoomMultiplier = -1;
        LinearSpeed = -1;
        InterpolateSpeed = -1;
    }
    public SequencePoint(Vector3 point, CameraMovementType movementType, float zoomMultiplier = -1, float linearSpeed = -1, float interpolateSpeed = -1)
    {
        Point = point;
        Rotation = new Vector3(-1, -1, -1);
        MovementType = movementType;
        ZoomMultiplier = zoomMultiplier;
        LinearSpeed = linearSpeed;
        InterpolateSpeed = interpolateSpeed;
    }
    public SequencePoint(Vector3 point, Vector3 rotation, CameraMovementType movementType, float zoomMultiplier = -1, float linearSpeed = -1, float interpolateSpeed = -1)
    {
        Point = point;
        Rotation = rotation;
        MovementType = movementType;
        ZoomMultiplier = zoomMultiplier;
        LinearSpeed = linearSpeed;
        InterpolateSpeed = interpolateSpeed;
    }
    public Vector3 Point { get; }
    public Vector3 Rotation { get; }
    public CameraMovementType MovementType { get; }
    public float ZoomMultiplier, LinearSpeed, InterpolateSpeed;

    public Vector3 PointWithExtraDistance(float distance)
    {
        Vector3 ExtraDistance = new Vector3
            (
            Point.x > 0 ? distance : -distance,
            Point.y > 0 ? distance : -distance,
            Point.z > 0 ? distance : -distance
            );
        return Point + ExtraDistance;
    }
}

/// <summary>
/// Manages camera actions and movements.
/// </summary>
public class CameraActions : MonoBehaviour
{
    #region MainFields
    public static CameraActions Main { get; private set; }

    public CameraState Focus;
    public CameraMovementType MovementType;

    private Transform _target, _player;

    [SerializeField]
    private float _zoomMultiplier = 1, _linearSpeed = 1, _interpolateSpeed = 5f;

    [SerializeField]
    private Vector3 _offset = Vector3.zero;

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
    public float ZoomMultiplier
    {
        get
        {
            return _zoomMultiplier;
        }
        set
        {
            if (value > 0)
                _zoomMultiplier = value;
        }
    }
    public float LinearSpeed
    {
        get
        {
            return _linearSpeed;
        }
        set
        {
            if (value > 0)
                _linearSpeed = value;
        }
    }
    public float InterpolateSpeed
    {
        get
        {
            return _interpolateSpeed;
        }
        set
        {
            if (value > 0)
                _interpolateSpeed = value;
        }
    }
    public Vector3 Offset
    {
        get => _offset;
    }

    #endregion

    #region SequenceFields

    private bool _sequenceIsRunning;
    private readonly Queue<SequencePoint> Sequence = new Queue<SequencePoint>();

    #endregion

    #region Setup
    private void Awake() => Main = this;
    #endregion

    private void LateUpdate()
    {
        if (_sequenceIsRunning)
            return;

        switch (Focus)
        {
            case CameraState.PLAYER:
                Move(Player.position);
                break;
            case CameraState.TARGET:
                Move(Target.position);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Move the camera to the specified destination.
    /// </summary>
    /// <param name="destination">The destination position to move the camera to.</param>
    private void Move(Vector3 destination)
    {
        switch (MovementType)
        {
            case CameraMovementType.INTERPOLATION:
                Interpolate();
                break;
            case CameraMovementType.LINEAR:
                LinearMove();
                break;
            default:
                Teleport();
                break;
        }
        void Interpolate()
        {
            Vector3 interpolatedPosition = Vector3.Lerp(
                transform.position,
                destination + new Vector3(_offset.x, _offset.y * _zoomMultiplier, _offset.z),
                1 - Mathf.Pow(0.5f, InterpolateSpeed * Time.deltaTime)
            );
            transform.position = interpolatedPosition;
        }
        void LinearMove()
        {
            transform.position += LinearSpeed * Time.deltaTime *
                new Vector3(
                    destination.x + _offset.x - transform.position.x,
                    destination.y + _offset.y - transform.position.y,
                    destination.z + _offset.z - transform.position.z
                ).normalized;
        }
        void Teleport()
        {
            transform.position = new Vector3(
                destination.x + _offset.x,
                destination.y + _offset.y * _zoomMultiplier,
                destination.z + _offset.z
            );
        }
    }

    /// <summary>
    /// Rotate the camera to the specified rotation.
    /// </summary>
    /// <param name="rotationDestination">The destination rotation to rotate the camera to.</param>
    private void Rotate(Vector3 rotationDestination)
    {
        Vector3 InterpolatedRotation = Vector3.Lerp
        (
            transform.rotation.eulerAngles,
            rotationDestination,
            1 - Mathf.Pow(0.5f, InterpolateSpeed * Time.deltaTime)
        );
        transform.rotation = Quaternion.Euler(InterpolatedRotation);
    }

    /// <summary>
    /// Add a sequence point to the camera sequence.
    /// </summary>
    /// <param name="point">The sequence point to add.</param>
    public void AddSequencePoint(SequencePoint point)
    {
        Sequence.Enqueue(point);
        if (_sequenceIsRunning)
            return;

        StartCoroutine(PlaySequence());
    }

    /// <summary>
    /// Play the camera sequence.
    /// </summary>
    private IEnumerator PlaySequence()
    {
        _sequenceIsRunning = true;

        CameraMovementType initialMovementType = MovementType;
        float initialInterpolateSpeed = _interpolateSpeed;
        float initialZoomMultiplier = _zoomMultiplier;
        float initialLinearSpeed = _linearSpeed;

        SequencePoint startPoint = Sequence.Dequeue();
        Vector3 destination = startPoint.PointWithExtraDistance(1f);
        Vector3 rotationDestination = startPoint.Rotation;
        bool isFinishedPlaying = false;

        MovementType = startPoint.MovementType;
        InterpolateSpeed = startPoint.InterpolateSpeed;
        ZoomMultiplier = startPoint.ZoomMultiplier;
        LinearSpeed = startPoint.LinearSpeed;

        while (true)
        {
            if (isFinishedPlaying)
            {
                if (Sequence.Count == 0)
                    break;

                SequencePoint point = Sequence.Dequeue();
                destination = point.PointWithExtraDistance(0.1f);
                rotationDestination = startPoint.Rotation;
                isFinishedPlaying = false;
                MovementType = point.MovementType;
            }
            else if (Vector3.Distance(transform.position, destination + new Vector3(_offset.x, _offset.y * _zoomMultiplier, _offset.z)) < 0.1f)
                isFinishedPlaying = true;

            Move(destination);
            if (startPoint.Rotation != new Vector3(-1, -1, -1))
                Rotate(rotationDestination);
            yield return null;
        }

        MovementType = initialMovementType;
        InterpolateSpeed = initialInterpolateSpeed;
        ZoomMultiplier = initialZoomMultiplier;
        LinearSpeed = initialLinearSpeed;
        _sequenceIsRunning = false;
    }

    /// <summary>
    /// Shake the camera with the specified amount and time.
    /// </summary>
    /// <param name="amount">The amount of shake.</param>
    /// <param name="time">The duration of the shake.</param>
    public void Shake(float amount, float time)
    {
        StartCoroutine(Shaker(amount, time));
    }

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
}
