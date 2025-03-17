using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundInteractable : MonoBehaviour
{

    [SerializeField]
    List<AudioClip> audios_1 = new List<AudioClip>();

    [SerializeField]
    List<AudioClip> audios_2 = new List<AudioClip>();

    AudioSource audioSource;

    int indexList = 0;

    private void Start()
    {
        audioSource = transform.AddComponent<AudioSource>();
    }


    public void PlayAudioRandom(int selectList = 0)
    {

        indexList = selectList;

        int i = 0;

        switch (indexList)
        {
            case 0:
                {
                    i = Random.Range(0, audios_1.Count);
                    audioSource.clip = audios_1[i];
                    break;
                }
            case 1:
                {
                    i = Random.Range(0, audios_2.Count);
                    audioSource.clip = audios_2[i];
                    break;
                }
        }


        audioSource.Play();
    }

}
