using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private Animator _dialogueBoxAnimator;
    [SerializeField] private AudioMixerGroup _voiceOverMixerGroup;

    private Dialogue _currentDialogue;
    private int _currentPage;
    private bool _isTyping;
    private AudioSource _voiceOverAudioSource;

    [Header("Text Animation Settings")]
    [SerializeField] private float _textSpeed = 0.05f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        _voiceOverAudioSource = gameObject.AddComponent<AudioSource>();
        _voiceOverAudioSource.outputAudioMixerGroup = _voiceOverMixerGroup;
    }

    private void Subscribe()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.Interact] += SkipText;
        InputSubscriber.InputEvents[(int)BoundKeys.LeftClick] += SkipText;
        InputSubscriber.InputEvents[(int)BoundKeys.RightClick] += SkipText;
        InputSubscriber.InputEvents[(int)BoundKeys.Enter] += SkipText;
    }

    private void Unsubscribe()
    {
        InputSubscriber.InputEvents[(int)BoundKeys.Interact] -= SkipText;
        InputSubscriber.InputEvents[(int)BoundKeys.LeftClick] -= SkipText;
        InputSubscriber.InputEvents[(int)BoundKeys.RightClick] -= SkipText;
        InputSubscriber.InputEvents[(int)BoundKeys.Enter] -= SkipText;
    }

    public void StartDialogue(Dialogue dialogue)
    {
        _currentDialogue = dialogue;
        _currentPage = 0;
        _dialogueBoxAnimator.SetTrigger("FadeIn");
        DisplayNextPage();
        Subscribe();
    }

    public void SkipText(ButtonState state)
    {
        if (state == ButtonState.Hold)
            return;

        if (_isTyping)
        {
            StopAllCoroutines();
            _dialogueText.text = _currentDialogue.Pages[_currentPage].Text;
            _isTyping = false;
        }
        else
        {
            if (_voiceOverAudioSource.isPlaying)
                _voiceOverAudioSource.Stop();

            DisplayNextPage();
        }
    }

    private void DisplayNextPage()
    {
        if (_currentPage < _currentDialogue.Pages.Count)
        {
            StopAllCoroutines();
            StartCoroutine(TypeText(_currentDialogue.Pages[_currentPage]));

            if (_currentDialogue.Pages[_currentPage].VoiceOver != null)
            {
                _voiceOverAudioSource.clip = _currentDialogue.Pages[_currentPage].VoiceOver;
                _voiceOverAudioSource.Play();
            }
        }
        else
            EndDialogue();
    }

    private IEnumerator TypeText(Dialogue.DialoguePage page)
    {
        _nameText.text = page.CharacterName;
        _dialogueText.text = "";
        _isTyping = true;

        foreach (char letter in page.Text)
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_textSpeed);
        }

        _isTyping = false;
    }

    private void EndDialogue()
    {
        Unsubscribe();
        _dialogueBoxAnimator.SetTrigger("FadeOut");
        _currentDialogue = null;
        _currentPage = 0;
        _nameText.text = "";
        _dialogueText.text = "";
    }
}
