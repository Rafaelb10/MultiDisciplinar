using UnityEngine;

public class FreeCam : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float sprintMultiplier = 2f;
    public float verticalSpeed = 5f;

    [Header("Mouse Look")]
    public float lookSpeed = 2f;
    public float maxYAngle = 80f;
    public float minYAngle = -80f;

    private float yaw = 0f;
    private float pitch = 0f;

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
            _fpCam.SetActive(true);
            _freeCam.SetActive(false);
        }

        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, minYAngle, maxYAngle);
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        // Movement
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed *= sprintMultiplier;

        Vector3 direction = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        Vector3 move = transform.TransformDirection(direction) * speed * Time.deltaTime;

        // Vertical movement
        if (Input.GetKey(KeyCode.Space)) move.y += verticalSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftControl)) move.y -= verticalSpeed * Time.deltaTime;

        transform.position += move;

        // Unlock mouse
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}