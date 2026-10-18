using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerStateManager stateManager;
    public GameObject cam;
    private Camera camCamera;

    //adjustables
    [SerializeField] float yawLookSensitivity;
    [SerializeField] float pitchLookSensitivity;

    [SerializeField] float maxLookPitch;

    [SerializeField] float maxSlopeAngle;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] float downForceWhileInAir;

    [SerializeField] float Bouncyness;
    //

    //movment vectors
    private Vector3 targetDir;
    private Quaternion lookRot;

    private float xLookAngle;
    private float yLookAngle;

    private float movement;
    //

    //inputs
    [SerializeField] InputActionAsset inputActions;

    private InputAction move;
    private InputAction jump;
    private InputAction look;
    private InputAction sprint;
    //

    
    //capsule colider stuff
    private Vector3 heightFromCent;
    //

    [SerializeField] GameObject[] objs;

    //interactions//
    private Interactor interactor;
    //
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cam = transform.Find("Camera").gameObject;
        stateManager = GetComponent<PlayerStateManager>();
        interactor = this.GetComponent<Interactor>();
        if (stateManager == null) stateManager = gameObject.AddComponent<PlayerStateManager>();

        if(inputActions == null) this.enabled = false;

        move = inputActions.FindAction("Player/Move");
        jump = inputActions.FindAction("Player/Jump");
        look = inputActions.FindAction("Player/Look");
        sprint = inputActions.FindAction("Player/Sprint");

        inputActions.FindAction("Player/LeftHandInteract").started += _ => interactor.LeftClick();
        inputActions.FindAction("Player/RightHandInteract").started += _ => interactor.RightClick();

        inputActions.FindAction("Player/LeftHandInteract").canceled += _ => interactor.LeftUnClick();
        inputActions.FindAction("Player/RightHandInteract").canceled += _ => interactor.RightUnClick();

        heightFromCent =  Vector3.up * (transform.localScale.y / 4);

        sprint.started += _ => Shift();
        sprint.canceled += _ => ShiftReleased();
        //jump.started += Jump;
        //jump.canceled += Jump;
        jump.started += _ => Spacebar();

        Cursor.lockState = CursorLockMode.Locked;
    }
    float jumpin = 0f;
    private void Jump(InputAction.CallbackContext inputAction)
    {
        if (stateManager.dead) return;
        if (inputAction.ReadValue<float>() > 0f)
        {
            jumpin += jumpHeight * 10f;
        }
        else
        {
            jumpin = 0;
        }
    }
    private void Shift()
    {
        EventBus.RequestEvent("ShiftPressed", true).Invoke();
    }
    private void ShiftReleased()
    {
        EventBus.RequestEvent("ShiftReleased", true).Invoke();
    }
    private void Spacebar()
    {
        EventBus.RequestEvent("SpacebarPressed", true).Invoke();
        
    }

    private Vector3 startPos;
    private void ResetData()
    {
        Debug.Log(transform.position + " | " + startPos);

        rb.position = startPos;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void rotateForLook()
    {
        if (stateManager.dead) return;
        Vector2 lookInput = -look.ReadValue<Vector2>();

        yLookAngle = Mathf.Clamp(yLookAngle + lookInput.y * pitchLookSensitivity, -maxLookPitch, maxLookPitch);
        xLookAngle -= lookInput.x * yawLookSensitivity;
        if (ServiceHub.Instance.gameState.gameplayState == GameState.GameplayState.Menu) xLookAngle = math.clamp(xLookAngle, -40, 40);
        lookRot = Quaternion.Euler(
            yLookAngle,
            xLookAngle,
            0
            );
    }

    private void GetReqMoveDir()
    {
        Vector3 adjustedForward;
        Vector3 adjustedRight;

        float inputted = move.ReadValue<Vector2>().y;

        Quaternion rot = Quaternion.AngleAxis(xLookAngle, stateManager.groundUp);
        Quaternion rot2 = Quaternion.AngleAxis(xLookAngle + 90f, stateManager.groundUp);
        adjustedForward = rot * transform.forward;
        adjustedRight = rot2 * transform.forward;

        adjustedForward.Normalize();
        adjustedRight.Normalize();

        targetDir = (adjustedForward * move.ReadValue<Vector2>().y) + 
        (adjustedRight * move.ReadValue<Vector2>().x);
        targetDir.Normalize();
    }


    //outside vars for memory assignment
    Vector3 sphere1 = Vector3.zero;
    Vector3 sphere2 = Vector3.zero;
    private bool CheckForCollisions(Vector3 dir, out RaycastHit hit)
    {
        sphere1 = transform.position + heightFromCent;
        sphere2 = transform.position - heightFromCent;

        return Physics.CapsuleCast(sphere1 , sphere2 , 0.6f, dir, out hit, walkSpeed * Time.deltaTime);
    }
    private bool CheckForCollisions(Vector3 dir)
    {
        sphere1 = transform.position + heightFromCent;
        sphere2 = transform.position - heightFromCent;

        return Physics.CapsuleCast(sphere1 , sphere2 , 0.6f, dir,walkSpeed * Time.deltaTime);
    }
    private Vector3 AdjustMoveDir(RaycastHit hit)
    {
        Vector3 returnVec = Vector3.ProjectOnPlane(targetDir, hit.normal).normalized;

        float speedLoss = Mathf.Abs((returnVec - targetDir).magnitude) * Bouncyness;

        returnVec *= speedLoss;
        return returnVec;
    }


    void Update()
    {
        rotateForLook();
        cam.transform.rotation = lookRot;
    }



    //
    private float FOV = 90;
    private float zoomInFov = 75;
    private float prog = 0f;
    private float targProg = 0f;

    //
    public void ZoomIn()
    {
        targProg = 1;
    }
    public void ZoomOut()
    {
        targProg = 0;
    }
    void Start()
    {
        camCamera = cam.GetComponent<Camera>();
        ServiceHub.Instance.player = gameObject;

        EventBus.RequestEvent("LevelReseting", true).ping += ResetData;
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        if (stateManager.dead || ServiceHub.Instance.gameState.gameplayState == GameState.GameplayState.Menu || ServiceHub.Instance.gameState.gameplayState == GameState.GameplayState.Paused) return;
        Vector3 ground= stateManager.checkGround(maxSlopeAngle);

        GetReqMoveDir();

        Vector3 newMovePos;
        float velClamper = walkSpeed * Mathf.Clamp01(1f - (rb.linearVelocity.magnitude / walkSpeed));


        if(stateManager.grounded) 
        {
            RaycastHit hit;
            if(CheckForCollisions(targetDir, out hit))
            {
                //targetDir = AdjustMoveDir(hit);
            }

            newMovePos = (targetDir * Time.deltaTime * velClamper * 10000f);

            //rb.AddForce(Vector3.up * jumpin, ForceMode.Impulse); // jumping
            rb.AddForce(newMovePos, ForceMode.Force); //moving 
            
            rb.linearVelocity = new Vector3(rb.linearVelocity.x * 0.95f, rb.linearVelocity.y, rb.linearVelocity.z *0.95f);
        }
        else 
        {
            rb.AddForce(Vector3.down * downForceWhileInAir * 100f);
        }  
    }

    public bool Teleport(Vector3 pos)
    {
        if(Physics.OverlapCapsule(
                pos + Vector3.up * 0.5f,
                pos - Vector3.up * 0.5f,
                0.5f, ~(1 << 2)).Length ==0 )
        {
            transform.position = pos;
            return true;
        }
        return false;
    }
}
