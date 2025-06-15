using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    [SerializeField] private AudioSource BGM_AUDIO;
    [SerializeField] private AudioSource SFX_AUDIO;

    [SerializeField] private AudioClip[] sfx_bgm_AudioClips;
    private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        SetInstacne();

        foreach (AudioClip audioClip in sfx_bgm_AudioClips)
        {
            audioClipDictionary.Add(audioClip.name, audioClip);
        }
        PlaySFXSound("MainLobbySFX", 0.35f);
        DontDestroyOnLoad(this);
    }

    private void SetInstacne()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static SoundManager GetInstacne()
    {
        return instance;
    }

    public void PlayBGMSound(string audioClipName, float volume = 0.3f)
    {
        BGM_AUDIO.loop = true;
        BGM_AUDIO.clip = audioClipDictionary[audioClipName];
        BGM_AUDIO.volume = volume;
        BGM_AUDIO.Play();
    }
    public void PlaySFXSound(string audioClipName, float volume = 0.5f)
    {
        SFX_AUDIO.PlayOneShot(audioClipDictionary[audioClipName], volume);
    }
    public void StopAllAudioSound()
    { 
        SFX_AUDIO.Stop();
        BGM_AUDIO.Stop();
    }
}
