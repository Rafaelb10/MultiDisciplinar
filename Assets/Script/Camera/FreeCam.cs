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

    [Header("Y Limits")]
    [SerializeField] private float maxY = 1.75f;

    [SerializeField] private GameObject _fpCam;
    [SerializeField] private GameObject _freeCam;
    [SerializeField] private GameObject _fpCam;

    private float yaw;
    private float pitch;
    void Start()
    {
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = e.x;

        Camera.main.nearClipPlane = 0;  
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _fpCam.SetActive(true);
            _freeCam.SetActive(false);
        }
        CameraMovement();
    }

    private void LateUpdate()
    {
        MouseRotation();

    }
    private void MouseRotation()
    {
        // read raw mouse delta
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * rotationSpeed;
        pitch -= my * rotationSpeed;
        pitch = Mathf.Clamp(pitch, -89f, +89f);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
    private void CameraMovement()
    {
        // horizontal (WASD)
        Vector3 horizontal = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) horizontal += transform.forward;
        if (Input.GetKey(KeyCode.S)) horizontal -= transform.forward;
        if (Input.GetKey(KeyCode.A)) horizontal -= transform.right;
        if (Input.GetKey(KeyCode.D)) horizontal += transform.right;
        if (horizontal.sqrMagnitude > 1f) horizontal.Normalize();

        // vertical (Space / Shift)
        Vector3 vertical = Vector3.zero;
        if (Input.GetKey(KeyCode.Space)) vertical += Vector3.up;
        if (Input.GetKey(KeyCode.LeftShift)) vertical -= Vector3.up;

        Vector3 delta = horizontal * moveSpeed * Time.deltaTime + vertical * verticalSpeed * Time.deltaTime;

        //Limits the camera to the Y axis
        Vector3 newPos = transform.position + delta;
        newPos.y = Mathf.Max(newPos.y, maxY);
        transform.position = newPos;
    }
}