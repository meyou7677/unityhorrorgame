using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player : MonoBehaviour
{
    torch m_torch;
    GameObject m_shootpoint;
    public GameObject bullet_prefab;
    public float bulletSpeed;
    private float m_shoottimer;
    public float shootcooldown;
    private int number_of_scraps = 0;
    private uimessage m_uimessage;
    private canvasmanager m_canvasmanager;
    private bool m_gunready = false;
    private GameObject m_gun;
    private float m_Crafting_timer;
    public float crafting_time_seconds;
    public int scraps_required;
    public bool IsCraftingFinished = false;
    public delegate void OnCraftingFinishedDelegate();
    public event OnCraftingFinishedDelegate OnCraftingFinished;
    private int m_ammo_count = 0;
    
    public bool IsPlayerCrafting { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        m_torch = GameObject.FindFirstObjectByType<torch>();
        m_shootpoint = GameObject.Find("shoot point");
        m_shoottimer = 0;
        m_uimessage = GameObject.Find("message").GetComponent<uimessage>();
        m_canvasmanager = GameObject.Find("Canvas").GetComponent<canvasmanager>();
        m_gun = transform.Find("CameraHolder/Main Camera/gun").gameObject;
        m_gun.SetActive(false);
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_shoottimer > 0)
        {
            m_shoottimer -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0) && m_shoottimer <= 0 && m_gunready && m_ammo_count > 0)
        {
            m_shoottimer = shootcooldown;
            GameObject b = GameObject.Instantiate(bullet_prefab);
            b.transform.position = m_shootpoint.transform.position;
            var bc = b.GetComponent<bullet>();
            if (bc != null)
            {
                bc.Initialize();
                bc.shoot(Camera.main.transform.forward, bulletSpeed);
                m_ammo_count -= 1;
                m_canvasmanager.updateammocount(m_ammo_count);
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
        else if(other.tag == "scrap")
        {
            number_of_scraps++;
            m_canvasmanager.updatescrapcount(number_of_scraps);
            other.gameObject.SetActive(false);
            if (IsGunCraftable())
            {
                m_uimessage.display_message("Gun is now craftable", 3);
                
            }

        }
        else if(other.tag == "crafting table")
        {
            if (IsGunCraftable())
            {
                m_uimessage.display_message("Hold E for " +crafting_time_seconds + " seconds.", 3);
            }
            
            m_Crafting_timer = 0;
        }
        
        else if(other.tag == "ammo box")
        {
            m_ammo_count += 10;
            m_canvasmanager.updateammocount(m_ammo_count);
            other.gameObject.SetActive(false);
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "crafting table")
        {
            m_Crafting_timer = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "crafting table" && IsGunCraftable())
        {
            
            if (Input.GetKey(KeyCode.E))
            {
                IsPlayerCrafting = true;
                m_Crafting_timer += Time.deltaTime;
                if (m_Crafting_timer > crafting_time_seconds)
                {
                    m_gunready = true;
                    m_gun.SetActive(true);
                    m_canvasmanager.updateammocount(m_ammo_count);
                    number_of_scraps -= scraps_required;
                    m_canvasmanager.updatescrapcount(number_of_scraps);
                    IsCraftingFinished = true;
                    OnCraftingFinished();
                    
                }
                
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                IsPlayerCrafting = false;
                m_Crafting_timer = 0;

            }
            Debug.Log(m_Crafting_timer);
        }
    }
    private bool IsGunCraftable()
    {
        return number_of_scraps >= scraps_required;
    }
}
