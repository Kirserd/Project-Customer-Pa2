using UnityEngine;

public class ScreenEventListener : MonoBehaviour
{
    public delegate void OnScreenSizeChangedHandler(float width, float height);
    public static OnScreenSizeChangedHandler OnScreenSizeChanged;

    private float _previousWidth;
    private float _previousHeight;

    private void Update()
    {
        if (_previousHeight != Screen.height || _previousWidth != Screen.width)
        {
            _previousHeight = Screen.height;
            _previousWidth = Screen.width;
            OnScreenSizeChanged.Invoke(_previousWidth, _previousHeight);
        }
    }
}
