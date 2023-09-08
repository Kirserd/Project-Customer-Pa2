using UnityEngine;

[System.Obsolete]
[RequireComponent(typeof(ParticleSystem))]
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
        Vector3 MousePosition = MouseManager.Instance.GetWorldPosition();
        Transform parent = transform.parent;
        transform.position = Quaternion.Euler(parent.rotation.ToEuler()) * (MousePosition - parent.position) + parent.position;
    }
}