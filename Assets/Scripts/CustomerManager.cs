using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance {get; private set;}

    
    [SerializeField] private Transform customerSpawn;
    [SerializeField] private Transform counterPlacement;

    public CustomerBehaviour currentCustomer {get;set;}
    public CustomerBehaviour [] customerPrefabs {get; set;}
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
        }
    }

    void Start()
    { 
        customersDead = new List<CustomerBehaviour>();
    }

    public Transform GetCustomerSpawn()
    {
        return customerSpawn;
    }

    public Transform GetCounterPlacement()
    {
        return counterPlacement;
    }

    public void AddDeadCustomers(CustomerBehaviour customer)
    {
        customersDead.Add(customer);
    }

    public void GiveCustomerItem(string itemName)
    {
        currentCustomer.TakeItem(itemName);
    }

    public void NextCustomer()
    {
        Debug.Log("Send him in");
        StartCoroutine(NextCustomer(Random.Range(1,4)));        
    }

    public IEnumerator NextCustomer(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(idx < customerPrefabs.Length)
        {
            Debug.Log("Oh hi Mark");
            currentCustomer = Instantiate(customerPrefabs[idx++], customerSpawn.position, Quaternion.identity);
            currentCustomer.storeEntrance = customerSpawn;
            currentCustomer.counterLine = counterPlacement;
        }
        else
        {
            //End Day
            GameManager.instance.EndDay();
        }
    }


}
