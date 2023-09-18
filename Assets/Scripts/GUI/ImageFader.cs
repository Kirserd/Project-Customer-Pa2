using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    private bool _isFading;
    private bool _fadeDirection;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private float _minOpacity = 0f;
    [SerializeField]
    private float _maxOpacity = 1f;

    private void Start() => _image = GetComponent<Image>();

    public void FadeIn() => Fade(false);
    public void FadeOut() => Fade(true);
    public void Fade(bool state)
    {
        _isFading = true;
        _fadeDirection = state;
    }
    private void AnimateFade()
    {
        float opacity = _image.color.a;
        if (!_fadeDirection && opacity < _maxOpacity)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, opacity + Time.deltaTime * 4f);
        else if (_fadeDirection && opacity > _minOpacity)
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, opacity - Time.deltaTime * 4f);
        else if (opacity >= _maxOpacity)
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _maxOpacity);
            _isFading = false;
        }
        else
        {
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _minOpacity);
            _isFading = false;
        }
    }
    private void Update()
    {
        if (_isFading)
            AnimateFade();
    }
}
