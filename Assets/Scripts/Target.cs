using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 5.0f;

    //public int pointValue;

    public ParticleSystem DestroyedEffect;

    public bool Destroyed => m_Destroyed;

    private bool m_Destroyed = false;
    float m_CurrecntHealth;

    public float speed = 1f;

    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        m_CurrecntHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward*Time.deltaTime*speed,Space.World);
    }
    public void GetDamge(float damge,Vector3 pos)
    {
        m_CurrecntHealth -= damge;

        Debug.Log("m_CurrecntHealth :"+ m_CurrecntHealth);
        if (m_CurrecntHealth > 0)
        {
            ParticleSystem pra = Instantiate(DestroyedEffect, pos, Quaternion.identity);
            Debug.Log("Return");
            return;
        }
        Vector3 position = transform.position;

        m_Destroyed = true;
        gameObject.SetActive(false);
        ParticleSystem praticels = Instantiate(DestroyedEffect, transform.position, Quaternion.identity);
    }
}
