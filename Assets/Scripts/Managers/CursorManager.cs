using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the cursor and provides methods for adding, removing, and setting custom cursors.
/// </summary>
public class CursorManager : MonoBehaviour
{
    /// <summary>
    /// The singleton instance of the CursorManager class.
    /// </summary>
    public static CursorManager Instance { get; private set; }

    [Header("Parameters")]
    [SerializeField]
    private SerializableDictionary<string, Texture2D> _cursorDictionary = new SerializableDictionary<string, Texture2D>();

    private Texture2D _cursorTexture;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetCursor(_cursorDictionary.Keys.First());
    }

    /// <summary>
    /// Adds or updates a custom cursor with the specified key and sprite.
    /// </summary>
    /// <param name="key">The key associated with the cursor.</param>
    /// <param name="cursorTexture">The sprite to be used as the cursor.</param>
    public void AddCursor(string key, Texture2D cursorTexture)
    {
        if (_cursorDictionary.ContainsKey(key))
            _cursorDictionary[key] = cursorTexture;
        else
            _cursorDictionary.Add(key, cursorTexture);
    }

    /// <summary>
    /// Removes the custom cursor associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the cursor to be removed.</param>
    public void RemoveCursor(string key) => _cursorDictionary.Remove(key);

    /// <summary>
    /// Sets the cursor to the custom cursor associated with the specified key.
    /// </summary>
    /// <param name="key">The key of the cursor to be set.</param>
    public void SetCursor(string key)
    {
        if (!_cursorDictionary.ContainsKey(key))
            return;

        _cursorTexture = _cursorDictionary[key];
        Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Resets the cursor to the default cursor by setting it to the first cursor in the dictionary.
    /// </summary>
    public void ResetCursor() => SetCursor(_cursorDictionary.Keys.First());
}