using UnityEngine;
public enum TaskID
{
    WateringPlants = 0,
    DoodleJump = 1,
    Television = 99,
    ToyBox = 2,
    DrawingGame = 3,
    WateringPlants2 = 4,
    DoodleJump2 = 5,
    ToyBox2 = 6,
    DrawingGame2 = 7,
    WateringPlants3 = 8,
    DoodleJump3 = 9,
    ToyBox3 = 10,
    DrawingGame3 = 11,
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
    public GameObject TutorialPrefab;
}