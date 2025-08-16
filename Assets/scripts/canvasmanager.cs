using TMPro;
using UnityEngine;

public class canvasmanager : MonoBehaviour
{
    private TextMeshProUGUI m_scrapstext;
    private TextMeshProUGUI m_ammoCountText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        m_scrapstext = transform.Find("scrapscollected").GetComponent<TextMeshProUGUI>();
        m_ammoCountText = transform.Find("ammo count").GetComponent <TextMeshProUGUI>();
        m_ammoCountText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatescrapcount(int scrapcount)
    {
        m_scrapstext.text = "scraps: " + scrapcount;
    }

    public void updateammocount(int ammocount)
    {
        m_ammoCountText.text = "Ammo: " + ammocount;
    }
}
