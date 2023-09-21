using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioSource Source;

    [SerializeField]
    private SerializableDictionary<string, AudioClip> _clips = new();
    public static SerializableDictionary<string, AudioClip> Clips;
    
    private void Start()
    {
        Source = GetComponent<AudioSource>();
        Clips = _clips;
    }
}
