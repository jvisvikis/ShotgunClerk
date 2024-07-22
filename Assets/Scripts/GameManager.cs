using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private GameObject Father;

    public int day {get;set;}
    public float money {get; set;}

    private CustomerManager customerManager;

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
        customerManager = CustomerManager.instance;
        DayStart();
    }

    public void DayStart()
    {
        
    }

    public void DayEnd()
    {
        day++;
    }
}
