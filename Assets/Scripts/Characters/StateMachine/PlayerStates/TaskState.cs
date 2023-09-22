using UnityEngine;

public class TaskState : PlayerState
{
    private TaskData _data;
    private Camera _gameCamera;
    public TaskState(Dad player, TaskData data) : base(player)
    {
        _data = data;
        _gameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
    }
    public override void EnterState()
    {
        SetTaskInWorldUI(true);
        SubscribeToInput();
        base.EnterState();
    }
    public override void ExitState()
    {
        SetTaskInWorldUI(false);
        UnsubscribeFromInput();
        base.ExitState();
    }
    private void SetTaskInWorldUI(bool state)
    {
        TaskIcon.AllTasksFade?.Invoke(state);
        TaskStarter.AllHintsAppear?.Invoke(state);
        _gameCamera.enabled = state;
    }
    private void SubscribeToInput()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.Esc] += StopTask;
        InputSubscriber.InputEvents[(int)BoundKeys.Backspace] += StopTask;
    }
    private void UnsubscribeFromInput()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.Esc] -= StopTask;
        InputSubscriber.InputEvents[(int)BoundKeys.Backspace] -= StopTask;
    }
    private void StopTask(ButtonState state) => _data.Task.ForcefullyStop(TaskStarter.Availability.Scheduled, state);
}
