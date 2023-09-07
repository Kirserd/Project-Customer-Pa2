using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TaskIcon : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<TaskStarter.Availability, Sprite> Icons;

    public void SetIcon(TaskStarter.Availability key) => GetComponent<SpriteRenderer>().sprite = Icons[key];
    public void Fade(bool state)
    {

    }
}
