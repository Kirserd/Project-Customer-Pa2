using UnityEngine;

public class RotateToCamera : MonoBehaviour
{
    public Vector3 Rotation;
    private void Update()
    {
        Transform cameraTransform = Camera.main.transform;
        transform.rotation = Quaternion.LookRotation(Rotation, cameraTransform.up);
    }
}
