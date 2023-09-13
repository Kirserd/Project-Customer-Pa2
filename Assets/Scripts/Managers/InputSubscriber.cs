using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BoundKeys
{
    ForwardKey,
    BackwardKey,
    LeftKey,
    RightKey,
    Interact,
    PickUpPhone,
    LeftClick,
    RightClick
}

public enum ButtonState
{
    None,
    Press,
    Hold
}

/// <summary>
/// The InputSubscriber class handles input events and key bindings.
/// </summary>
public sealed class InputSubscriber : MonoBehaviour
{
    /// <summary>
    /// Delegate for input events.
    /// </summary>
    /// <param name="state">The state of the button (None, Press, or Hold).</param>
    public delegate void InputEvent(ButtonState state);

    /// <summary>
    /// Array of input events for each BoundKey.
    /// </summary>
    public static InputEvent[] InputEvents = new InputEvent[Enum.GetNames(typeof(BoundKeys)).Length];

    /// <summary>
    /// Singleton instance of the InputSubscriber class.
    /// </summary>
    public static InputSubscriber Instance;

    // Assigned key names for each BoundKey.
    private static readonly string[] _assignedKeys = new string[]
    {
        "W",
        "S",
        "A",
        "D",
        "F",
        "E",
        "Mouse0",
        "Mouse1"
    };

    // Array of KeyCode for bound keys.
    private KeyCode[] _boundKeyCodes;

    /// <summary>
    /// Array of KeyCode for bound keys.
    /// </summary>
    public KeyCode[] BoundKeyCodes
    {
        get => (KeyCode[])_boundKeyCodes.Clone();
    }

    private void Start()
    {
        if (Instance is null)
        {
            Instance = this;
            SceneManager.sceneLoaded += Refresh;
            DontDestroyOnLoad(Instance);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _boundKeyCodes = new KeyCode[Enum.GetNames(typeof(BoundKeys)).Length];
        SetupKeyBinds(_boundKeyCodes);
    }
    private void Update()
    {
        for (int i = 0; i < Enum.GetNames(typeof(BoundKeys)).Length; i++)
        {
            if (Input.GetKeyDown(_boundKeyCodes[i]))
            {
                InputEvents[i]?.Invoke(ButtonState.Press);
            }
            else if (Input.GetKey(_boundKeyCodes[i]))
            {
                InputEvents[i]?.Invoke(ButtonState.Hold);
            }
            else
            {
                continue;
            }
        }
    }
    private void Refresh(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < InputEvents.Length; i++)
        {
            InputEvents[i] = null;
        }
    }

    /// <summary>
    /// Sets up the key bindings by parsing saved player preferences.
    /// </summary>
    /// <param name="keys">Array of KeyCode to store the parsed key bindings.</param>
    private void SetupKeyBinds(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
            _boundKeyCodes[i] = (KeyCode)Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(((BoundKeys)i).ToString(), _assignedKeys[i]));
    }

    /// <summary>
    /// Returns the KeyCode for the specified BoundKey.
    /// </summary>
    /// <param name="key">The BoundKey to get the KeyCode for.</param>
    /// <returns>The KeyCode for the specified BoundKey.</returns>
    public static KeyCode GetKey(BoundKeys key)
    {
        return Instance.BoundKeyCodes[(int)key];
    }
}