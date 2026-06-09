using UnityEngine;

public class BetterPlayerController : MonoBehaviour
{
    [Header("References")] // creates a "group" in Unity's Inspector
    public Transform cameraContainer;

    [Header("Move (Forces)")]
    public float moveStregth = 10f;
    public float groundDrag = 5f; // simulates friction with the floor

    [Header("Camera Rotation")]
    public float mouseSensibility = 2f;
    public float camMinAngle = -40f; // clamp the lower camera angle
    public float camMaxAngle = 70f; // clamp the upper camera angle

    [Header("Ground Detection")]
    public LayerMask groundLayer; // select the Layer in Inspector
    public float radiusDistance = 0.1f; // this is the minimum distance from the floor for the player is considered "in ground"

    // Control private variables:
    private Rigidbody rb;
    private float cameraRotationX = 0f;
    private bool isOnFloor;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // finds the Rigidbody component inside this object (Player)

        // Lock the mouse cursor inside the game screen and hide it:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 1. Rotation (at Update to be smooth)
        RotateCharacterAndCamera();

        // 2. Test collision with the ground
        CheckGround();
    }

    // Fixed Update is called at a fixed rate of FPS "paired" with unity's physics phase
    private void FixedUpdate()
    {
        // 3. Movement (always on FixedUpdate when using Rigidbody/forces)
        ApplyMoveForces();
    }

    private void RotateCharacterAndCamera()
    {
        // Get mouse movement
        float mouse_x = Input.GetAxis("Mouse X") * mouseSensibility;
        float mouse_y = Input.GetAxis("Mouse Y") * mouseSensibility;

        // turns the CHARACTER in Y axis (left/right)
        // uses Quaternion.Euler to apply the incremental rotation
        Vector3 charRotation = new Vector3(0f, mouse_x, 0f);
        transform.Rotate(charRotation);

        // turns the CAMERA CONTAINER on X axis (UP/DOWN)
        // We subtract the mouse_y so the control is not reversed/inverted
        cameraRotationX -= mouse_y;

        // Clamps the camera rotation limits so it doesnt go upside down
        cameraRotationX = Mathf.Clamp(cameraRotationX, camMinAngle, camMaxAngle);

        // Apply the local rotation to the camera container
        cameraContainer.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);

    }

    private void CheckGround()
    {

    }

    private void ApplyMoveForces()
    {

    }
}
