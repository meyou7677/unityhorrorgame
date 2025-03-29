using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class player : MonoBehaviour
{
    torch m_torch;

    // Start is called before the first frame update
    void Start()
    {
        m_torch = GameObject.FindObjectOfType<torch>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
