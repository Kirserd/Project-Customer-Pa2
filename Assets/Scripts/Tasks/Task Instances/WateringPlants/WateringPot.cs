using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[System.Obsolete]
public class WateringPot : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    private void Start() => _particles.enableEmission = false;
    private void Update()
    {
        SetPositionToCursor();
        if (Input.GetMouseButtonDown(0))
            _particles.enableEmission = true;
        if (Input.GetMouseButtonUp(0))
            _particles.enableEmission = false;
    }
    private void OnParticleCollision(GameObject other)
    {
        other.gameObject.TryGetComponent(out Plant plant);
        if(plant == null)
            return;
        plant.Water();
    }
    private void SetPositionToCursor()
    {
        Vector3 MousePosition = MouseManager.Instance.GetWorldPosition(MouseManager.Instance.GameCamera);
        Transform parent = transform.parent;
        Vector3 newPosition = MousePosition;
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}