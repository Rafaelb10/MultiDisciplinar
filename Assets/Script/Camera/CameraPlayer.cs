using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPlayer : MonoBehaviour
{
    private IInterectable _lastInteracted;
    [SerializeField] private UiManager uiManager;

    [SerializeField] private InputActionReference click;
    [SerializeField] private GameObject _freeCam;
    [SerializeField] private GameObject _fpCam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _freeCam.SetActive(true);
            _fpCam.SetActive(false);
        }



            IInterectable hitObject = null;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 20))
            {
                hitObject = hit.transform.GetComponent<IInterectable>();
            }

            if (hitObject != _lastInteracted)
            {
                _lastInteracted?.ResetColor();
                hitObject?.PossibleToInterect();
            }

            _lastInteracted = hitObject;
       

        if(_lastInteracted!=null)
        {
            uiManager.UpdateInteract(true,_lastInteracted.InteractName);
        }
        else
        {
            uiManager.UpdateInteract(false);
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
        if (_lastInteracted!=null)
        {
            _lastInteracted.Interact();
            _lastInteracted = null;
        }
    }
}

