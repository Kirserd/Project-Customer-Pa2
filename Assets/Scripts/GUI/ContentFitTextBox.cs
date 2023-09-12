using UnityEngine;
using TMPro;
public class ContentFitTextBox : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private string _text;

    [Header("TextBox Appearance")]
    [SerializeField]
    private RectTransform[] _corners;
    [SerializeField]
    private RectTransform[] _sides;
    [SerializeField]
    private RectTransform _inside;
    [Header("Content")]
    [SerializeField]
    private TextMeshProUGUI _content;

    private Vector2 _previousContentSize;


    private void Start()
    {
        RemapTextBox();
        _previousContentSize = _content.rectTransform.sizeDelta;
    }
    private void Update()
    {
        if(_previousContentSize != _content.rectTransform.sizeDelta)
            RemapTextBox();
        if (_text != _content.text)
        {
            string newText = _text.Replace("\\n", "\n");
            _content.text = newText;
        }
    }
    private void RemapTextBox()
    {
        Vector2 halfContentSize = _content.rectTransform.sizeDelta / 2f;

        _corners[0].anchoredPosition = new Vector2(-halfContentSize.x, halfContentSize.y);
        _corners[1].anchoredPosition = halfContentSize;
        _corners[2].anchoredPosition = -halfContentSize;
        _corners[3].anchoredPosition = new Vector2(halfContentSize.x, -halfContentSize.y);

        _sides[0].sizeDelta = new Vector2(_content.rectTransform.sizeDelta.x, _sides[0].sizeDelta.y);
        _sides[1].sizeDelta = new Vector2(_content.rectTransform.sizeDelta.y, _sides[1].sizeDelta.y);
        _sides[2].sizeDelta = new Vector2(_content.rectTransform.sizeDelta.x, _sides[2].sizeDelta.y);
        _sides[3].sizeDelta = new Vector2(_content.rectTransform.sizeDelta.y, _sides[3].sizeDelta.y);

        _sides[0].anchoredPosition = new Vector2(0, halfContentSize.y);
        _sides[1].anchoredPosition = new Vector2(halfContentSize.x, 0);
        _sides[2].anchoredPosition = new Vector2(0, -halfContentSize.y);
        _sides[3].anchoredPosition = new Vector2(-halfContentSize.x, 0);

        _inside.sizeDelta = _content.rectTransform.sizeDelta;
        _inside.anchoredPosition = Vector2.zero;
        _previousContentSize = _content.rectTransform.sizeDelta;

        (gameObject.transform as RectTransform).sizeDelta = new Vector2
            (
            _sides[0].sizeDelta.x + _corners[0].sizeDelta.y + _corners[1].sizeDelta.y,
            _sides[1].sizeDelta.x + _corners[2].sizeDelta.y + _corners[3].sizeDelta.y
            );
    }
    public void SetText(string text) => _text = text;
}
