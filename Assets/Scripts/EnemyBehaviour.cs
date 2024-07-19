using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float fireCooldown; //seconds
    [SerializeField] private float delay;
    [SerializeField] private int damage = 1;
    [SerializeField] private float maxAimOffset;
    [SerializeField] private float distToShoot;
    [SerializeField] private float distToFollow;   

    [SerializeField] private Rigidbody [] ragdoll;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private FieldOfView fov;
    
    private float fireTimer; //seconds
    private float idleTimer;
    private bool timerStarted;

    private AgentState state;
    private AudioManager audioManager;
    private Transform player;
    
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        state = AgentState.Following;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch(state)
            {
                case AgentState.Idle:
                
                    break;
                
                case AgentState.Wandering:

                    break;
                
                case AgentState.Following:
                
                    agent.SetDestination(player.position);
                    if(fov.visibleTargets.Count > 0)
                    {
                        //animator.SetBool("Shooting", true);
                        state = AgentState.Shooting;
                    }
                    break;
                
                case AgentState.Shooting:
                    agent.SetDestination(transform.position);
                    var lookPos = player.transform.position - transform.position;
                    Quaternion lookRot = Quaternion.LookRotation(lookPos);
                    lookRot.eulerAngles =new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10);
                    if(Vector3.Distance(transform.position, player.position) >= distToFollow || fov.visibleTargets.Count <= 0)
                    {
                        //animator.SetBool("Shooting", false);    
                        state = AgentState.Following;                
                    }
                    Shoot();
                    break;
                
            }  
        
        

    }

    public void Die()
    {
        agent.SetDestination(transform.position);
        state = AgentState.Dead;
        //DisableKinematics();
        //animator.enabled = false;
        GetComponent<Collider>().enabled = false;
       //GameManager.instance.DecrementEnemyCount();
    }

    void DisableKinematics() {
        foreach(Rigidbody child in ragdoll) {
            child.isKinematic = false;
        }
    }

    public void Shoot()
    {
        if(fireTimer >= fireCooldown)
        {
            fireTimer = 0;
            //animator.SetTrigger("Fire");
            StartCoroutine(DelayShot(delay));
        
       }

       fireTimer += Time.deltaTime;
    }

    private IEnumerator DelayShot(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        //audioManager.PlayGunShot(bulletSpawn);
        RaycastHit hit;
        Vector3 bulletDir = player.position - bulletSpawn.position;
        bulletDir = new Vector3
            (
                bulletDir.x * Random.Range(0,maxAimOffset),
                bulletDir.y * Random.Range(0,maxAimOffset),
                bulletDir.z * Random.Range(0,maxAimOffset)
            );
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(bulletSpawn.position, bulletDir, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(bulletSpawn.position, bulletDir * 10, Color.yellow);
            if(hit.collider.gameObject.layer == 6)
            {
                Debug.Log("Player hit");
                // Health playerHealth = player.gameObject.GetComponent<Health>();
                // playerHealth.TakeDamage(damage);
            }
        }

        TrailRenderer bulletTrail = Instantiate(trail,bulletSpawn.transform.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(bulletTrail, hit));

    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        Vector3 endPosition = hit.point;
        if(endPosition.magnitude <= 0)
        {
            endPosition = bulletSpawn.position + bulletSpawn.TransformDirection(Vector3.forward) * distToFollow;
        }

        while(time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition,endPosition,time);
            time += Time.deltaTime /trail.time;

            yield return null;
        }
        trail.transform.position = endPosition;
        Destroy(trail.gameObject, trail.time);

    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) {
        Vector3 randDirection = Random.insideUnitSphere * dist;
 
        randDirection += origin;
 
        NavMeshHit navHit;
 
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
 
        return navHit.position;
    }

    enum AgentState
    {
        Idle,
        Wandering,
        Following,
        Shooting,
        Dead
    }
}
