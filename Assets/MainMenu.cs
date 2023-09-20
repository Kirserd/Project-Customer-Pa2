using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int _playSceneIndex;
    public void LoadStartingScene() => SceneManager.LoadScene(_playSceneIndex);
    public void QuitGame() => Application.Quit();
}