using UnityEngine;
using UnityEngine.InputSystem;

public class Menus : MonoBehaviour
{
    [SerializeField] private GameObject _startIcon;
    [SerializeField] private Collider2D _colliderToDestroy;
    
    [SerializeField] private InputActionReference _inputAction;
    [SerializeField] private InputActionReference _inputPause;
    
    [SerializeField] private Animator _animatorCurtains;

    private bool _canStart;
    private bool _canPause;
    
    void OnEnable()
    {
        _inputAction.action.Enable();
        _inputPause.action.Enable();
    }
    
    void Start()
    {
        _canStart = true;
        _canPause = false;
    }
    
    void Update()
    {
        StartGame();

        if (_canPause)
            PauseGame();

        if (!_canStart && _canPause)
        {
            ResumeGame();
        }
    }

    public void StartGame()
    {
        if (_canStart == true && _inputAction.action.WasPressedThisFrame())
        {
            _animatorCurtains.SetTrigger("Opening");
            _canPause = true;
            _colliderToDestroy.enabled = false;
        }
            
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _startIcon.SetActive(true);
            _canStart = true;
            Debug.Log("dedans");
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _startIcon.SetActive(false);
            _canStart = false;
            Debug.Log("pas dedans");
        }
    }

    public void PauseGame()
    {
        if (_inputPause.action.WasPressedThisFrame())
        {
            _animatorCurtains.SetTrigger("Closing");
            _canPause = false;
        }
    }

    public void ResumeGame()
    {
        if (_inputPause.action.WasPressedThisFrame())
        {
            _animatorCurtains.SetTrigger("Opening");
            _canPause = true;
        }
    }
}
