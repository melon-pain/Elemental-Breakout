using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBGM : MonoBehaviour
{
    [SerializeField] AudioClip levelBGM;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = levelBGM;
        audioSource.Play();
    }

    public void PlayBossBGM(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
