using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phone : MonoBehaviour
{
    public static Phone Instance;

    [SerializeField]
    private GameObject _phonePrefab;
    private static GameObject _phone;

    [SerializeField]
    private GameObject _notificationPrefab;

    private Dad _dad;
    private Transform _root, _notificationRoot;
    private bool _pickedUp;

    private List<GameObject> _notifications = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SceneManager.sceneLoaded += 
        (Scene scene, LoadSceneMode mode) => Refresh();
    }

    private void Refresh()
    {
        RefreshReferences();
        Subscribe();
    }

    private void RefreshReferences()
    {
        _root = GameObject.FindGameObjectWithTag("PhoneCanvas").transform;
        _notificationRoot = GameObject.FindGameObjectWithTag("Notifications").transform.GetChild(0);
        _dad = GameObject.FindGameObjectWithTag("Player").GetComponent<Dad>();
    }

    private void Subscribe()
    {
        DayCycle.OnTimeIntervalChanged += ctx => ClearNotifications();
        InputSubscriber.InputEvents[(int)BoundKeys.PickUpPhone] += ChangePickUpState;
    }
    private void ChangePickUpState(ButtonState state)
    {
        if (state == ButtonState.Hold || _dad.PlayerStateMachine.CurrentState != _dad.MovingState && _dad.PlayerStateMachine.CurrentState != _dad.PhoneState)
            return;

        if (!_pickedUp)
        {
            _phone = Instantiate(_phonePrefab, _root);
            _dad.PlayerStateMachine.UpdateState(_dad.PhoneState);
            Animator animator = _phone.GetComponent<Animator>();
            animator.SetTrigger("PickUp");
            _pickedUp = true;
        }
        else
        {
            _dad.PlayerStateMachine.UpdateState(_dad.MovingState);
            Animator animator = _phone.GetComponent<Animator>();
            animator.SetTrigger("Hide");
            StartCoroutine(WaitToDestroy());
        }
    }

    private IEnumerator WaitToDestroy()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(_phone);
        _phone = null;
        _pickedUp = false;
    }

    private void ClearNotifications()
    {
        for (int i = 0; i < _notifications.Count; i++)
            Destroy(_notifications[i]);

        _notifications.Clear();
    }

    public void PushNotification(string text) => StartCoroutine(SafePushNotification(text));
    private IEnumerator SafePushNotification(string text)
    {
        yield return new WaitForEndOfFrame();
        _notifications.Add(Instantiate(_notificationPrefab, _notificationRoot));
        _notifications[^1].GetComponent<ContentFitTextBox>().SetText(text);
        (_notifications[^1].transform as RectTransform).anchoredPosition -=
            new Vector2(0, 1.8f * (_notificationPrefab.transform as RectTransform).sizeDelta.y * _notifications.Count - 2);
    }
}
