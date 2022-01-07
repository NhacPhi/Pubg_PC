using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// Setting Unity Editor use Marcro in C#
//#if UNITY_EDITOR
using UnityEditor;
//#endif

public class Controller : MonoBehaviour
{
    public Controller Instance { get;protected set; }

    public Camera MainCamera;

    public Transform CameraPosition;

    public GameObject Boom;

    private CharacterController m_CharacterController;

    //Control settings
    public float MouseSentitivity = 60f;
    public float PlayerSpeed = 5.0f;
    public float RuningSpeed = 7.0f;
    public float ThrowingFore = 100f;



    // Audio
    // Private properties
    float m_VerticalSpeed = 0.0f;
    bool m_IsPaused = false;
    float m_VerticalAngle, m_HorizontalAngle;

    // Check player in ground
    public float m_SpeedAtJump = 1.2f;
    float m_GroundedTimer;
    float jumpSpeed = 5f;
    bool Grounded;

    float VelocityVetical = 0;

    // Some function use of Instance is called other clase
    public float Speed { get; private set; } = 0.0f;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // Lock mouse in screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Setting Camera when player start
        MainCamera.transform.SetParent(CameraPosition, false);
        MainCamera.transform.localPosition = Vector3.zero;
        MainCamera.transform.rotation = Quaternion.identity;

        // Ground
        Grounded = true;

        // Movement 
        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;

        m_CharacterController = GetComponent<CharacterController>();
        m_CharacterController.detectCollisions = false;

    }

    // Update is called once per frame
    void Update()
    {
        bool wasGrounded = Grounded;

        bool loosedGrounding = false;
        if (!m_CharacterController.isGrounded)
        {
            if (!Grounded)
            {
                m_GroundedTimer += Time.deltaTime;
                if (m_GroundedTimer >= 0.5f)
                {
                    Debug.Log("LOL");
                    loosedGrounding = true;
                    Grounded = false;
                }
            }
        }
        else
        {
            m_GroundedTimer = 0.0f;
            Grounded = true;
        }

        if (Grounded && Input.GetKeyDown(KeyCode.Space))
        {
            VelocityVetical = jumpSpeed;
            Debug.Log("Jump");
            Grounded = false;
        }
        Vector3 move = Vector3.zero;
       
        // Handle run when player bring the Gun

        // movement player by KeyBoard
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // check quare lenght of vector move > 0 get Nomalize of vector move ( Nomalize always has quare leght = 1 ) but keep direction
        if (move.sqrMagnitude > 0)
        {
            move.Normalize();
            //Weapon.Walk = true;
        }
        else
        {
            //Weapon.Walk = false;
        }
            
        if(Input.GetKey(KeyCode.LeftShift))
        {
            move = move * Time.deltaTime * RuningSpeed;
        }
        else
        {
            move = move * Time.deltaTime * PlayerSpeed;
        }


        move = transform.TransformDirection(move);
        m_CharacterController.Move(move);

        // handle gravity
        VelocityVetical = VelocityVetical - 9.8f * Time.deltaTime;
        if (VelocityVetical < -9.8f)
        {
            VelocityVetical = -9.8f;
        }
        Vector3 verticalSpeed = new Vector3(0, VelocityVetical * Time.deltaTime, 0);
        m_CharacterController.Move(verticalSpeed);

        // Player rotation by Mouse

        // Axis Horizontal with CharactorRoot
        float turnPlayer = Input.GetAxis("Mouse X") * MouseSentitivity;
        m_HorizontalAngle = m_HorizontalAngle + turnPlayer;

        if (m_HorizontalAngle > 360) m_HorizontalAngle -= 360.0f;
        if (m_HorizontalAngle <0) m_HorizontalAngle += 360.0f;

        Vector3 currentAngles = transform.localEulerAngles;
        currentAngles.y = m_HorizontalAngle;
        transform.localEulerAngles = currentAngles;


        // Axis Vertical with Camera look up or down

        float turnCam = -Input.GetAxis("Mouse Y");
        turnCam = turnCam * MouseSentitivity;
        m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle, -89.0f, 89.0f);
        currentAngles = CameraPosition.transform.localEulerAngles;

        currentAngles.x = m_VerticalAngle;
        CameraPosition.localEulerAngles = currentAngles;
    }
}
