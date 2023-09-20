using UnityEngine;
using TMPro;

public class ResultDisplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name, _desc, _minuses, _pluses;
    [SerializeField]
    private Transform _point;

    private void Start()
    {
        Vector2 coordinates = PointManager.CalculateCoordinate();
        _point.localPosition = new Vector3(coordinates.x * 100, coordinates.y * 100, 0);
    }
}
