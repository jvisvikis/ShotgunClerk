using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    [SerializeField] private string [] speechTotext;
    [SerializeField] private float charactersPerSecond = 90;
    [SerializeField] private float textInterval;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private AudioManager audioManager;
    private int idx;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        audioManager.PlayVoiceLine(idx);
        StartCoroutine(TypeTextUncapped(speechTotext[0]));
    }    

    IEnumerator TypeTextUncapped(string line)
    {
        float timer = 0;
        float lineLength = audioManager.GetCurrentLineLength();
        float interval = lineLength/(speechTotext[idx].Length * 1.5f);
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
        yield return new WaitForSeconds(textInterval);
        idx++;
        if(idx < speechTotext.Length)
        {
            audioManager.PlayVoiceLine(idx);
            StartCoroutine(TypeTextUncapped(speechTotext[idx]));
        }
    }
}
