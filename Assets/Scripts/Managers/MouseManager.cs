using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages mouse input and interaction with the game world.
/// </summary>
public class MouseManager : MonoBehaviour
{
    public static MouseManager Instance { get; private set; }
    public Camera MainCamera;
    public Camera GameCamera;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += Refresh;

        if (MainCamera == null)
            MainCamera = Camera.main;
        if (GameCamera == null)
            GameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
    }

    /// <summary>
    /// Refreshes the camera references when a new scene is loaded.
    /// </summary>
    /// <param name="scene">The loaded scene.</param>
    /// <param name="mode">The load scene mode.</param>
    private void Refresh(Scene scene, LoadSceneMode mode)
    {
        MainCamera = Camera.main;
        GameCamera = GameObject.FindGameObjectWithTag("GameCamera").GetComponent<Camera>();
    }
    /// <summary>
    /// Gets the world position of the mouse cursor from screen.
    /// </summary>
    /// <returns>The world position of the mouse cursor.</returns>
    public Vector3 GetWorldPosition(Camera camera)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -camera.transform.position.z;
        return camera.ScreenToWorldPoint(mousePosition);
    }

    /// <summary>
    /// Checks if the mouse cursor is over a UI element.
    /// </summary>
    /// <returns><c>true</c> if the mouse cursor is over a UI element; otherwise, <c>false</c>.</returns>
    public bool IsMouseOverUI() => UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

    /// <summary>
    /// Performs a raycast from the mouse cursor to the game world with the specified layer mask.
    /// </summary>
    /// <param name="layerMask">The layer mask to use for the raycast.</param>
    /// <returns>The RaycastHit object containing information about the hit.</returns>
    public RaycastHit RaycastToWorld(LayerMask layerMask)
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            return hit;
        else
            return new RaycastHit();
    }

    /// <summary>
    /// Performs a raycast from the mouse cursor to the game world.
    /// </summary>
    /// <returns>The RaycastHit object containing information about the hit.</returns>
    public RaycastHit RaycastToWorld()
    {
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
            return hit;
        else
            return new RaycastHit();
    }
}