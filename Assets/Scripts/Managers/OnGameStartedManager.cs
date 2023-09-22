using UnityEngine;

public class OnGameStartedManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _tutorialPrefab;

    private void Start()
    {
        Instantiate(_tutorialPrefab, GameObject.FindGameObjectWithTag("Canvas").transform);
        Destroy(this);
    }
}