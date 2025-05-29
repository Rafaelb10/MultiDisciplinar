using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPlayer : MonoBehaviour
{
    private IInterectable _lastInteracted;
    private Card _lastCard;
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
            Card hitObject2 = null;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 200))
            {
                hitObject = hit.transform.GetComponent<IInterectable>();
                hitObject2 = hit.transform.GetComponent<Card>();
            }

            _lastCard = hitObject2;
            _lastInteracted = hitObject;
       

        if(_lastInteracted!=null)
        {
            uiManager.UpdateInteract(true,_lastInteracted.InteractName);
        }
        else
        {
            uiManager.UpdateInteract(false);
        }

        if (_lastCard != null)
        {
            uiManager.ViewCard(true, _lastCard.Data.Life, _lastCard.Data.Attack, _lastCard.Data.Description, _lastCard.Data.Sprite);
        }
        else
        {
            uiManager.ViewCard(false, 0, 0, "", null);
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

