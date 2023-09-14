using System.Text;
using UnityEngine;

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
    public TaskData Data => _data;
    [SerializeField]
    private TaskData _data;

    [SerializeField]
    private TaskIcon _iconPrefab;
    private TaskIcon _icon;

    [SerializeField]
    private Availability _state = Availability.Early;
    public Availability CurrentState => _state;

    [SerializeField]
    private DayCycle.TimeInterval _interval = DayCycle.TimeInterval.Morning;
    public DayCycle.TimeInterval Interval => _interval;

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

    private void Awake()
    {
        OnStateChanged += TryCreatingIcon;
        DayCycle.OnTimeIntervalChanged += HandleTimeIntervalChange;
    }

    private void HandleTimeIntervalChange(DayCycle.TimeInterval interval)
    {
        if ((int)interval < (int)_interval) SetAvailabilityState(Availability.Early);
        else if ((int)interval == (int)_interval) SetAvailabilityState(Availability.Scheduled);
        else if (_state != Availability.Done) SetAvailabilityState(Availability.Late);
    }
    private void TryCreatingIcon(Availability state)
    {
        if (_icon is not null || state != Availability.Scheduled)
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
        if (state != Availability.Scheduled)
            TurnSelectabilityTo(false);
        else
        {
            Phone.Instance.PushNotification
            (
                "TODO ( " 
                + DayCycle.IntervalToDigit(_interval) + 
                " ):\n<b>" + "\"" + GetConvertedTaskName() + "\""
            );
            TurnSelectabilityTo(true);
        }
        OnStateChanged?.Invoke(state);
        
        string GetConvertedTaskName()
        {
            string text = _data.TaskID.ToString();
            if (string.IsNullOrWhiteSpace(text))
                return "";
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
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

        TryHideHint();
    }

    public virtual void Interact() => _data.Task.Start(this);

    public void Select()
    {
        if (!IsActive)
            return;

        TryGetComponent(out Renderer renderer);

        if (renderer is null)
            return;

        renderer.material.SetColor("_Color", SelectionManager.Instance.SelectionColor);
        OnSelectionStateChanged?.Invoke(true);

        TryShowHint();
    }

    public void TryShowHint()
    {
        if (_hintPrefab is null)
            return;

        _hint = Instantiate(_hintPrefab.gameObject, GameObject.FindGameObjectWithTag("Notifications").transform)
            .GetComponent<ContentFitTextBox>();

        _hint.gameObject.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        _hint.SetText(_hintText + "\n<size=12>Press <size=14><b><color=#823721ff>[F]</color></b><size=12> to <size=14><b><color=#823721ff>Interact</color>");
    }

    public void TryHideHint()
    {
        if (_hint is null)
            return;

        Destroy(_hint.gameObject);
        _hint = null;
    }

    public GameObject InstantiatePrefab(Transform transform) => Instantiate(_data.TaskPrefab, transform);
}
