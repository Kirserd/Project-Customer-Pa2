using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class TaskIcon : MonoBehaviour
{
    public delegate void AllTasksFadeHandler(bool state);
    public static AllTasksFadeHandler AllTasksFade;

    [SerializeField]
    private float _constantSize = 2;
    [SerializeField]
    private SerializableDictionary<TaskStarter.Availability, Sprite> Icons;
    
    private bool _isFading;
    private bool _fadeDirection;
    private Material _material;

    private bool _toBeDestroyed;

    private void Start()
    {
        RotateToCamera();
        ScaleItself();
        RefreshMaterial();
        AllTasksFade += Fade;
    }
    private void RefreshMaterial() =>_material = GetComponent<Renderer>().material;
    private void ScaleItself() => transform.localScale = new Vector3
    (
        _constantSize,
        _constantSize,
        transform.localScale.z
    );
    private void RotateToCamera()
    {
        Transform cameraTransform = Camera.main.transform;
        transform.rotation = Quaternion.LookRotation(cameraTransform.forward, cameraTransform.up);
    }
    public void SetIcon(TaskStarter.Availability key)
    {
        if (key == TaskStarter.Availability.Done || key == TaskStarter.Availability.Late)
        {
            StartCoroutine(FadeDestroy());
        }
        GetComponent<SpriteRenderer>().sprite = Icons[key];
    }

    public void Fade(bool state)
    {
        _isFading = true;
        _fadeDirection = state;
    }
    private void AnimateFade()
    {
        if (_toBeDestroyed)
            return;

        float opacity = _material.GetFloat("_Opacity");
        if (!_fadeDirection && opacity < 1f)
            _material.SetFloat("_Opacity", opacity + Time.deltaTime * 4f);
        else if (_fadeDirection && opacity > 0f)
            _material.SetFloat("_Opacity", opacity - Time.deltaTime * 4f);
        else if (opacity >= 1f)
        {
            _material.SetFloat("_Opacity", 1f);
            _isFading = false;
        }
        else
        {
            _material.SetFloat("_Opacity", 0f);
            _isFading = false;
        }
    }
    private void Update()
    {
        if (_isFading)
            AnimateFade();
    
        RotateToCamera();
    }
    private IEnumerator FadeDestroy()
    {
        AllTasksFade -= Fade;
        _toBeDestroyed = true;
        _material.SetFloat("_Opacity", 1);
        yield return new WaitForSeconds(1.5f);
        float opacity = _material.GetFloat("_Opacity");
        while (opacity > 0f)
        {
            opacity = _material.GetFloat("_Opacity");
            _material.SetFloat("_Opacity", opacity - Time.deltaTime * 2f);
            yield return new WaitForEndOfFrame();
        }
    }
}
