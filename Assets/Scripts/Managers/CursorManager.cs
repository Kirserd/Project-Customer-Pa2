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
    private SerializableDictionary<string, Sprite> _cursorDictionary = new SerializableDictionary<string, Sprite>();

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
    public void AddCursor(string key, Sprite cursorTexture)
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
        if (_cursorDictionary.ContainsKey(key))
            StartCoroutine(SetCursorAsync(key));
    }

    private IEnumerator SetCursorAsync(string key)
    {
        Sprite cursorSprite = _cursorDictionary[key];
        yield return StartCoroutine(SpriteToTexture2DAsync(cursorSprite, texture =>
        {
            _cursorTexture = texture;
            Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);
        }));
    }
    private IEnumerator SpriteToTexture2DAsync(Sprite sprite, System.Action<Texture2D> onComplete)
    {
        if (sprite == null)
        {
            onComplete?.Invoke(null);
            yield break;
        }

        Texture2D texture = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height, TextureFormat.RGBA32, false, true);
        texture.filterMode = sprite.texture.filterMode;
        texture.SetPixels(sprite.texture.GetPixels((int)sprite.textureRect.x, (int)sprite.textureRect.y, (int)sprite.textureRect.width, (int)sprite.textureRect.height));
        texture.Apply();

        yield return null;

        onComplete?.Invoke(texture);
    }

    /// <summary>
    /// Resets the cursor to the default cursor by setting it to the first cursor in the dictionary.
    /// </summary>
    public void ResetCursor() => SetCursor(_cursorDictionary.Keys.First());
}