using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
     public static AudioManager Instance { get; private set; }
     public AudioSource ManagerSource { get; private set; }
     [SerializeField]
     private List<Sound> soundList;
     [SerializeField] 
     private List<Sound> songsList;
     [SerializeField]
     private AudioSource musicSource;
     [SerializeField]
     private Animator musicAnimator;

     private bool _keepPlaying;
     private Coroutine _playlistCoroutine;
     
     private void Awake()
     {
          if (Instance == null)
          {
               Instance = this;
               DontDestroyOnLoad(gameObject);
          }
          else
          {
               Destroy(gameObject);
          }
          ManagerSource = GetComponent<AudioSource>();
     }

     private void Start()
     {
          ToggleMusic(true);
     }
      
     public enum SoundType
     {
          Cast,
          Reel,
          Splash,
          Hook,
          Bite,
          UIButtonPop,
          UIButtonPop2,
          Pause,
          Unpause,
          MeowNormal,
          MeowAngry,
          MeowFeed,
          DayStart,
          DayOver,
          MoveOne,
          MoveTwo,
          Close,
          SongOne,
          SongTwo,
          SongThree,
          SongFour
     }

     [Serializable]
     private class Sound
     {
          public SoundType type;
          public AudioClip clip;
          [Range(0, 1)]
          public float volume;
          public bool loop;
     }

     public void PlaySound(SoundType soundType, AudioSource source)
     {
          var sound = soundList.Find(x => x.type == soundType);
          source.clip = sound.clip;
          source.volume = sound.volume;
          source.loop = sound.loop;
          if (sound.type == soundType)
          {
               if (!source.isPlaying)
               {
                    source.Play();
               }
          }
     }
     

     public void MuteSounds(bool isEnabled)
     {
          AudioListener.volume = isEnabled ? 1 : 0;
     }

     private IEnumerator PlayMusic()
     {
          float fadeDuration = 4f;
          float silence = 2f;
          
          while (_keepPlaying)
          {
               foreach (Sound song in songsList)
               {
                    if (!_keepPlaying) yield break;
                    
                    
                    musicSource.clip = song.clip;
                    musicSource.volume = 0;
                    musicSource.Play();
                    musicAnimator.Play("FadeIn");
                    
                    float playDuration = Mathf.Max(0, song.clip.length - fadeDuration);
                    
                    yield return new WaitForSecondsRealtime(playDuration);
                    musicAnimator.SetTrigger("FadeOut");
                    yield return new WaitForSecondsRealtime(fadeDuration);
                    musicSource.Stop();
                    yield return new WaitForSecondsRealtime(silence);
               }
          }
          
     }

     public void ToggleMusic(bool play)
     {
          _keepPlaying = play;
          if (play)
          {
               if (_playlistCoroutine == null)
               {
                    _playlistCoroutine = StartCoroutine(PlayMusic());
               }  
          }
          else
          {
               if (_playlistCoroutine != null)
               {
                    StopCoroutine(_playlistCoroutine);
                    _playlistCoroutine = null;
               }
               musicSource.Stop();
          }
     }
     
}
