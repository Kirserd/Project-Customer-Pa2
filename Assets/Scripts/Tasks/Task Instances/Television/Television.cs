using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Programmes
{
    None = 0,
    News = 1,
    Soccer = 2,
    MLP = 3
}
[RequireComponent(typeof(SpriteRenderer))]
public class Television : MonoBehaviour
{
    [Serializable]
    private class Programme
    {
        public Programmes Name;
        public Sprite Image;
        public float ChildPoints;
        public float AntiStressPoints;
        public int DurationInMinutes;
    }

    [SerializeField]
    private List<Programme> _programmes;

    private Programmes _currentProgramName;
    private Programme _currentProgram;

    private SpriteRenderer _renderer;

    private void Start()
    {
        SortProgrammes();
        RefreshComponents();
    }
    private void RefreshComponents() => _renderer = GetComponent<SpriteRenderer>();
    private void SortProgrammes() => _programmes = _programmes.OrderBy(programme => (int)programme.Name).ToList();

    public void SetProgrammeTo(Programmes programme)
    {
        if (_currentProgramName == programme)
            return;

        _currentProgramName = programme;
        _currentProgram = _programmes[(int)programme];
        _renderer.sprite = _currentProgram.Image;
    }
    public void SetProgrammeTo(int programmeId) => SetProgrammeTo((Programmes)programmeId);

    public void StartProgramme()
    {
        StartCoroutine(PlayProgramme());
    }

    private IEnumerator PlayProgramme()
    {
        yield return null;
    }
}
