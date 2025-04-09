using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wolfenemy : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    public float walking_speed;
    public float running_speed;
    private GameObject myplayer;
    public float attackD;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myplayer = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = myplayer.transform.position - transform.position;
        Vector3 newdirection = new Vector3(direction.x, 0, direction.z);
        if (newdirection.magnitude > attackD) 
        {
            Vector3 target = transform.position + newdirection;
            transform.LookAt(target);
            rb.AddForce(newdirection.normalized, ForceMode.Impulse);
            SpeedControl();

        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetTrigger("attack");
        }
        

    }
    private void SpeedControl()
    {
        var velocity = rb.velocity;
        var flatVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if (!(flatVelocity.magnitude > walking_speed)) return;
        var limitedVelocity = flatVelocity.normalized * walking_speed;
        rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
    }
}
