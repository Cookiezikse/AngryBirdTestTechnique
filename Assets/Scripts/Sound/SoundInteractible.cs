using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundInteractable : MonoBehaviour
{

    [SerializeField]
    List<AudioClip> audios = new List<AudioClip>();

    AudioSource audioSource;

    private void Start()
    {
        audioSource = transform.AddComponent<AudioSource>();
    }


    public void PlayAudioRandom()
    {
        int i = Random.Range(0, audios.Count);

        audioSource.clip = audios[i];
        audioSource.Play();
    }

}
