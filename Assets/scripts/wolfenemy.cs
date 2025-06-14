using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class wolfenemy : MonoBehaviour
{
    Animator animator;
    Rigidbody rb;
    public float walking_speed;
    public float running_speed;
    private GameObject myplayer;
    public float attackD;
    public float randomPointDistance;
    public Vector3? randomPoint = null;
    private lineOfSight raycastPoint;
    public float health;
    private bool hasdied = false;
    private NavMeshAgent m_NavMeshAgent;
    public enum enemyStates 
    {
        chase, patrol, attack, die
    }
    public enemyStates enemyState = enemyStates.patrol;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myplayer = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        raycastPoint = GetComponentInChildren<lineOfSight>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (enemyState)
        {
            case enemyStates.chase:
                ChaseState();
                break;
            case enemyStates.patrol:
                PatrolState();
                break;
            case enemyStates.attack:
                AttackState();
                break;
            case enemyStates.die:
                DieState();
                break;
        }
        
        

    }
    private void SpeedControl()
    {
        var velocity = rb.linearVelocity;
        var flatVelocity = new Vector3(velocity.x, 0f, velocity.z);
        if (!(flatVelocity.magnitude > walking_speed)) return;
        var limitedVelocity = flatVelocity.normalized * walking_speed;
        rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
    }

    private void ChaseState()
    {
        RaycastHit? HitInfo = raycastPoint.Castray();
        if (HitInfo != null)
        {
            Debug.Log(HitInfo.Value.collider.gameObject.name);
            if (HitInfo.Value.collider.gameObject.tag != "Player")
            {
                animator.SetTrigger("walk");
                enemyState = enemyStates.patrol;
                return;
            }
        }
        Vector3 direction = myplayer.transform.position - transform.position;
        Vector3 newdirection = new Vector3(direction.x, 0, direction.z);
        
        if (newdirection.magnitude > attackD) 
        {
            if (newdirection.magnitude > raycastPoint.sightDistance * 1.5)
            {
                animator.SetTrigger("walk");
                enemyState = enemyStates.patrol;
            }
            Vector3 target = transform.position + newdirection;
            transform.LookAt(target);
            rb.AddForce(newdirection.normalized * running_speed, ForceMode.Impulse);
            SpeedControl();
            
        
        }
        else
        {
            enemyState = enemyStates.attack;
            animator.SetTrigger("attack");
        }
    }

    private void PatrolState()
    {

        if (randomPoint == null)
        {
            
            randomPoint = new Vector3(Random.Range(20, randomPointDistance), 0, Random.Range(20, randomPointDistance));
            NavMeshHit hit;
            NavMesh.SamplePosition(randomPoint.Value, out hit, 20, 1);
            randomPoint = hit.position;
            m_NavMeshAgent.SetDestination(randomPoint.Value);
        }
        else
        {
            
           
            Vector3 direction = randomPoint.Value - transform.position;
           Vector3 newdirection = new Vector3(direction.x, 0, direction.z);
           Vector3 target = transform.position + newdirection;
           //transform.LookAt(target);
           //rb.AddForce(newdirection.normalized * walking_speed, ForceMode.Impulse);
           //SpeedControl();
            if (direction.magnitude < 0.5)
            {
                randomPoint = null;
            }

        }
        return;
        RaycastHit? HitInfo = raycastPoint.Castray();
        if (HitInfo != null)
        {
            
            if (HitInfo.Value.collider.gameObject.tag == "Player")
            {
                Vector3 direction = myplayer.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward,direction);
                animator.SetTrigger("chase");
                
                enemyState = enemyStates.chase;
            }
        }
    }

    private void AttackState()
    {
       
        Vector3 direction = myplayer.transform.position - transform.position;
        Vector3 newdirection = new Vector3(direction.x, 0, direction.z);
        if (newdirection.magnitude > attackD)
        {
            enemyState = enemyStates.chase;
            
            animator.SetTrigger("chase");

        }
        else
        {
            rb.linearVelocity = Vector3.zero;
            
        }
    }

    private void DieState()
    {
        rb.linearVelocity = Vector3.zero;
        if (!hasdied)
        {
            hasdied = true;
            animator.SetTrigger("die");
        }
        
    }

    private void OnDrawGizmos()
    {
        if (randomPoint != null)
        {
            Gizmos.DrawCube(randomPoint.Value, new Vector3 (1, 1, 1));
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            if (health > 0)
            {
                animator.SetTrigger("chase");
                enemyState = enemyStates.chase;
                health -= 1;
            }
            
            
            if (health <= 0 && !hasdied)
            {
                enemyState = enemyStates.die;
            }

        }
    }
}
