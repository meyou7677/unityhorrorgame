using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class torch : MonoBehaviour
{
    Light m_light;
    public float battery_energy;
    public float min_energy = 0;
    public float max_energy = 30;
    public Slider battery_slider;
    public float dimSpeed = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        m_light = GetComponent<Light>();
        battery_energy = max_energy;
        battery_slider.minValue = min_energy;
        battery_slider.maxValue = max_energy;

    }

    // Update is called once per frame
    void Update()
    {
        battery_energy -= dimSpeed * Time.deltaTime;
        m_light.intensity = battery_energy;
        battery_slider.value = battery_energy;
    }
}
