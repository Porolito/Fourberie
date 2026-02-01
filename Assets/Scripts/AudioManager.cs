using System;
using UnityEngine;

public enum SoundType
{
    Dash,
    Jump,
    Attack,
    Win,
    Lose,
    Corde,
    LoopPeople,
    LoopMusic,
    BossMusic
}
public class AudioManager : MonoBehaviour
{
    private static AudioManager m_Instance;
    [SerializeField]private AudioSource audioSource;
    [SerializeField] private AudioSource loopPeople;
    [SerializeField] private AudioSource loopMusic;
    [SerializeField] private AudioClip[] sounds;
    private void Awake()
    {
        m_Instance = this;
    }

    public static void PlayOneShot(SoundType soundToPlay, float volume = 1)
    {
        m_Instance.audioSource.PlayOneShot(m_Instance.sounds[(int)soundToPlay], volume);
    }

    public static void PlayLoopingPeople(SoundType soundToLoop, float volume = 1)
    {
        m_Instance.loopPeople.clip = m_Instance.sounds[(int)soundToLoop];
        m_Instance.loopPeople.volume = volume;
        m_Instance.loopPeople.loop = true;
        m_Instance.loopPeople.Play();
    }

    public static void StopLoopingPeople(AudioClip soundToStop)
    {
        m_Instance.loopPeople.clip = soundToStop;
        m_Instance.loopPeople.Stop();
    }

    public static void PlayMusic(SoundType soundToPlay, float volume = 1)
    {
        m_Instance.loopMusic.clip = m_Instance.sounds[(int)soundToPlay];
        m_Instance.loopMusic.volume = volume;
        m_Instance.loopMusic.loop = true;
        m_Instance.loopMusic.Play();
    }
    
    public static void StopLoopingMusic(AudioClip soundToStop)
    {
        m_Instance.loopMusic.clip = soundToStop;
        m_Instance.loopMusic.Stop();
    }
}
