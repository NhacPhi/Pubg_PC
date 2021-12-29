using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Using RaycastHit 

    // Type Shoot
    public enum TriggerType
    {
        Auto,
        Manual
    }
    public enum WeaponState
    {
        Idle,
        Firing,
        Reloading
    }
    class ActiveTrail
    {
        public LineRenderer renderer;
        public Vector3 direction;
        public float remainingTime;
    }
    public float reloadTime = 2.0f;

    private Animator m_Animator;

    private Controller m_Controller;

    bool m_ShotDone;
    float m_ShotTimer = -1.0f;

    public static bool Walk = false;




    // Setting Shoot 
    public int gunDame = 1;
    public float fireRate = 0.25f;
    public float weaponRate = 50f;
    public float hitForce = 100f;
    public Transform gunEnd;

    //public LineRenderer PrefabRayTrail;
    private LineRenderer PrefabRayTrail;

    public Camera fpsCam;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.1f);

    public GameObject prefabGameObject;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();

        m_Controller = GetComponent<Controller>();

        //fpsCam = GetComponentInParent<Camera>();
        PrefabRayTrail = GetComponent<LineRenderer>();

        //if (PrefabRayTrail != null)
        //{
        //    const int trailPoolSize = 16;
        //    //PoolSystem.Instance.InitPool(PrefabRayTrail, trailPoolSize);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        ControlGun();
    }
    void ControlGun()
    {
        if(Walk)
        {
            // walk
            m_Animator.SetFloat("Speed", 1);
        }
        else
        {
            m_Animator.SetFloat("Speed", 0);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            // Reload gun.
            Debug.Log("Reload Gun");
            m_Animator.SetTrigger("Reload");
        }
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot Gun");
            m_Animator.SetTrigger("Shoot");
            RaycastShot();
        }
    }
    void RaycastShot()
    {
        //Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        StartCoroutine(ShotEffect());
        //RaycastHit hit;
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        RaycastHit hit;
        PrefabRayTrail.SetPosition(0, gunEnd.position);
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, weaponRate))
        {
            PrefabRayTrail.SetPosition(1, hit.point);

            //Transform point = hit.collider.GetComponent<Transform>();



            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemies"))
            {
                hit.transform.GetComponent<Target>().GetDamge(2,hit.point);
                Debug.Log("Hit");
            }
            else
            {
                Vector3 point = hit.point;

                //Debug.Log("Transfrom Point :" + point.position.x + point.position.y + point.position.z);

                GameObject obj = Instantiate(prefabGameObject, point, Quaternion.identity);
            }
        }
        else
        {
            // If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
            PrefabRayTrail.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRate));
        }
    }
    private IEnumerator ShotEffect()
    {
        // Play the shooting sound effect
        //gunAudio.Play();

        // Turn on our line renderer
        PrefabRayTrail.enabled = true;

        //Wait for .07 seconds
        yield return shotDuration;

        // Deactivate our line renderer after waiting
        PrefabRayTrail.enabled = false;
    }
}
