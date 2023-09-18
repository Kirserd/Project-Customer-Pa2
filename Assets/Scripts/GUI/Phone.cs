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

    private Dad _dad;
    private Transform _root;
    private bool _pickedUp;

    private List<GameObject> _notifications = new();

    private bool _pickUpOnCD = false;
    private const float PICK_UP_CD = 0.6f;

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
        _dad = GameObject.FindGameObjectWithTag("Player").GetComponent<Dad>();
    }

    private void Subscribe()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.PickUpPhone] += ChangePickUpState;
    }
    public void ChangePickUpStateProxy() => ChangePickUpState(ButtonState.Press);
    private void ChangePickUpState(ButtonState state)
    {
        if (state == ButtonState.Hold ||
            _dad.PlayerStateMachine.CurrentState != _dad.MovingState &&
            _dad.PlayerStateMachine.CurrentState != _dad.PhoneState ||
            _pickUpOnCD)
            return;

        StartCoroutine(PickUpCD());
        if (!_pickedUp)
        {
            _phone = Instantiate(_phonePrefab, _root);
            _dad.PlayerStateMachine.UpdateState(_dad.PhoneState);
            Animator animator = _phone.GetComponent<Animator>();
            animator.SetTrigger("PickUp");
            ImageFader screenDarkening = _phone.transform.GetChild(0).GetComponent<ImageFader>();
            screenDarkening.FadeIn();
            _pickedUp = true;
        }
        else
        {
            _dad.PlayerStateMachine.UpdateState(_dad.MovingState);
            Animator animator = _phone.GetComponent<Animator>();
            animator.SetTrigger("Hide");
            ImageFader screenDarkening = _phone.transform.GetChild(0).GetComponent<ImageFader>();
            screenDarkening.FadeOut();
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
    private IEnumerator PickUpCD()
    {
        _pickUpOnCD = true;
        yield return new WaitForSeconds(PICK_UP_CD);
        _pickUpOnCD = false;
    }
}
