using UnityEngine;
public enum TaskID
{
    WateringPlants,
    DoodleJump
}
[CreateAssetMenu(fileName = "New Task", menuName = "Tasks")]
public class TaskData : ScriptableObject
{
    public string Name;
    public byte Points;
    public bool IsGame;
    public TaskID TaskID;
    public Task Task { get => TaskArchive.GetTaskByID(TaskID); }
    public GameObject TaskPrefab;
}