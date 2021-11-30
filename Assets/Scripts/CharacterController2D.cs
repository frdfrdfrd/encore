using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
//using UnityEngine.InputSystem;

public enum GroundType
{
    Empty,
    Soft,
    Hard
}

public class CharacterController2D : MonoBehaviour
{
    readonly Vector3 flippedScale = new Vector3(-1, 1, 1);
    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    [Header("Character")]
    [SerializeField] Animator animator = null;
     [SerializeField] public Transform puppet = null;
    public Rigidbody controllerRigidbody;
    public Transform _groundChecker;

    // [Header("Equipment")]
    // [SerializeField] Transform handAnchor = null;
    //[SerializeField] SpriteLibrary spriteLibrary = null;

    [Header("Movement")]
    public bool _canJump = false;
    [SerializeField] float acceleration = 0.0f;
    [SerializeField] float maxSpeed = 0.0f;
    [SerializeField] float jumpForce = 0.0f;
    [SerializeField] float minFlipSpeed = 0.1f;
    //[SerializeField] float jumpGravityScale = 1.0f;
    //[SerializeField] float fallGravityScale = 1.0f;
    [SerializeField] float groundedGravityScale = 1.0f;
    [SerializeField] bool resetSpeedOnLand = false;


    //public Collider controllerCollider;
    //public CharacterController _controller;

    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;

    private Vector2 movementInput;
    private bool jumpInput;

    private Vector2 prevVelocity;
    private GroundType groundType;
    private bool isFlipped;
    private bool isJumping;
    private bool isFalling;

    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;

    public Transform _philac;
    public Transform _switch;
    public GameObject _BGText;

    public bool CanMove { get; set; }

    public Cinemachine.CinemachineVirtualCamera _virtualCameraRef;
    bool _controlledDisabled;
    float currentCamDist;
    public float zoomSpeed = 10;
    public Vector2 _zoomLimit = new Vector2(4, 40);

    void Start()
    {
//#if UNITY_EDITOR
//        if (Keyboard.current == null)
//        {
//            var playerSettings = new UnityEditor.SerializedObject(Resources.FindObjectsOfTypeAll<UnityEditor.PlayerSettings>()[0]);
//            var newInputSystemProperty = playerSettings.FindProperty("enableNativePlatformBackendsForNewInputSystem");
//            bool newInputSystemEnabled = newInputSystemProperty != null ? newInputSystemProperty.boolValue : false;

//            if (newInputSystemEnabled)
//            {
//                var msg = "New Input System backend is enabled but it requires you to restart Unity, otherwise the player controls won't work. Do you want to restart now?";
//                if (UnityEditor.EditorUtility.DisplayDialog("Warning", msg, "Yes", "No"))
//                {
//                    UnityEditor.EditorApplication.ExitPlaymode();
//                    var dataPath = Application.dataPath;
//                    var projectPath = dataPath.Substring(0, dataPath.Length - 7);
//                    UnityEditor.EditorApplication.OpenProject(projectPath);
//                }
//            }
//        }
//#endif


        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");

        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");

        puppet.localScale = flippedScale;
       // _philac.localScale = flippedScale;
        _switch.localScale = flippedScale;

        CanMove = true;

        _BGText.SetActive(false);
        _philac.gameObject.SetActive(false);

        _controlledDisabled = false;
        if(_virtualCameraRef) currentCamDist = _virtualCameraRef.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>().m_CameraDistance;
    }

    public void Freeze(bool freeze)
    {
        CanMove = !freeze;
        controllerRigidbody.useGravity = !freeze;

        if (freeze)
        {
            controllerRigidbody.velocity = Vector3.zero;
            controllerRigidbody.angularVelocity = Vector3.zero;
            controllerRigidbody.mass = 100;
            prevVelocity = Vector2.zero;
         }
        else
        {
            controllerRigidbody.mass = 1;
        }
    }

    void Update()
    {
        if(_controlledDisabled)
        {
            float moveVertical = 0.0f;
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                moveVertical = 1.0f;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                moveVertical = -1.0f;

            if(moveVertical != 0.0f) ControlCamZoom(moveVertical);
        }
        else
        {
            if (!CanMove)
            {
                prevVelocity = new Vector2(0, 0);
                movementInput = new Vector2(0, 0);
                return;
            }
                

            // Horizontal movement
            float moveHorizontal = 0.0f;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A))
                moveHorizontal = -1.0f;
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                moveHorizontal = 1.0f;

            movementInput = new Vector2(moveHorizontal, 0);
        }


        

        // Jumping input
        if (_canJump && !isJumping && Input.GetKeyDown(KeyCode.Space)) jumpInput = true;
    }

    void FixedUpdate()
    {
        UpdateGrounding();
        UpdateVelocity();
        UpdateDirection();
        UpdateJump();
        //UpdateGravityScale();

        if(controllerRigidbody != null) prevVelocity = controllerRigidbody.velocity;
    }

    private void UpdateGrounding()
    {
        //if (controllerCollider == null) return;
        // Debug.Log(_controller.isGrounded);

        int layerMask = 1 << 3;
        // This would cast rays only against colliders in layer 3.
        // But instead we want to collide against everything except layer 3. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        bool groundcheck = Physics.Raycast(_groundChecker.transform.position + new Vector3(0, 0.1f, 0), new Vector3(0, -1, 0), 0.2f, layerMask);
        //Debug.DrawRay(_groundChecker.transform.position, new Vector3(0, -1, 0));
        //Debug.Log(groundcheck);

        //Use character collider to check if touching ground layers
        if (groundcheck/*_controller.isGrounded*/ /*controllerCollider.IsTouchingLayers(softGroundMask)*/)
            groundType = GroundType.Hard;
        else
            groundType = GroundType.Empty;

        // Update animator
        animator.SetBool(animatorGroundedBool,/* controllerRigidbody.velocity.y == 0*/groundType != GroundType.Empty);
    }

    private void UpdateVelocity()
    {
        if (controllerRigidbody == null) return;

        Vector2 velocity = controllerRigidbody.velocity;

        // Apply acceleration directly as we'll want to clamp
        // prior to assigning back to the body.
        velocity += CanMove ? movementInput * acceleration * Time.fixedDeltaTime : Vector2.zero;

        // We've consumed the movement, reset it.
        movementInput = Vector2.zero;

        // Clamp horizontal speed.
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        // Assign back to the body.
        controllerRigidbody.velocity = velocity;

        // Update animator running speed
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / maxSpeed;
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);

        // Play audio
        //audioPlayer.PlaySteps(groundType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && controllerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (jumpInput && groundType != GroundType.Empty)
        {
            // Jump using impulse force
            controllerRigidbody.AddForce(new Vector3(0, jumpForce,0), ForceMode.Impulse);

            // Set animator
            animator.SetTrigger(animatorJumpTrigger);

            // We've consumed the jump, reset it.
            jumpInput = false;

            // Set jumping flag
            isJumping = true;

            // Play audio
           // audioPlayer.PlayJump();
        }

        // Landed
        else if (isJumping && isFalling && groundType != GroundType.Empty)
        {
            // Since collision with ground stops rigidbody, reset velocity
            if (resetSpeedOnLand)
            {
                prevVelocity.y = controllerRigidbody.velocity.y;
                controllerRigidbody.velocity = prevVelocity;
            }

            // Reset jumping flags
            isJumping = false;
            isFalling = false;

            // Play audio
            //audioPlayer.PlayLanding(groundType);
        }
    }

    private void UpdateDirection()
    {
        if (controllerRigidbody == null) return;

          // Use scale to flip character depending on direction
        if (controllerRigidbody.velocity.x > minFlipSpeed && isFlipped)
        {
            isFlipped = false;
            puppet.localScale = flippedScale;
            //_philac.localScale = flippedScale;
            _switch.localScale = flippedScale;
        }
        else if (controllerRigidbody.velocity.x < -minFlipSpeed && !isFlipped)
        {
            isFlipped = true;
            puppet.localScale = Vector3.one;
            //_philac.localScale = Vector3.one;
            _switch.localScale = Vector3.one;
        }
    }


    private void UpdateGravityScale()
    {
        // Use grounded gravity scale by default.
        //var gravityScale = groundedGravityScale;

        //if (controllerRigidbody == null) return;

        //if (groundType == GroundType.None)
        //{
        //    // If not grounded then set the gravity scale according to upwards (jump) or downwards (falling) motion.
        //    gravityScale = controllerRigidbody.velocity.y > 0.0f ? jumpGravityScale : fallGravityScale;           
        //}

        //controllerRigidbody.gravityScale = gravityScale;
    }

    //public void GrabItem(Transform item)
    //{
    //    // Attach item to hand
    //    item.SetParent(handAnchor, false);
    //    item.localPosition = Vector3.zero;
    //    item.localRotation = Quaternion.identity;
    //}

    //public void SwapSprites(SpriteLibraryAsset spriteLibraryAsset)
    //{
    //    spriteLibrary.spriteLibraryAsset = spriteLibraryAsset;
    //}


    public void HideAndKillControls()
    {
        _controlledDisabled = true;
       puppet.GetComponent<Renderer>().enabled = false;
    }

    void ControlCamZoom(float vertical)
    {
        float dist = currentCamDist + vertical * Time.deltaTime * zoomSpeed;

        currentCamDist = Mathf.Clamp(dist, _zoomLimit.x, _zoomLimit.y);

        _virtualCameraRef.GetCinemachineComponent<Cinemachine.CinemachineFramingTransposer>().m_CameraDistance = currentCamDist;
    }
}
