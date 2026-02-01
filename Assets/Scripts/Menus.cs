using System.Collections;
using Timeline;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject _startIcon;
    [SerializeField] private Collider2D _colliderToDestroy;
    [SerializeField] private GameObject _narratorVisual;
    
    [SerializeField] private InputActionReference _inputAction;
    [SerializeField] private InputActionReference _inputPause;
    
    [SerializeField] private Animator _animatorCurtains;
    [SerializeField] private Animator _fade;
    [SerializeField] private float _fadeDuration = 1.5f;
    [SerializeField] private GameObject _fadeGO;
    
    [SerializeField] private DialogPartData[] dialogParts;

    private bool _canStart;
    private bool _canPause;
    private bool _isPaused;
    private bool _isTransitioning;
    private bool _introStarted;
    private bool _playerInStartZone;
    
    void OnEnable()
    {
        _inputAction.action.Enable();
        _inputPause.action.Enable();
    }
    
    void Start()
    {
        _canStart = false;
        _canPause = false;
        _introStarted = false;
        _playerInStartZone = false;

        _narratorVisual.SetActive(false);

        _fadeGO.SetActive(true);
        StartCoroutine(StartFadeOut());
    }
    
    void Update()
    {
        if (_introStarted && !_canPause)
            return;
        
        StartGame();

        if (_inputPause.action.WasPressedThisFrame() && !_isTransitioning && _canPause)
        {
            if (_isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void StartGame()
    {
        if (_introStarted)
            return;

        if (_playerInStartZone && _canStart && _inputAction.action.WasPressedThisFrame())
        {
            _introStarted = true;
            _canStart = false;

            _startIcon.SetActive(false);
            _narratorVisual.SetActive(true);

            StartCoroutine(IntroSequence());
            AudioManager.PlayLoopingPeople(SoundType.LoopPeople);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_introStarted)
        {
            _startIcon.SetActive(true);
            _canStart = true;
            _playerInStartZone = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _startIcon.SetActive(false);
            _canStart = false;
            _playerInStartZone = false;
        }
    }

    void PauseGame()
    {
        _isTransitioning = true;
        _animatorCurtains.SetTrigger("Closing");
        StartCoroutine(PauseAfterCurtains());
    }

    void ResumeGame()
    {
        _isTransitioning = true;
        Time.timeScale = 1f;
        _animatorCurtains.SetTrigger("Opening");
        StartCoroutine(ResumeAfterCurtains());
    }
    IEnumerator PauseAfterCurtains()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0f;
        _isPaused = true;
        _isTransitioning = false;
    }

    IEnumerator ResumeAfterCurtains()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        _isPaused = false;
        _isTransitioning = false;
    }

    private IEnumerator IntroSequence()
    {
        DialogManager.instance.PlayDialog(dialogParts[0]);
        yield return new WaitForSeconds(3.5f);
        DialogManager.instance.PlayDialog(dialogParts[1]);
        yield return new WaitForSeconds(3.5f);
        DialogManager.instance.PlayDialog(dialogParts[2]);
        yield return new WaitForSeconds(3.5f);
        DialogManager.instance.PlayDialog(dialogParts[3]);
        yield return new WaitForSeconds(3.5f);
        DialogManager.instance.PlayDialog(dialogParts[4]);
        yield return new WaitForSeconds(3.5f);
        DialogManager.instance.PlayDialog(dialogParts[5]);
        yield return new WaitForSeconds(3.5f);
        _animatorCurtains.SetTrigger("Opening");
        _canPause = true;
        _colliderToDestroy.enabled = false;
        AudioManager.PlayOneShot(SoundType.Corde);
        yield return new WaitForSeconds(5f);
        TimelineHandler.instance.StartTimeline();
    }
    
    private IEnumerator StartFadeOut()
    {
        yield return new WaitForSeconds(1f);

        _fade.SetTrigger("FadeOut");

        yield return new WaitForSecondsRealtime(_fadeDuration);

        _fadeGO.SetActive(false);
    }
}
