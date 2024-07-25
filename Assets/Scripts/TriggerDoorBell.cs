using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoorBell : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        if(other.gameObject.layer == 9)
            AudioManager.instance.PlayAudio(clip, transform);
    }
}
