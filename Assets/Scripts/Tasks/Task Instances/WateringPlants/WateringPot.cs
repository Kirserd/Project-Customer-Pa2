using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[System.Obsolete]
public class WateringPot : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    private void Start() 
    {
        AudioManager.Source.clip = AudioManager.Clips["WPPour"];
        AudioManager.Source.loop = true;
        AudioManager.Source.volume = 0.5f;
        AudioManager.Source.Pause();
        _particles.enableEmission = false; 
    }
    private void Update()
    {
        SetPositionToCursor();
        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Source.Play();
            _particles.enableEmission = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            AudioManager.Source.Pause();
            _particles.enableEmission = false;
        }
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
        transform.position = new Vector3(MousePosition.x, MousePosition.y, transform.position.z);
    }
}