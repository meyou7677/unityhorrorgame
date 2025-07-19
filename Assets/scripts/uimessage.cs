using System.Collections;
using TMPro;
using UnityEngine;

public class uimessage : MonoBehaviour
{
    private TextMeshProUGUI m_textMeshProUGUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void display_message(string message, int wait_seconds)
    {
        m_textMeshProUGUI.text = message;
        StartCoroutine(wait_text(wait_seconds));
    }

    IEnumerator wait_text(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_textMeshProUGUI.text = "";
    }
}
