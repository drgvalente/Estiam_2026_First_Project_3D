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

    [Header("Shoot")]
    public GameObject bulletPrefab;
    public Transform muzzle;
    public Transform gunContainer;
    public float shootCooldown = 1f; // time between shoots
    public float lastShoot = 0f; // time since our last shoot - a chronometer

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

        // 4. Shooting
        HandleShooting();
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

        // Apply the local rotation to the camera container and gun container
        cameraContainer.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
        gunContainer.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);
    }

    private void CheckGround()
    {
        // Raycast casts an invisible line from the base of character going down
        // If it hits something inside ground layer, returns true
        Vector3 rayOrigin = transform.position; // stores the player position
        isOnFloor = Physics.Raycast(rayOrigin, Vector3.down, radiusDistance + 1f, groundLayer);

        // create a visual Debug for the ray: draws the line in the Scene tab in Unity
        Debug.DrawRay(rayOrigin, Vector3.down * (radiusDistance + 1f), isOnFloor ? Color.green : Color.red);
    }

    private void ApplyMoveForces()
    {
        // get the WASD or ARROWS in keyboard
        float inputHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float inputVertical = Input.GetAxis("Vertical"); // W/S or Up/Down

        // converts the input in the LOCAL axis of the character
        // transform.right = X local axis (left/right of the character)
        // transform.forward = Z local axis (forward/back of the character)
        Vector3 moveDirection = (transform.right * inputHorizontal) + (transform.forward * inputVertical);

        // Normalizes the vextor so the speed is the same in diagonal movements
        moveDirection.Normalize();

        // Apply the force as a continuous impulse
        rb.AddForce(moveDirection * moveStregth, ForceMode.Force);

        // FRICTION: If is on floor and not pressing any key, apply a "drag" (friction)
        // this will make the character stops sliding when player releases the buttons
        if (isOnFloor)
        {
            // if there's no move inputs, slows the character in X and Z axis
            if (inputHorizontal == 0 && inputVertical == 0)
            {
                Vector3 currentSpeed = rb.linearVelocity;
                currentSpeed.x *= (1 - groundDrag * Time.deltaTime);
                currentSpeed.z *= (1 - groundDrag * Time.deltaTime);
                rb.linearVelocity = currentSpeed;
            }
        }
    }

    void Shoot()
    {
        // just instantiate the bullet
        // as the Muzzle already inherit the rotation of the gunContainer
        // the bullet will be created already oriented to the front of the player
        Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
    }

    void HandleShooting()
    {
        lastShoot += Time.deltaTime; // add time in seconds since last frame
        // test if player JUST clicked left mouse button (0)
        if (Input.GetMouseButtonDown(0) && lastShoot >= shootCooldown)
        {
            Shoot();
            lastShoot = 0f; // reset the cooldown
        }
    }
}

