using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class CustomerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TextDisplay textDisplay;
    [SerializeField] private TextMeshProUGUI speech;
    [SerializeField] private NavMeshAgent agent;    
    [SerializeField] private bool robbing;
    [SerializeField] private string askingItem;
    [SerializeField] private string entranceSpeech; 

    public Transform counterLine {get; set;}
    public Transform storeEntrance {get; set;}

    private PlayerController player;
    private CustomerManager customerManager;
    private CustomerState state;
    private bool served;
    // Start is called before the first frame update
    void Start()
    {
        textDisplay = GetComponent<TextDisplay>();
        speechBubble.SetActive(false);
        speech.text = entranceSpeech;
        player = FindObjectOfType<PlayerController>();
        customerManager = CustomerManager.instance;
        state = CustomerState.EnterStore;
        agent.SetDestination(counterLine.position);
    }   

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case CustomerState.EnterStore:
                FacePlayer();
                if(transform.position.x == counterLine.position.x && transform.position.z == counterLine.position.z)
                {
                    textDisplay.TypeLine(entranceSpeech);
                    speechBubble.SetActive(true);
                    state = CustomerState.AskedForItem;
                }
                break;
            case CustomerState.AskedForItem:
                FacePlayer();
                if(served)
                {
                    if(robbing)
                    {
                        //rob
                    }
                    agent.SetDestination(storeEntrance.position);
                    state = CustomerState.ExitStore;
                }

                break;

            case CustomerState.ExitStore:
                if(transform.position.x == storeEntrance.position.x && transform.position.z == storeEntrance.position.z)
                {
                    customerManager.NextCustomer();
                    Destroy(this.gameObject);
                }
                break;
        }
    }

    private void FacePlayer()
    {
        var lookPos = player.transform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        lookRot.eulerAngles =new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 3);
    }

    public void TakeItem(string itemName)
    {
        Debug.Log(itemName);
        if(itemName.Contains(askingItem))
        {
            StartCoroutine(SayThanks(customerManager.thankTime));
        }
    }

    public IEnumerator SayThanks(float duration)
    {
        textDisplay.TypeLine("Thank you!");
        yield return new WaitForSeconds(duration);
        served = true;
        speechBubble.SetActive(false);
    }

    enum CustomerState{
        EnterStore,
        AskedForItem,
        ExitStore,
        Dead
    }
}
