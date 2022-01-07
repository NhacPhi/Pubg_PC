using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTrajectory3DFPS : MonoBehaviour
{
    private LineRenderer lineRender;
    //public Transform cameraFPS;
    [SerializeField]
    private GameObject cannonBall;


    private Rigidbody cannonBallRG;

    private float force = 300;
    private float mass;
    private float fixdeDeltaTime;
    private float velocity;
    private float gravity;
    private float collisionCheckRadius = 0.1f;

    public float MouseSentitivity = 60f;
    float m_VerticalAngle;
    // Start is called before the first frame update
    void Start()
    {
        lineRender = GetComponent<LineRenderer>();
        cannonBallRG = cannonBall.GetComponent<Rigidbody>();
        mass = cannonBallRG.mass;
        lineRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        DrawTrajectory();
        Movements();

        if (Input.GetMouseButton(1))
        {
            lineRender.enabled = true;
        }
        else
        {
            lineRender.enabled = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            GameObject ball = Instantiate(cannonBall, transform.position + transform.up, Quaternion.identity);
            Rigidbody ballRG = ball.GetComponent<Rigidbody>();
            ballRG.AddForce(transform.up * force);
        }
        if (Input.GetMouseButton(1))
        {
            if(force<1000)
            {
                force++;
            }
        }
        else
        {
            force = 600;
        }
    }
    void DrawTrajectory()
    {
        lineRender.positionCount = SimulateArc().Count;

        for (int a = 0; a < lineRender.positionCount; a++)
        {
            lineRender.SetPosition(a, SimulateArc()[a]);
        }
    }
    private List<Vector3> SimulateArc()
    {
        List<Vector3> lineRendererPoints = new List<Vector3>();

        float maxDuration = 5f;
        float timeStepInterval = 0.1f;
        int maxSteps = (int)(maxDuration / timeStepInterval);

        Vector3 directionVector = transform.up;
        Vector3 lunchPosition = transform.position + transform.up;

        velocity = force / mass * Time.fixedDeltaTime;
        for (int i = 0; i < maxSteps; i++)
        {
            Vector3 calculatedPosition = lunchPosition + directionVector * velocity * i * timeStepInterval;
            calculatedPosition.y += Physics.gravity.y / 2 * Mathf.Pow(i * timeStepInterval, 2);
            lineRendererPoints.Add(calculatedPosition);
            if (CheckForCollision(calculatedPosition))
            {
                break;
            }
        }
        return lineRendererPoints;
    }
    private bool CheckForCollision(Vector3 position)
    {
        Collider[] hits = Physics.OverlapSphere(position, collisionCheckRadius);
        if (hits.Length > 0)
        {
            return true;
        }
        return false;
    }
    private void Movements()
    {
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //Vector2 direction = new Vector2(horizontal, vertical);

        //transform.Translate(direction * 5 * Time.deltaTime);

        //if (Input.GetKey(KeyCode.E))
        //{
        //    transform.Rotate(-0.5f, 0, 0);
        //}

        //if (Input.GetKey(KeyCode.Q))
        //{
        //    transform.Rotate(0.5f, 0, 0);
        //}

        // Set thu cong the camera
        //if (cameraFPS.localEulerAngles.x < 0)
        //{
        //    float m_VerticalAngle = Mathf.Clamp(60f - cameraFPS.localEulerAngles.x, 60f, 100f);
        //    transform.localEulerAngles = new Vector3(m_VerticalAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
        //}
        float turnCam = -Input.GetAxis("Mouse Y");
        turnCam = turnCam * MouseSentitivity;
        Vector3 currentAngles = transform.localEulerAngles;
        m_VerticalAngle = Mathf.Clamp(turnCam + m_VerticalAngle,25f,60f);
        currentAngles = transform.localEulerAngles;

        currentAngles.x = m_VerticalAngle;
        transform.localEulerAngles = currentAngles;
    }
   }