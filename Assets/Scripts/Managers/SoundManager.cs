using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private Sound[] m_OneShotAudioArr;
    [SerializeField] private BackgroundMusic[] m_MusicSourceArr;
    [SerializeField] private AudioSource m_PlayOneShotSource;
    [SerializeField] private AudioSource m_MusicSource;

    private Dictionary<string, Sound> m_OneShotAudioDict;
    private IEnumerator m_MusicLoopFunc;

    private void Start()
    {
        GameManager.Instance.SoundManager = this;

        this.m_OneShotAudioDict = new Dictionary<string, Sound>();
        for (int s = 0; s < this.m_OneShotAudioArr.Length; s++)
        {
            Sound sound = this.m_OneShotAudioArr[s];
            this.m_OneShotAudioDict.Add(sound.SoundName, sound);
        }
    }

    /// <summary>
    /// Plays the specified sound using PlayOneShot
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayOneShot(string soundName)
    {
        // Sound soundToPlay = Array.Find(m_OneShotAudioArr, sound => sound.SoundName == soundName);
        Sound soundToPlay;
        if (this.m_OneShotAudioDict.TryGetValue(soundName, out soundToPlay))
        {
            m_PlayOneShotSource.volume = soundToPlay.Volume;
            m_PlayOneShotSource.spatialBlend = soundToPlay.SpatialBlend;
            m_PlayOneShotSource.pitch = soundToPlay.Pitch;
            m_PlayOneShotSource.PlayOneShot(soundToPlay.Clip);
        } else
        {
            Debug.LogWarning("Sound: " + soundName + " not found!");
            return;
        }

    }

    /// <summary>
    /// Plays the specified music
    /// </summary>
    /// <param name="musicName"></param>
    public void PlayMusic(string musicName)
    {
        BackgroundMusic musicToPlay = Array.Find(m_MusicSourceArr, music => music.MusicName == musicName);

        if (musicToPlay == null)
        {
            Debug.LogWarning("Music: " + musicName + " not found!");
            return;
        }

        if (m_MusicLoopFunc != null)
            StopCoroutine(m_MusicLoopFunc);

        m_MusicSource.loop = false;
        m_MusicSource.clip = musicToPlay.StartMusic;
        m_MusicSource.volume = musicToPlay.Volume;
        m_MusicSource.pitch = musicToPlay.Pitch;

        m_MusicLoopFunc = PlayLoopMusic(musicToPlay);
        StartCoroutine(m_MusicLoopFunc);
    }

    /// <summary>
    /// Stops any music playing on the music source
    /// </summary>
    public void StopMusic()
    {
        m_MusicSource.Stop();
        StopCoroutine(m_MusicLoopFunc);
        m_MusicLoopFunc = null;


        Debug.Log("Stop Music");
    }

    /// <summary>
    /// Loops the music
    /// </summary>
    /// <param name="music"></param>
    /// <returns></returns>
    private IEnumerator PlayLoopMusic(BackgroundMusic music)
    {
        m_MusicSource.Play();
        yield return new WaitForSeconds(music.StartMusic.length);
        m_MusicSource.clip = music.LoopMusic;
        m_MusicSource.loop = true;
        m_MusicSource.Play();
    }
}