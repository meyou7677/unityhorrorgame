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
    public float speed_multiplier;
    private GameObject myplayer;
    public float attackD;
    public float randomPointDistance;
    public Vector3? randomPoint = null;
    private lineOfSight raycastPoint;
    public float health;
    private bool hasdied = false;
    private NavMeshAgent m_NavMeshAgent;
    public float min_range;
    public float max_range;
    private Vector3 invesLocation;
    private float timer;
    private bool timer_started = false;
    private player m_playerScript;
    public bool m_isChaseSequence = false;
    public enum enemyStates 
    {
        chase, patrol, attack, die, investigate
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
        m_playerScript = myplayer.GetComponent<player>();
    }

    // Update is called once per frame

    private void Awake()
    {
        
    }
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
            case enemyStates.investigate:
                InvestigateState();
                break;

        }
        
        

    }
  

    private void ChaseState()
    {
        m_NavMeshAgent.speed = running_speed;
        m_NavMeshAgent.SetDestination(myplayer.transform.position);
        RaycastHit? HitInfo = raycastPoint.Castray();
        if (HitInfo != null)
        {
            Debug.Log(HitInfo.Value.collider.gameObject.name);
            
            if (HitInfo.Value.collider.gameObject.tag != "Player" && !m_isChaseSequence)
            {
                Debug.Log("Ivestigating");
                animator.SetTrigger("walk");
                invesLocation = myplayer.transform.position;
                enemyState = enemyStates.investigate;
                return;
            }
        }
        Vector3 direction = myplayer.transform.position - transform.position;
        Vector3 newdirection = new Vector3(direction.x, 0, direction.z);
        
        if (newdirection.magnitude > attackD ) 
        {
            if (newdirection.magnitude > raycastPoint.sightDistance * 1.5 && !m_isChaseSequence)
            {
                Debug.Log("Ivestigating");
                randomPoint = null;
                invesLocation = myplayer.transform.position;
                animator.SetTrigger("walk");
                enemyState = enemyStates.investigate;
            }
            Vector3 target = transform.position + newdirection;
            transform.LookAt(target);
            
        
        }
        else
        {
            enemyState = enemyStates.attack;
            animator.SetTrigger("attack");
        }
    }

    private void PatrolState()
    {
        
        m_NavMeshAgent.speed = walking_speed;
        if (randomPoint == null)
        {
            float range = Random.Range(min_range, max_range); 
            randomPoint = transform.position + Random.insideUnitSphere * range;
            NavMeshHit hit;
            bool value = NavMesh.SamplePosition(randomPoint.Value, out hit, range, 1);
            while (!value)
            {
                value = NavMesh.SamplePosition(randomPoint.Value, out hit, range, 1);
                Debug.Log("Couldn't find position");
            }
            randomPoint = hit.position;
            m_NavMeshAgent.SetDestination(randomPoint.Value);
            Debug.Log("Destination set " + randomPoint.Value);
           

        }
        else
        {
            
           m_NavMeshAgent.isStopped = false;
            Vector3 direction = randomPoint.Value - transform.position;
           Vector3 newdirection = new Vector3(direction.x, 0, direction.z);
           Vector3 target = transform.position + newdirection;
            Debug.DrawLine(transform.position, randomPoint.Value);
            if (direction.magnitude < 0.5)
            {
                randomPoint = null;
            }

        }
        RaycastHit? HitInfo = raycastPoint.Castray();
        if (HitInfo != null)
        {
            
            if (HitInfo.Value.collider.gameObject.tag == "Player")
            {
                Vector3 direction = myplayer.transform.position - transform.position;
                float angle = Vector3.Angle(transform.forward,direction);
                randomPoint = null;
                animator.SetTrigger("chase");
                m_NavMeshAgent.SetDestination(myplayer.transform.position);
                enemyState = enemyStates.chase;
            }
        }
        if (m_playerScript.IsPlayerCrafting)
        {
            invesLocation = myplayer.transform.position;
            enemyState = enemyStates.investigate;
        }
    }

    private void AttackState()
    {

        m_NavMeshAgent.isStopped = true;
        Vector3 direction = myplayer.transform.position - transform.position;
        Vector3 newdirection = new Vector3(direction.x, 0, direction.z);
        if (newdirection.magnitude > attackD)
        {
            enemyState = enemyStates.chase;
            m_NavMeshAgent.isStopped = false;
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
            m_NavMeshAgent.isStopped = true;
        }
        
    }

    private void InvestigateState()
    {
        m_NavMeshAgent.speed = walking_speed;
        Debug.DrawLine(transform.position, invesLocation);
        m_NavMeshAgent.SetDestination(invesLocation);
        RaycastHit? HitInfo = raycastPoint.Castray();
        if (HitInfo != null)
        {

            if (HitInfo.Value.collider.gameObject.tag == "Player")
            {
                Vector3 direction = myplayer.transform.position - transform.position;
                randomPoint = null;
                animator.SetTrigger("chase");
                m_NavMeshAgent.SetDestination(myplayer.transform.position);
                enemyState = enemyStates.chase;
                
            }
        }
        if (m_NavMeshAgent.remainingDistance <= 0.01)
        {
            m_NavMeshAgent.isStopped = true;
            enemyState = enemyStates.patrol;
            
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

    //private IEnumerator delayed_patrol()
    //{

    //}
}
