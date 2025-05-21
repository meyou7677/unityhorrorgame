using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    torch m_torch;
    GameObject m_shootpoint;
    public GameObject bullet_prefab;
    public float bulletSpeed;
    // Start is called before the first frame update
    void Start()
    {
        m_torch = GameObject.FindObjectOfType<torch>();
        m_shootpoint = GameObject.Find("shoot point");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject b = GameObject.Instantiate(bullet_prefab);
            b.transform.position = m_shootpoint.transform.position;
            var bc = b.GetComponent<bullet>();
            if (bc != null)
            {
                bc.shoot(transform.forward, bulletSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "battery")
        {
            other.gameObject.SetActive(false);
            m_torch.battery_energy = m_torch.max_energy;
        }

    }

    private void OnTriggerExit(Collider other)
    {
       
    }

    private void OnTriggerStay(Collider other)
    {
        
    }
}
