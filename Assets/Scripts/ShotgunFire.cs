using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotgunFire : MonoBehaviour
{
    [SerializeField] private float fireCooldown; //seconds
    [SerializeField] private float maxOffset;
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Recoil recoil;

    private float fireTimer; //seconds
    private Transform cam;
    private InputManager inputManager;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        inputManager = InputManager.instance;
        fireTimer = 0f;       
    }

    // void Update()
    // {

    //     fireTimer += Time.deltaTime;
                    
    // }

    public void Shoot()
    {
        if(inputManager.PlayerFired())
        {
            //AudioManager.instance.PlayGunShot(bulletSpawn);
            recoil.RecoilFire();
            fireTimer = 0;
            Vector3 [] bulletDir = new Vector3[6];
            for(int i = 0; i < bulletDir.Length; i++)
            {
                bulletDir[i] = cam.transform.TransformDirection(Vector3.forward);
                bulletDir[i] = new Vector3
                (
                    bulletDir[i].x + Random.Range(-maxOffset,maxOffset),
                    bulletDir[i].y + Random.Range(-maxOffset,maxOffset),
                    bulletDir[i].z + Random.Range(-maxOffset,maxOffset)
                );

                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, bulletDir[i], out hit, Mathf.Infinity))
                {
                    Debug.Log("Did Hit: " + hit.collider.gameObject.name);
                }
                // Debug.DrawRay(cam.transform.position, cam.transform.TransformDirection(Vector3.forward) * 10, Color.yellow);
                // Debug.Log("Ray: " + cam.transform.TransformDirection(Vector3.forward) * 10);
                TrailRenderer bulletTrail = Instantiate(trail,bulletSpawn.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(bulletTrail, hit, bulletDir[i]));
            }
             
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit, Vector3 dir)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;
        Vector3 endPosition = hit.point;
   
        if(endPosition.magnitude <= 0)
        {
            endPosition = cam.position + dir * 10;
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
    
}