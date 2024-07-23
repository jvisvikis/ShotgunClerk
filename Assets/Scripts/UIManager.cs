using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private GameObject endDayPanel;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI [] deadCustomers;
    [SerializeField] private TextMeshProUGUI [] deadCustomersStatus;
    [SerializeField] private TextMeshProUGUI grossProfitText;
    [SerializeField] private TextMeshProUGUI killCostText;
    [SerializeField] private TextMeshProUGUI robberyCostText;
    [SerializeField] private TextMeshProUGUI netProfitText;

    private GameManager gameManager;
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
        gameManager = GameManager.instance;
        customerManager = CustomerManager.instance;
        endDayPanel.SetActive(false);
    }

    public void StartDay()
    {
        endDayPanel.SetActive(false);
    }

    public void EndDay(int day)
    {
        endDayPanel.SetActive(true);
        FillDeadCustomerInfo();
        dayText.text = $"Day {day} over!";
    }

    public void FillDeadCustomerInfo()
    {
        float amount = 500;
        for(int i = 0; i<customerManager.customersDead.Count; i++)
        {
            deadCustomers[i].gameObject.SetActive(true);
            deadCustomersStatus[i].gameObject.SetActive(true);
            deadCustomers[i].text = customerManager.customersDead[i].GetName();
            string status = customerManager.customersDead[i].GetRobbing() ? $"Thief <color=green>+${amount}" : $"Civilian <color=red>-${amount}";
            deadCustomersStatus[i].text = status;
        }

        if(customerManager.customersDead.Count < deadCustomers.Length)
        {
            for(int i = customerManager.customersDead.Count; i<deadCustomers.Length; i++)
            {
                deadCustomers[i].gameObject.SetActive(false);
                deadCustomersStatus[i].gameObject.SetActive(false);
            }
        }

        grossProfitText.text = gameManager.gross >= 0 ? $"Gross <color=green>{gameManager.gross}" : $"Gross <color=red>{gameManager.gross}" ;
        killCostText.text = gameManager.killCost >= 0 ? $"Kill Cost       <color=green>{gameManager.killCost}" : $"Kill Cost       <color=red>{gameManager.killCost}";
        robberyCostText.text = $"Robbery Cost  <color=red>{gameManager.robCost}";
        netProfitText.text = gameManager.money >= 0 ? $"Net Profit <color=green>{gameManager.money}" : $"Net Profit <color=red>{gameManager.money}";
    }
}
