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
    [SerializeField] private AudioSource playOnLoop;

    [SerializeField] private AudioClip[] sounds;
    private void Awake()
    {
        m_Instance = this;
    }

    public static void PlayOneShot(SoundType soundToPlay, float volume = 1)
    {
        m_Instance.audioSource.PlayOneShot(m_Instance.sounds[(int)soundToPlay], volume);
    }

    public static void PlayLooping(SoundType soundToLoop, float volume = 1)
    {
        m_Instance.playOnLoop.clip = m_Instance.sounds[(int)soundToLoop];
        Debug.Log(m_Instance.playOnLoop.clip);
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
