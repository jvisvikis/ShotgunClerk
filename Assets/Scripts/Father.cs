using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Father : MonoBehaviour
{
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private GameObject shotgun;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private TextDisplay textDisplay;
    [SerializeField] private string [] dayOneLines;
    [SerializeField] private string [] dayTwoLines;

    [SerializeField] private AudioClip [] dayOneAudioClips;
    [SerializeField] private AudioClip [] dayTwoAudioClips;

    private AudioManager audioManager;
    private CustomerManager customerManager;
    private GameManager gameManager;

    private PlayerController player;
    private bool startedTalking;
    private bool stoppedTalking;
    private bool dayStarted;
    private bool playerHasCigs;

    // Start is called before the first frame update
    void Start()
    {
        speechBubble.SetActive(false);
        shotgun.SetActive(false);
        audioManager = AudioManager.instance;
        customerManager = CustomerManager.instance;
        gameManager = GameManager.instance;
        player = FindObjectOfType<PlayerController>();
        agent.SetDestination(customerManager.GetCounterPlacement().position);
    }

    void Update()
    {
        if (!stoppedTalking) FacePlayer();

        if(player.itemEquipped != null && player.itemEquipped.name.Contains("Cigarettes"))
        {
            playerHasCigs = true;
        }

        if(gameManager.day == 1)
        {
            if(agent.destination == transform.position && !startedTalking)
            {
                speechBubble.SetActive(true);
                startedTalking = true;
                StartCoroutine(StartTalking(dayOneLines, dayOneAudioClips, 0, 15));
            }
            if(agent.destination == transform.position && stoppedTalking && !dayStarted)
            {
                dayStarted = true;
                StartCoroutine(StartDay(3f)); 
                Destroy(this.gameObject,3.5f);           
            }
        }
        if(gameManager.day == 2)
        {
            if(agent.destination == transform.position && !startedTalking)
            {
                speechBubble.SetActive(true);
                startedTalking = true;
                StartCoroutine(StartTalking(dayTwoLines, dayTwoAudioClips, 0, dayTwoAudioClips.Length));
            }
            if((agent.destination == transform.position && stoppedTalking && !dayStarted) )
            {
                Debug.Log("Please start");
                dayStarted = true;
                StartCoroutine(StartDay(3f)); 
                Destroy(this.gameObject,3.5f);           
            }
        }
        
    }

    private void FacePlayer()
    {
        var lookPos = player.transform.position - transform.position;
        Quaternion lookRot = Quaternion.LookRotation(lookPos);
        lookRot.eulerAngles =new Vector3(transform.rotation.eulerAngles.x, lookRot.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 3);
    }

    public IEnumerator StartTalking(string [] lines, AudioClip[] audioClips, int idx, int stopIdx)
    {
        audioManager.PlayAudio(audioClips[idx], transform);
        textDisplay.TypeLine(lines[idx]);
        yield return new WaitForSeconds(audioClips[idx].length);
        idx++;
        if(idx < stopIdx || (playerHasCigs && idx < audioClips.Length))
        {
            StartCoroutine(StartTalking(lines, audioClips, idx, stopIdx));
        }
        else if(idx < audioClips.Length && !playerHasCigs)
        {
            while(!playerHasCigs)
            {
                yield return null;
            }
            shotgun.SetActive(true);
            yield return new WaitForSeconds(1f);
            StartCoroutine(StartTalking(lines, audioClips, idx, stopIdx));
        }
        else
        {
            agent.SetDestination(customerManager.GetCustomerSpawn().position);
            stoppedTalking = true;
            shotgun.SetActive(false);
            speechBubble.SetActive(false);
        }      
    }

    public IEnumerator StartDay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.StartDay();
    }


}
