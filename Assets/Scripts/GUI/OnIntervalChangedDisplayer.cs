using System.Collections;
using UnityEngine;
using TMPro;

public class OnIntervalChangedDisplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    public void SetTextTo(DayCycle.TimeInterval interval) 
    {
        Debug.Log(interval);
        _text.text = "\n>> >> >> >> >> >> >> >> " + interval +
            " >> >> >> >> >> >> >> >>\n______________________________________________________________________";
    }
    private void Start() => StartCoroutine(DestroyAfterFade());
    
    private IEnumerator DestroyAfterFade()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
