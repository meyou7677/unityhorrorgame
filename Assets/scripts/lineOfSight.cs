using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineOfSight : MonoBehaviour
{

    private GameObject player;
    public float sightDistance;
    public LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public RaycastHit? Castray()
    {
        Vector3 direction = player.transform.position - transform.position;
        if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hitInfo, sightDistance, groundMask))
        {
           return hitInfo;
        }
        return null;
    }
}
