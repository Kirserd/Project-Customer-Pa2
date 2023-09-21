using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pen : MonoBehaviour
{
    private Vector3 _lastMousePos;
    private bool _isDrawing = false;
    private List<Vector3> _currentLine = new();

    private GameObject _drawnLinesContainer;

    private LineRenderer _lineRenderer;
    private LineRenderer _savedLineRendererPrefab;

    private Texture2D _canvasTexture;
    public Texture2D _desiredTexture;

    private const float INIT = 0.1107f;
    private const float PREDICATE = 95;

    private float _similarityValue;

    [SerializeField]
    private GaugeVisualizer _progressVisualizer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;

        _drawnLinesContainer = new GameObject("DrawnLinesContainer");
        _drawnLinesContainer.transform.SetParent(gameObject.transform);
        _savedLineRendererPrefab = GetComponent<LineRenderer>();
        _canvasTexture = new Texture2D(256, 256);

        AudioManager.Source.clip = AudioManager.Clips["DRDraw"];
        AudioManager.Source.loop = true;
        AudioManager.Source.volume = 0.5f;
        AudioManager.Source.Pause();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!_isDrawing)
                AudioManager.Source.Play();

            _isDrawing = true;
            _lineRenderer.positionCount = 0;
            _currentLine.Clear();
        }

        if (_isDrawing && Input.GetMouseButton(0))
        {
            Vector3 currentMousePos = MouseManager.Instance.GetWorldPosition(MouseManager.Instance.GameCamera);
            currentMousePos.z = 0;

            if (Vector3.Distance(currentMousePos, _lastMousePos) > 0.03f)
            {
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, currentMousePos);
                _currentLine.Add(currentMousePos);
            }

            _lastMousePos = currentMousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_isDrawing)
                AudioManager.Source.Pause();
            _isDrawing = false;
            SaveCurrentLine();
            UpdateCanvasTexture();
        }
    }

    private void UpdateCanvasTexture()
    {
        RenderTexture renderTexture = new(_canvasTexture.width, _canvasTexture.height, 0, RenderTextureFormat.ARGB32);
        Camera mainCamera = Camera.main;

        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();

        RenderTexture.active = renderTexture;
        _canvasTexture.ReadPixels(new Rect(0, 0, _canvasTexture.width, _canvasTexture.height), 0, 0);
        _canvasTexture.Apply();

        mainCamera.targetTexture = null;

        RenderTexture.active = null;
        Destroy(renderTexture);

        _similarityValue = 22000 * (1 - CompareTextures(_canvasTexture, _desiredTexture) - INIT);
        UpdateGUI();
        CheckPredicate();
    }

    private void UpdateGUI() => _progressVisualizer.Value = _similarityValue;

    private void CheckPredicate()
    {
        if (_similarityValue >= PREDICATE)
            StartCoroutine(WaitToFinish());
    }

    private void SaveCurrentLine()
    {
        GameObject savedLineObject = new("SavedLine");
        LineRenderer savedLineRenderer = savedLineObject.AddComponent<LineRenderer>();

        CopyLineRendererProperties(savedLineRenderer, _savedLineRendererPrefab);

        savedLineRenderer.positionCount = _currentLine.Count;
        savedLineRenderer.SetPositions(_currentLine.ToArray());

        savedLineObject.transform.parent = _drawnLinesContainer.transform;
    }

    private void CopyLineRendererProperties(LineRenderer destination, LineRenderer source)
    {
        destination.widthCurve = source.widthCurve;
        destination.material = source.material;
    }

    private float CompareTextures(Texture2D playerTexture, Texture2D desiredTexture)
    {
        Color[] playerPixels = playerTexture.GetPixels();
        Color[] desiredPixels = desiredTexture.GetPixels();

        float similarity = 0;

        for (int i = 0; i < playerPixels.Length; i++)
        {
            similarity += CalculateColorDistance(playerPixels[i], desiredPixels[i]);
        }

        similarity /= playerPixels.Length;

        return 1 - similarity;
    }

    float CalculateColorDistance(Color color1, Color color2)
    {
        float rDiff = color1.r - color2.r;
        float gDiff = color1.g - color2.g;
        float bDiff = color1.b - color2.b;

        return Mathf.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
    }


    private IEnumerator WaitToFinish()
    {
        yield return new WaitForSeconds(0.4f);
        Task.Instance.ForcefullyStop(TaskStarter.Availability.Done);
    }
}