﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class TaskStarter : MonoBehaviour, IInteractable
{
    public enum Availability
    {
        Early,
        Scheduled,
        ScheduledImportant,
        Done,
        Late,
        Television
    }
    public delegate void AllHintsAppearHandler(bool state);
    public static AllHintsAppearHandler AllHintsAppear;
    public GameObject GameObject => gameObject;
    public bool IsActive => _isActive;
    private bool _isActive;
    public TaskData Data => _data;
    [SerializeField]
    private TaskData _data;

    [SerializeField]
    private TaskIcon _iconPrefab;
    private TaskIcon _icon;

    [SerializeField]
    private GameObject _legendaPrefab;

    [SerializeField]
    private Availability _state = Availability.Early;
    public Availability CurrentState => _state;

    [SerializeField]
    private DayCycle.TimeInterval _interval = DayCycle.TimeInterval.Morning;
    public DayCycle.TimeInterval Interval => _interval;

    public bool IsSelected
    {
        get => _isSelected;
        set => _isSelected = value;
    }
    private bool _isSelected;

    [Header("Optional")]
    [SerializeField]
    private string _hintText;

    [SerializeField]
    private ContentFitTextBox _hintPrefab;
    private ContentFitTextBox _hint;

    public delegate void OnStateChangedHandler(Availability state);
    public OnStateChangedHandler OnStateChanged;

    public delegate void OnSelectionStateChangedHandler(bool state);
    public OnSelectionStateChangedHandler OnSelectionStateChanged;

    public static List<Type> TutorialsPassed = new();

    private void Awake()
    {
        OnStateChanged += TryCreatingIcon;
        DayCycle.OnTimeIntervalChanged += HandleTimeIntervalChange;
        AllHintsAppear += HandleAllHintsAppear;
    }

    private void HandleTimeIntervalChange(DayCycle.TimeInterval interval)
    {

        if (_interval == DayCycle.TimeInterval.All)
            SetAvailabilityState(Availability.Television);
        else if (PointManager.CompletionOrder.Contains((Data.TaskID, Data.IsGame)))
            SetAvailabilityState(Availability.Done);
        else if ((int)interval == (int)_interval)
        {
            if(Data.IsGame)
                SetAvailabilityState(Availability.Scheduled);
            else
                SetAvailabilityState(Availability.ScheduledImportant);
        }
        else if ((int)interval < (int)_interval)
            SetAvailabilityState(Availability.Early);
        else if (_state != Availability.Done)
            SetAvailabilityState(Availability.Late);
    }
    private void TryCreatingIcon(Availability state)
    {
        if (_icon is not null || state != Availability.Scheduled 
            && state != Availability.Television 
            && state != Availability.ScheduledImportant)
            return;
        if (_iconPrefab is null)
            return;
        _icon = Instantiate(_iconPrefab.gameObject, gameObject.transform).GetComponent<TaskIcon>();
        OnStateChanged += ctx => _icon.SetIcon(ctx);
        OnSelectionStateChanged += ctx => _icon.Fade(ctx);
    }

    public void SetAvailabilityState(Availability state)
    {
        _state = state;
        if (state != Availability.Scheduled && 
            state != Availability.ScheduledImportant && 
            state != Availability.Television)
            TurnSelectabilityTo(false);
        else 
            TurnSelectabilityTo(true);

        if (state == Availability.Done)
        {
            if (PointManager.CompletionOrder.Contains((Data.TaskID, Data.IsGame)))
                return;

            Task.Dad.AddPoints(Data.IsGame, Data.Points);
            PointManager.CompletionOrder.Add((Data.TaskID, Data.IsGame));
        }
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
        OnSelectionStateChanged?.Invoke(false);

        IsSelected = false;
        TryHideHint();
    }

    public virtual void Interact() => _data.Task.Start(this);

    public void Select()
    {
        if (!IsActive)
            return;

        OnSelectionStateChanged?.Invoke(true);

        IsSelected = true;
        TryShowHint();
    }

    public void TryShowHint()
    {
        if (_hintPrefab is null || !IsSelected)
            return;

        _icon.Fade(true);
        _hint = Instantiate(_hintPrefab.gameObject, GameObject.FindGameObjectWithTag("Notifications").transform)
            .GetComponent<ContentFitTextBox>();

        _hint.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        _hint.SetText(_hintText + "\n<size=12>Press <size=14><b><color=#622611ff>[F]</color></b><size=12> to <size=14><b><color=#622611ff>Interact</color>");
    }

    public void TryHideHint()
    {
        if (_hint is null)
            return;

        Destroy(_hint.gameObject);
        _hint = null;
    }

    private void HandleAllHintsAppear(bool state)
    {
        if (state) TryHideHint();
        else TryShowHint();
    }

    public GameObject InstantiatePrefab(Transform transform)
    {
        Instantiate(_legendaPrefab, Task.TaskGUI);
        return Instantiate(_data.TaskPrefab, transform);
    }

    public GameObject InstantiateTutorial() => Instantiate(Data.TutorialPrefab);

}
