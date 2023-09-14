using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    [Serializable]
    public struct DialoguePage
    {
        public string CharacterName;
        public string Text;
        public AudioClip VoiceOver;
    }

    public List<DialoguePage> Pages = new();
}
