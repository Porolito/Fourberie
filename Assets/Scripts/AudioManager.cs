using System;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    private static AudioManager m_Instance;
    private AudioSource audioSource;

    private AudioSource playOnLoop;

    private void Awake()
    {
        m_Instance = this;
    }
    private void Start()
    {
        audioSource =  GetComponent<AudioSource>();
    }

    public static void PlayOneShot(AudioClip soundToPlay, float volume = 1)
    {
        m_Instance.audioSource.PlayOneShot(soundToPlay, volume);
    }

    public static void PlayLooping(AudioClip soundToLoop, float volume = 1)
    {
        m_Instance.playOnLoop.clip = soundToLoop;
        m_Instance.playOnLoop.volume = volume;
        m_Instance.playOnLoop.loop = true;
        m_Instance.playOnLoop.Play();
    }

    public static void StopLooping(AudioClip soundToStop)
    {
        m_Instance.playOnLoop.clip = soundToStop;
        m_Instance.playOnLoop.Stop();
    }
}
