using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CustomCanvasScaler : MonoBehaviour
{
    [SerializeField]
    private Vector2 _referenceScreenSize = new(1280, 720);
    [SerializeField]
    private bool _scaleByHeight = true;
    [SerializeField]
    private float _scaleFactor = 1;

    private RectTransform _rect;
    private Vector3 _originScale;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _originScale = _rect.localScale;

        RefreshScale(Screen.width, Screen.height);
        ScreenEventListener.OnScreenSizeChanged += RefreshScale;       
    }
    private void RefreshScale(float width, float height)
    {
        Vector2 screenSizeDeviation = new Vector2(width, height) / _referenceScreenSize;
        float deltaElementSize = (_scaleByHeight ? screenSizeDeviation.y : screenSizeDeviation.x) * _scaleFactor;
        _rect.localScale = new Vector3(_originScale.x * deltaElementSize, _originScale.y * deltaElementSize, _rect.localScale.z);
    }
}
