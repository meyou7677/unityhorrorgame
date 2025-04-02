using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfenemy : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) 
        {
            animator.SetTrigger("attack");
        }
        rb.AddForce(new Vector3 (1,0,0), ForceMode.Impulse);

    }
}
