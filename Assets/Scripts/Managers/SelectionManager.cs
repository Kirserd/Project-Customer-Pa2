using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance { get; private set; }

    [ColorUsage(true, true)]
    public Color SelectionColor;
    
    [SerializeField]
    private ISelectable _selectedObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SceneManager.sceneLoaded += Refresh;
    }

    /// <summary>
    /// Nullifies selected object when a new scene is loaded.
    /// </summary>
    /// <param name="scene">The loaded scene.</param>
    /// <param name="mode">The load scene mode.</param>
    private void Refresh(Scene scene, LoadSceneMode mode) => _selectedObject = null;

    /// <summary>
    /// Gets currently selected object or null.
    /// </summary>
    public ISelectable GetSelectedObject() => _selectedObject;

    /// <summary>
    /// Sets new selected object and deselects previous one.
    /// </summary>
    /// <param name="selectable">The selected object.</param>
    public void SetSelectedObject(ISelectable selectable)
    {
        if (_selectedObject == selectable)
            return;

        if (_selectedObject != null)
            _selectedObject.Deselect();

        _selectedObject = selectable;

        if (_selectedObject != null)
            _selectedObject.Select();
    }

    public void TrySelectClosestFromPoint(Vector3 point, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(point, radius);

        List<ISelectable> selectables = new List<ISelectable>();
        foreach (Collider collider in colliders)
        {
            collider.gameObject.TryGetComponent(out ISelectable selectable);
            if (selectable is null)
                continue;

            selectables.Add(selectable);
        }

        if (selectables is null || selectables.Count == 0)
        {
            SetSelectedObject(null);
            return;
        }

        ISelectable closestSelectable = ClosestFrom(selectables.ToArray());
        SetSelectedObject(closestSelectable);
    }

    private ISelectable ClosestFrom(ISelectable[] selectables)
    {
        if (selectables.Length == 1)
            return selectables[0];

        ISelectable closest = null;
        float minDistance = float.MaxValue;
        for (int i = 0; i < selectables.Length; i++)
        {
            float distance = Vector3.Distance(selectables[i].GameObject.transform.position, transform.position);
            if (distance < minDistance && selectables[i].IsActive)
            {
                minDistance = distance;
                closest = selectables[i];
            }
        }
        return closest;
    }
}