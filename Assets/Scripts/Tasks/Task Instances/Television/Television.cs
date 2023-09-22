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
        SetProgrammeTo(0);
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
    public void SetProgrammeTo(int programmeId) 
    {
        AudioSource source = AudioManager.Source.transform.GetChild(0).GetComponent<AudioSource>();
        switch (programmeId)
        {
            case 2:
                source.clip = AudioManager.Clips["TV2"];
                source.volume = 1f;
                break;
            case 3:
                source.clip = AudioManager.Clips["TV3"];
                source.volume = 1f;
                break;
            default:
                source.clip = AudioManager.Clips["TV0"];
                source.volume = 0.15f;
                break;
        }
        source.Play();
        SetProgrammeTo((Programmes)programmeId);
    }
    public void NextChannel()
    {
        if((int)_currentProgramName < 3)
            SetProgrammeTo((int)_currentProgramName + 1);
    }
    public void PrevChannel()
    {
        if ((int)_currentProgramName > 0)
            SetProgrammeTo((int)_currentProgramName - 1);
    }
    private void Update()
    {
        if (_currentProgramName == Programmes.None)
            PointManager.StressPoints -= Time.deltaTime / 2f;
        else if (_currentProgramName == Programmes.News)
            PointManager.StressPoints += Time.deltaTime;
        else if (_currentProgramName == Programmes.Soccer)
        {
            PointManager.StressPoints -= Time.deltaTime;
        }
        else
        {
            PointManager.ChildPoints += Time.deltaTime / 2f;
            PointManager.StressPoints -= Time.deltaTime / 2f;
        }
    }
}
