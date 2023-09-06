using UnityEngine;

public class MinigameStarter : MonoBehaviour, IInteractable
{
    public GameObject GameObject => gameObject;

    [SerializeField]
    protected MiniGame miniGame;

    public void Deselect()
    {
        TryGetComponent(out Renderer renderer);

        if (renderer is null)
            return;

        renderer.material.SetColor("_Color", Color.white);
        Debug.Log("deselected");
    }

    public virtual void Interact(){}

    public void Select()
    {
        TryGetComponent(out Renderer renderer);

        if (renderer is null)
            return;

        renderer.material.SetColor("_Color", SelectionManager.Instance.SelectionColor);
        Debug.Log("selected");
    }
}
