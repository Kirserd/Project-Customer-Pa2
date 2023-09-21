using UnityEngine;

public enum Apps
{
    None,
    Notes,
    Contacts
}
public class PhoneApps : MonoBehaviour
{
    [SerializeField]
    private SerializableDictionary<Apps, GameObject> _apps;
    [SerializeField]
    private Apps _currentApp;

    private void Start() => Clear();

    private void Clear()
    {
        foreach (GameObject gameObject in _apps.Values)
            gameObject.SetActive(false);
    }

    public void TurnOn(int appId)
    {
        Apps app = (Apps)appId;
        if (_currentApp == app)
            return;

        Clear();

        if (app != Apps.None)
            _apps[app].SetActive(true);

        _currentApp = app;

        if (app == Apps.Notes)
            DisplayNotes();
    }

    private void DisplayNotes() => _apps[Apps.Notes].GetComponent<Notes>().UpdateAll();
    
    public void ChangePickUpStateProxy() => Phone.Instance.ChangePickUpStateProxy();
    public void Click() => AudioManager.Source.PlayOneShot(AudioManager.Clips["PhoneClick"]);
}
