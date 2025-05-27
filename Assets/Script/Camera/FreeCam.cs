using UnityEngine;

public class FreeCam : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [SerializeField] private GameObject _fpCam;
    [SerializeField] private GameObject _freeCam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S))
            moveDirection -= transform.forward;
        if (Input.GetKey(KeyCode.A))
            moveDirection -= transform.right;
        if (Input.GetKey(KeyCode.D))
            moveDirection += transform.right;

        if (Input.GetKey(KeyCode.Space))
            moveDirection += transform.up;
        if (Input.GetKey(KeyCode.LeftShift))
            moveDirection -= transform.up;
        if (Input.GetKey(KeyCode.E))
        {
            _fpCam.SetActive(true);
            _freeCam.SetActive(false);
        }

        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;

        Vector3 flatMove = new Vector3(moveDirection.x, 0f, moveDirection.z);
        if (flatMove != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatMove);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
