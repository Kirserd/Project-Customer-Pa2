using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum FromSceneToScene
{
    KitchenToLivingRoom,
    LivingRoomToKitchen,
    BedroomToLivingRoom,
    LivingRoomToBedroom,
}
public class Teleport : MonoBehaviour, IInteractable
{
    private static FromSceneToScene _current = FromSceneToScene.KitchenToLivingRoom;
    private static bool _placed = false;
    private static Dictionary<FromSceneToScene, Teleport> _spawns = new();


    [SerializeField]
    private SerializableDictionary<FromSceneToScene, FromSceneToScene> _reverseFromTo = new()
    {
        { FromSceneToScene.KitchenToLivingRoom, FromSceneToScene.LivingRoomToKitchen},
        { FromSceneToScene.LivingRoomToKitchen, FromSceneToScene.KitchenToLivingRoom },
        { FromSceneToScene.BedroomToLivingRoom, FromSceneToScene.LivingRoomToBedroom },
        { FromSceneToScene.LivingRoomToBedroom, FromSceneToScene.BedroomToLivingRoom }
    };

    [SerializeField]
    private SerializableDictionary<FromSceneToScene, string> _sceneNames = new()
    {
        { FromSceneToScene.KitchenToLivingRoom, "LivingRoom" },
        { FromSceneToScene.LivingRoomToKitchen, "Kitchen" },
        { FromSceneToScene.BedroomToLivingRoom, "LivingRoom" },
        { FromSceneToScene.LivingRoomToBedroom, "Bedroom" }
    };


    [SerializeField]
    private FromSceneToScene _fromTo;
    [SerializeField]
    private Vector3 _spawnOffset;
    public GameObject GameObject => gameObject;

    public bool IsActive => _isActive;
    private bool _isActive;

    public bool IsSelected { get => _isSelected; set => _isSelected = value; }
    private bool _isSelected;

    public void Deselect()
    {
        transform.localScale /= 1.05f;
        _isSelected = false;
    }
    public void Interact()
    {
        if (Dad.PlayerStateMachine.CurrentState is not MovingState)
            return;

        _current = _fromTo;
        AudioManager.Source.PlayOneShot(AudioManager.Clips["OpenDoor"], 0.5f);
        ResetSubscriptions();
        SceneManager.LoadScene(_sceneNames[_current], LoadSceneMode.Single);
        _current = _reverseFromTo[_current];
        _placed = false;
    }

    private void ResetSubscriptions()
    {
        ScreenEventListener.OnScreenSizeChanged = null;
        TaskIcon.AllTasksFade = null;
        TaskStarter.AllHintsAppear = null;
        DayCycle.OnTimeChanged = null;
        DayCycle.OnTimeIntervalChanged = null;
    }

    public void Select()
    {
        transform.localScale *= 1.05f;
        _isSelected = true;
    }
    private void Start()
    {
        _isActive = true;
        _spawns[_fromTo] = this;
        Place();
    }
    private void Place()
    {
        if (_placed || _current != _fromTo)
            return;

        Task.RefreshReferences();
        Teleport spawn = _spawns[_current];
        Vector3 newPosition = new
            (
                spawn.transform.position.x + spawn._spawnOffset.x, 
                Task.Dad.transform.position.y, 
                spawn.transform.position.z + spawn._spawnOffset.z
            );
        Task.Dad.transform.position = newPosition;
        _placed = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + _spawnOffset, 2f);
    }
}
