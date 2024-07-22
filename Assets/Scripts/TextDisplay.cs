using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] private string speechTotext;
    [SerializeField] private float charactersPerSecond = 90;
    [SerializeField] private float textInterval;
    [SerializeField] private TextMeshProUGUI dialogueText;

    //private AudioManager audioManager;
    //private int idx;
    // Start is called before the first frame update

    public void TypeLine(string line)
    {
        StartCoroutine(TypeTextUncapped(line));
    }

    IEnumerator TypeTextUncapped(string line)
    {
        float timer = 0;
        float interval = 1/charactersPerSecond;
        string textBuffer = null;
        char[] chars = line.ToCharArray();
        int i = 0;

        while (i < chars.Length)
        {
            if (timer < Time.deltaTime)
            {
                textBuffer += chars[i];
                dialogueText.text = textBuffer;
                timer += interval;
                i++;
            }
            else
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }
        
    }
}
