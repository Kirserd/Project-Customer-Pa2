using UnityEngine;

public class Toy : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 _offset;

    [SerializeField]
    private float _dragRadius;

    private Rigidbody2D _rigidbody;

    private void Start() => _rigidbody = GetComponent<Rigidbody2D>();


    private void OnMouseDown()
    {
        Vector3 MousePosition = MouseManager.Instance.GetWorldPosition(MouseManager.Instance.GameCamera);
        if (Vector3.Distance(MousePosition, transform.position) > _dragRadius)
            return;

        if(isDragging == false)
            AudioManager.Source.PlayOneShot(AudioManager.Clips["TBPickUp"], 1f);

        isDragging = true;
        _offset = transform.position - MousePosition;
    }

    private void OnMouseUp() => isDragging = false;

    private void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = MouseManager.Instance.GetWorldPosition(MouseManager.Instance.GameCamera) + _offset;
            _rigidbody.velocity = new Vector3(newPosition.x, newPosition.y) - transform.position;
        }
    }
}