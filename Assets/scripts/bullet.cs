using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    Rigidbody rb;
    float life_time = 0 ;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        life_time += Time.deltaTime;
        if (life_time > 10)
        {
            Destroy(gameObject);
        }
    }

    public void shoot(Vector3 direction, float speed)
    {
        rb.AddForce(direction * speed);
    }

    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        Destroy(gameObject);
    }



}
