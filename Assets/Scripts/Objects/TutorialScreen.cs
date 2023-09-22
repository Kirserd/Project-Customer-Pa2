using UnityEngine;

public class TutorialScreen : MonoBehaviour
{
    [Header("Options")]
    [SerializeField]
    private GameObject _prefabToCreateOnFinish;
    [SerializeField]
    private GameObject _gameObjectToActivate;

    public bool StopTime;
    public bool ContinueTime;

    private void Start()
    {
        if(StopTime)
            DayCycle.StopCount();

        if (_gameObjectToActivate != null)
            _gameObjectToActivate.SetActive(false);
    }

    public void Exit()
    {
        if (ContinueTime)
            DayCycle.ContinueCount();

        AudioManager.Source.PlayOneShot(AudioManager.Clips["Interval"], 1f);

        if (_prefabToCreateOnFinish != null)
            Instantiate(_prefabToCreateOnFinish, transform.parent);

        if (_gameObjectToActivate != null)
            _gameObjectToActivate.SetActive(true);

        Destroy(gameObject);
    }
}