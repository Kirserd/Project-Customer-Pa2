﻿using UnityEngine;

public class TaskStarter : MonoBehaviour, IInteractable
{
    public enum Availability
    {
        Early,
        Scheduled,
        Done,
        Late
    }
    public GameObject GameObject => gameObject;
    public bool IsActive => _isActive;
    private bool _isActive;

    [SerializeReference]
    public TaskData Data;

    [SerializeField]
    private TaskIcon _iconPrefab;
    private TaskIcon _icon;

    [SerializeField]
    private Availability _state = Availability.Early;

    [SerializeField]
    private DayCycle.TimeInterval _interval = DayCycle.TimeInterval.LATE_EVENING;

    public delegate void OnStateChangedHandler(Availability state);
    public OnStateChangedHandler OnStateChanged;

    public delegate void OnSelectionStateChangedHandler(bool state);
    public OnSelectionStateChangedHandler OnSelectionStateChanged;

    private void Awake()
    {
        OnStateChanged += ctx => TryCreatingIcon(ctx);
        DayCycle.OnTimeIntervalChanged += ctx => HandleTimeIntervalChange(ctx);
    }

    private void HandleTimeIntervalChange(DayCycle.TimeInterval interval)
    {
        if ((int)interval < (int)_interval) SetAvailabilityState(Availability.Early);
        else if ((int)interval == (int)_interval) SetAvailabilityState(Availability.Scheduled);
        else SetAvailabilityState(Availability.Late);
    }

    private void TryCreatingIcon(Availability state)
    {
        if (_icon is not null || state != Availability.Scheduled)
            return;
        try 
        {
            _icon = Instantiate(_iconPrefab.gameObject, gameObject.transform).GetComponent<TaskIcon>();
            OnStateChanged += ctx => _icon.SetIcon(ctx);
            OnSelectionStateChanged += ctx => _icon.Fade(ctx);
        }
        catch {}
    }

    public void SetAvailabilityState(Availability state) 
    {
        _state = state;
        if (state != Availability.Scheduled)
            TurnSelectabilityTo(false);
        else
            TurnSelectabilityTo(true);
        OnStateChanged?.Invoke(state);
    }

    public void TurnSelectabilityTo(bool state)
    {
        if (_isActive == state) 
            return;

        _isActive = state;

        if (!state)
            Deselect();
    }

    public void Deselect()
    {
        TryGetComponent(out Renderer renderer);

        if (renderer is null)
            return;

        renderer.material.SetColor("_Color", Color.white);
        OnSelectionStateChanged?.Invoke(false);
    }

    public virtual void Interact() => Data.Task.Start(this);

    public void Select()
    {
        if (!IsActive)
            return;

        TryGetComponent(out Renderer renderer);

        if (renderer is null)
            return;

        renderer.material.SetColor("_Color", SelectionManager.Instance.SelectionColor);
        OnSelectionStateChanged?.Invoke(true);
    }

    public GameObject InstantiatePrefab(Transform transform) => Instantiate(Data.TaskPrefab, transform);
}