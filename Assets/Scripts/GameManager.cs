using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private Father fatherPrefab;

    public int day {get; set;}
    public float money {get; set;}
    public float gross {get; set;}
    public float killCost {get; set;}
    public float robCost {get; set;}
    public bool dayOver  {get; set;}

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
        if(InputManager.instance.PlayerFired() && dayOver && day < 4)
        {
            //Load Next Day
            sceneManager.ReloadScene();
        }

        if(InputManager.instance.PlayerFired() && day > 3)
        {
            //GameOver
        }
    }

   
    public void BeforeDayStart()
    {
        if(day == 1)
        {
            FindObjectOfType<Radio>().gameObject.SetActive(false);
        }

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
        AudioManager.instance.PlayBackgroundMusic();
        string dayToLoad = $"Day{day}";
        uiManager.StartDay();
        customerManager.customerPrefabs = Resources.LoadAll<CustomerBehaviour>(dayToLoad);
        customerManager.ResetIndex();
        customerManager.ResetDeadCustomers();
        customerManager.NextCustomer();
        if(day >= 2)
            FindObjectOfType<Radio>().GetComponent<Radio>().TurnOnRadio();
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
