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

    public List<CustomerBehaviour> customersDead {get; private set;}
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

    public void ResetIndex()
    {
        idx = 0;
    }

    public void AddDeadCustomers(CustomerBehaviour customer)
    {
        customersDead.Add(customer);
    }

    public void ResetDeadCustomers()
    {
        customersDead.Clear();
    }

    public void GiveCustomerItem(string itemName)
    {
        currentCustomer.TakeItem(itemName);
    }

    public void NextCustomer()
    {
        StartCoroutine(NextCustomer(Random.Range(1,4)));        
    }

    public IEnumerator NextCustomer(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(idx < customerPrefabs.Length)
        {
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
