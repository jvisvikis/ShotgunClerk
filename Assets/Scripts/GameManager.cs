using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Father fatherPrefab;

    public int day {get;set;}
    public float money {get; set;}
    public float gross {get; set;}
    public float killCost {get; set;}
    public float robCost {get;set;}
    private bool dayOver;

    private CustomerManager customerManager;
    private InputManager inputManager;
    private UIManager uiManager;
    private ScenesManager sceneManager;
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
        inputManager = InputManager.instance;
        uiManager = UIManager.instance;
        sceneManager = ScenesManager.instance;
        BeforeDayStart();
    }

    void Update()
    {
        if(InputManager.instance.PlayerFired() && dayOver)
        {
            //Load Next Day
            sceneManager.ReloadScene();
        }
    }

   
    public void BeforeDayStart()
    {
        dayOver = false;
        if(customerManager == null)
        {
            return;
        }
        if(day <= 2)
        {
            father = null;
            father = Instantiate(fatherPrefab, customerManager.GetCustomerSpawn().position, Quaternion.identity);
        }
        else    
        {
            StartDay();
        }
    }

    public void StartDay()
    {
        Debug.Log("Day Started");
        string dayToLoad = $"Day{day}";
        customerManager.customerPrefabs = Resources.LoadAll<CustomerBehaviour>(dayToLoad);
        customerManager.NextCustomer();
    }

    public void Served(float amount)
    {
        gross += amount;
        money += amount;
    }

    public void Robbed()
    {
        float amountLost = Mathf.Min(Random.Range(100,1000),money);
        amountLost = amountLost < 0 ? 0 : amountLost;

        robCost += amountLost;
        money -= amountLost;
    }

    public void KilledCustomer(bool robber)
    {
        killCost += robber ? 500 : -500;
    }
    public void EndDay()
    {
        Debug.Log("Day Ended");
        money += killCost;
        dayOver = true;
        uiManager.EndDay(day);
        day++;
    
    }

    public IEnumerator BeginDay()
    {
        while(customerManager == null)
        {
            yield return null;
        }
        BeforeDayStart();
    }
}
