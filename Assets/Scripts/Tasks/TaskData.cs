using UnityEngine;
public enum TaskID
{
    WateringPlants,
    DoodleJump,
    Television,
    ToyBox,
    DrawingGame,
    WateringPlants2,
    DoodleJump2,
    ToyBox2,
    DrawingGame2,
    WateringPlants3,
    DoodleJump3,
    ToyBox3,
    DrawingGame3,
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