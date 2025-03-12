using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torch : MonoBehaviour
{
    Light m_light;
    public float battery;
    
    // Start is called before the first frame update
    void Start()
    {
        m_light = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        m_light.intensity -= battery * Time.deltaTime;
    }
}
