using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance {get; private set;}

    [SerializeField] private CustomerBehaviour [] customerPrefabs;
    [SerializeField] private Transform customerSpawn;
    [SerializeField] private Transform counterPlacement;

    public CustomerBehaviour currentCustomer {get;set;}
    public float thankTime;

    private List<CustomerBehaviour> customersDead;
    private int idx;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        NextCustomer(); 
        customersDead = new List<CustomerBehaviour>();
    }

    public void GiveCustomerItem(string itemName)
    {
        currentCustomer.TakeItem(itemName);
    }

    public void NextCustomer()
    {
        if(idx < customerPrefabs.Length)
        {
            currentCustomer = Instantiate(customerPrefabs[idx++], customerSpawn.position, Quaternion.identity);
            currentCustomer.storeEntrance = customerSpawn;
            currentCustomer.counterLine = counterPlacement;
        }
        else
        {
            //End Day
        }
        
    }


}
