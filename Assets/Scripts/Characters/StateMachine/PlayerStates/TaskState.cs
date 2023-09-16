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
        TaskIcon.AllTasksFade.Invoke(true);
        TaskStarter.AllHintsAppear.Invoke(true);
        _gameCamera.enabled = true;
        base.EnterState();
    }
    public override void ExitState()
    {
        TaskIcon.AllTasksFade.Invoke(false);
        TaskStarter.AllHintsAppear.Invoke(false);
        _gameCamera.enabled = false;
        base.ExitState();
    }
}
