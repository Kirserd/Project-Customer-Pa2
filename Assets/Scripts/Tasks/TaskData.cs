using UnityEngine;

[CreateAssetMenu(fileName ="New Task",menuName = "Tasks")]
public class TaskData : ScriptableObject
{
    public string Name;
    public byte Points;
    public bool IsGame;
    public Sprite Background;
    public Sprite Border;
}