using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] private float waitTime;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private string [] day2lines;
    [SerializeField] private string [] day3lines;
    [SerializeField] private TextDisplay textDisplay;

    private int idx;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        speechBubble.SetActive(false);
    }

    public void TurnOnRadio()
    {
        if(gameManager.day == 2)
            StartCoroutine(StartNews(day2lines));
        if(gameManager.day == 3)
            StartCoroutine(StartNews(day3lines));
    }

    private IEnumerator StartNews(string [] lines)
    {
        while(!gameManager.dayOver)
        {
            speechBubble.SetActive(true);
            textDisplay.TypeLine(lines[idx%lines.Length]);
            idx++;
            yield return new WaitForSeconds(waitTime);
            speechBubble.SetActive(false);
            yield return new WaitForSeconds(Random.Range(1,waitTime));
        }
    }
}
