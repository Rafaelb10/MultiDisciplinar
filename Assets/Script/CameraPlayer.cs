using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPlayer : MonoBehaviour
{
    private IInterectable _lastInteracted;
    private bool _isInteracting = false;

    [SerializeField] private InputActionReference click;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        if (_isInteracting == false)
        {
            IInterectable hitObject = null;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 20))
            {
                hitObject = hit.transform.GetComponent<IInterectable>();
            }

            if (hitObject != _lastInteracted)
            {
                _lastInteracted?.ResetColor();
                hitObject?.PossibleToInterect();

                _lastInteracted = hitObject;
            }
        }

    }

    private void OnEnable()
    {
        if (click != null)
        {
            click.action.performed += OnClick;
            click.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (click != null)
        {
            click.action.performed -= OnClick;
            click.action.Disable();
        }
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Interact();
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 20))
        {
            IInterectable Iteractable = hit.transform.GetComponent<IInterectable>();

            if (Iteractable != null)
            {
                _isInteracting = true;
                Iteractable.Interect();
            }
        }
    }

    public void StopInterect()
    {
        _isInteracting = false;
    }
}

