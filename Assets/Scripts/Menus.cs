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
    
    void Start()
    {
        _canStart = false;
        _canPause = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _startIcon.SetActive(true);
            _canStart = true;
        }
        else
        {
            _startIcon.SetActive(false);
            _canStart = false;
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
