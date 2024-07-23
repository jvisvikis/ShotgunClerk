using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Father fatherPrefab;

    public int day {get;set;}
    public float money {get; set;}

    private CustomerManager customerManager;
    private Father father;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        day = 1;
        customerManager = CustomerManager.instance;
        BeforeDayStart();
    }

    public void BeforeDayStart()
    {
        father = Instantiate(fatherPrefab, customerManager.GetCustomerSpawn().position, Quaternion.identity);
    }

    public void StartDay()
    {
        Debug.Log("Day Started");
        string dayToLoad = $"Day{day}";
        customerManager.customerPrefabs = Resources.LoadAll<CustomerBehaviour>(dayToLoad);
        customerManager.NextCustomer();
    }



    public void EndDay()
    {
        day++;
        //Show list of dead customers
    }
}
