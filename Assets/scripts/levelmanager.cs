using Unity.AI.Navigation;
using UnityEngine;

public class levelmanager : MonoBehaviour
{
    private player m_playerScript;
    private GameObject m_garage;
    private NavMeshSurface m_meshSurface;

    private void OnDisable()
    {
        m_playerScript.OnCraftingFinished -= M_playerScript_OnCraftingFinished;

    }

    private void M_playerScript_OnCraftingFinished()
    {
        m_garage.SetActive(false);
        m_meshSurface.BuildNavMesh();

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        GameObject myplayer = GameObject.FindGameObjectWithTag("Player");
        m_playerScript = myplayer.GetComponent<player>();
        m_playerScript.OnCraftingFinished += M_playerScript_OnCraftingFinished;
        m_garage = GameObject.FindGameObjectWithTag("garage");
        m_meshSurface = GetComponent<NavMeshSurface>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
