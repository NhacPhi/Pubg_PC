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
    public float JumpSpeed = 5.0f;
    public float ThrowingFore = 100f;

    // Audio
    // Private properties
    float m_VerticalSpeed = 0.0f;
    bool m_IsPaused = false;
    float m_VerticalAngle, m_HorizontalAngle;

    // Check player in ground
    bool m_Grounded;
    float m_GroundTimer;
    float m_SpeedAtJump = 0.0f;

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
        m_IsPaused = false;
        m_Grounded = true;

        // Movement 
        m_VerticalAngle = 0.0f;
        m_HorizontalAngle = transform.localEulerAngles.y;

        m_CharacterController = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement of player;

        // check player in Ground use CharacterController
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    GameObject boom = Instantiate(Boom, transform.position + transform.forward, Quaternion.identity);
        //    Rigidbody rg = boom.GetComponent<Rigidbody>();
        //    //Vector3 dir = transform.position - Camera.main.ViewportToWorldPoint(Input.mousePosition);    
        //    Vector3 dir = (transform.forward +new Vector3(0,transform.eulerAngles.y,0));
        //    Debug.Log("Euler Angle : " + transform.eulerAngles.y/180 * Mathf.Rad2Deg);
        //    Debug.Log("Angle :" + Mathf.Rad2Deg);
        //    rg.AddForce(dir * ThrowingFore);
        //}
        if(!m_CharacterController.isGrounded)
        {
            // Handle charactor controller juimp after 0.5s
        }
        else
        {
            m_Grounded = true;
            //Debug.Log("Jump : true");
        }

        Vector3 move = Vector3.zero;
        if(!m_IsPaused)
        {
            // Jump
            if(m_Grounded && Input.GetButtonDown("Jump"))
            {
                Debug.Log("Jump :>");

                m_VerticalSpeed = JumpSpeed;
                m_Grounded = false;
            }
        }

        // Handle run when player bring the Gun

        // movement player by KeyBoard
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // check quare lenght of vector move > 0 get Nomalize of vector move ( Nomalize always has quare leght = 1 ) but keep direction
        if (move.sqrMagnitude > 0)
        {
            move.Normalize();
            Weapon.Walk = true;
        }
        else
        {
            Weapon.Walk = false;
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
