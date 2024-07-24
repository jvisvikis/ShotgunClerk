using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float bounceSpeed = 5f;
    [SerializeField] private float gravityValue = -9.81f;
    [SerializeField] private string [] itemNames;
    [SerializeField] private GameObject [] itemPrefabs;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform counterSpawn;
    [SerializeField] private Transform itemSpawn;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private LayerMask counterLayer;
    [SerializeField] private Vector3 shotgunPosOffset;

    public GameObject itemEquipped {get;set;}
    private ShotgunFire shotgun;

    private bool groundedPlayer;
    private bool hasItem;   
    private bool shotgunEquipped;
    private Camera cam;
    private CharacterController controller;
    private CustomerManager customerManager;
    private GameManager gameManager;
    private Vector3 playerVelocity; 
    private InputManager inputManager;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main;
        counterSpawn = GameObject.Find("ItemSpawn").transform;
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.instance;
        customerManager = CustomerManager.instance;
        gameManager = GameManager.instance;
    }

    void Update()
    {
        
        Vector2 movement = inputManager.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        move = camTransform.forward * move.z + camTransform.right * move.x;
        move.y = 0;
        controller.Move(move * Time.deltaTime * playerSpeed);
        if(inputManager.PlayerFired() && !hasItem)
        {
            GrabItem();
        }
        else if(inputManager.PlayerFired() && !shotgunEquipped)
        {
            PlaceItem();
        }
        else if(inputManager.PlayerFired())
        {
            shotgun.Shoot();
            DropItem();
        }
        
        // if(itemEquipped != null)
        // {
        //     BobItem();
        // }
        playerVelocity.y += gravityValue * Time.deltaTime;
        if(gameManager.dayOver)
            controller.Move(playerVelocity * Time.deltaTime);
        
    }

    private void GrabItem()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, itemLayer))
        {
            if(hit.collider == null)
            {
                return;
            }
            if(hit.collider.gameObject.name.Contains("Shotgun"))
            {
                hasItem = true;
                shotgunEquipped = true;
                itemEquipped = hit.collider.gameObject.transform.root.gameObject;
                itemEquipped.GetComponent<Rigidbody>().isKinematic = true;
                itemEquipped.transform.parent = itemSpawn;
                itemEquipped.transform.localPosition = shotgunPosOffset;
                itemEquipped.transform.localRotation = Quaternion.Euler(0,-5,0);
                shotgun = itemEquipped.GetComponent<ShotgunFire>();

            }
            else
            {
                for(int i = 0; i<itemNames.Length; i++)
                {
                    if(itemNames[i].Equals(hit.collider.gameObject.name))
                    {
                        hasItem = true;
                        itemEquipped = Instantiate(itemPrefabs[i], itemSpawn.position, itemSpawn.localRotation);
                        itemEquipped.transform.parent = itemSpawn;
                        itemEquipped.transform.localPosition = Vector3.zero;
                        itemEquipped.transform.localRotation = Quaternion.identity;
                    }
                }
            }
        }
    }

    private void PlaceItem()
    {
        itemEquipped.GetComponent<Collider>().enabled = false;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, counterLayer))
        {
            hasItem = false;
            itemEquipped.GetComponent<Collider>().enabled = true;
            //Debug.Log("Did Hit: " + hit.collider.gameObject.name);
            if(customerManager.currentCustomer != null)
            {
                customerManager.GiveCustomerItem(itemEquipped.name);
            }            
            itemEquipped.transform.parent = counterSpawn;
            itemEquipped.transform.localPosition = Vector3.zero;
            Rigidbody rb = itemEquipped.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            Destroy(itemEquipped, customerManager.thankTime);
            itemEquipped = null;
        }
        else
        {
            itemEquipped.GetComponent<Collider>().enabled = true;
            DropItem();
        }

    }

    private void DropItem()
    {
        hasItem = false;
        shotgunEquipped = false;
        Rigidbody rb = itemEquipped.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        if(shotgun == null)
        {
            rb.AddForce(itemEquipped.transform.forward * 100f);
        }
        else
        {
            rb.AddForce(itemEquipped.transform.forward * -500f);
        }
        
        itemEquipped.transform.parent = null;
        //Destroy(itemEquipped, 3);
        itemEquipped = null;
        shotgun = null;
    }

    private void BobItem()
    {
        float sinValue = Mathf.Sin(Time.time * controller.velocity.magnitude);
        float velocity = controller.velocity.magnitude/playerSpeed;
        Debug.Log(velocity);
        float yPos = Mathf.Lerp(0.1f * velocity, -0.1f * velocity, Mathf.Abs(sinValue));
        itemEquipped.transform.localPosition = new Vector3(itemEquipped.transform.localPosition.x, yPos, itemEquipped.transform.localPosition.z);
        itemEquipped.transform.localRotation = Quaternion.identity;
        //Rotate
        // itemEquipped.transform.Rotate(Vector3.up, Time.deltaTime * 3);
    }
}
