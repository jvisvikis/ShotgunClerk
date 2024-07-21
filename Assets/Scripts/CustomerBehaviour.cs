using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehaviour : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] private Transform counterLine;
    [SerializeField] private Transform storeEntrance;
    [SerializeField] private bool robbing;

    private CustomerState state;
    private bool served;
    // Start is called before the first frame update
    void Start()
    {
        state = CustomerState.EnterStore;
        agent.SetDestination(counterLine.position);
    }   

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case CustomerState.EnterStore:
                if(served)
                {
                    agent.SetDestination(storeEntrance.position);
                    state = CustomerState.ExitStore;
                }
                break;
            case CustomerState.ExitStore:
                if(robbing)
                {
                    //rob store
                }

                break;
        }
    }

    enum CustomerState{
        EnterStore,
        ExitStore,
        Dead
    }
}
