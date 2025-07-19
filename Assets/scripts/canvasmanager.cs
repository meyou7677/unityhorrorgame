using TMPro;
using UnityEngine;

public class canvasmanager : MonoBehaviour
{
    private TextMeshProUGUI m_scrapstext;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_scrapstext = transform.Find("scrapscollected").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updatescrapcount(int scrapcount)
    {
        m_scrapstext.text = "scraps: " + scrapcount;
    }
}
