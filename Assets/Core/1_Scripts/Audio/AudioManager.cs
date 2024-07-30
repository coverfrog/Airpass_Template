using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public enum AudioType
    {
        Bgm,
        Narration,
        Sfx,
    }

    public class AudioManager : Singleton<AudioManager> 
    {
        #region > Dictionary
        private Dictionary<AudioType, AudioSource> _audioSources;

        private Dictionary<AudioType, AudioSource> AudioSources =>
            _audioSources ??= InitAudioSources();

        private Dictionary<AudioType, AudioSource> InitAudioSources()
        {
            var audioDictionary = new Dictionary<AudioType, AudioSource>();
            var enumNameArray = Enum.GetValues(typeof(AudioType));

            foreach (AudioType enumName in enumNameArray)
            {
                CreateNewSource(enumName, out var newSource);
                if (audioDictionary.TryAdd(enumName, newSource))
                {
                    
                }
            }

            return audioDictionary;
        }
        
        private void CreateNewSource(AudioType enumName, out AudioSource newSource)
        {
            var obj = new GameObject(enumName.ToString());
            obj.transform.SetParent(transform);

            newSource = obj.AddComponent<AudioSource>();
        }
        #endregion

        [SerializeField] private AudioDatabase database;

        public void Play(AudioType audioType, AudioName audioName)
        {
            var source = _audioSources[audioType];
            source.clip = database.Get(audioName);
            source.Play();
        }
        
        public void Play(AudioType audioType, AudioName audioName, out AudioSource source)
        {
            source = _audioSources[audioType];
            source.clip = database.Get(audioName);
            source.Play();
        }

        public void Stop(AudioType audioType)
        {
            var source = _audioSources[audioType];
            if (source.isPlaying)
                source.Stop();
            
            source.clip = null;
        }

        public bool IsPlaying(AudioType audioType)
        {
            var source = _audioSources[audioType];

            if (source.clip == null)
                return false;

            return source.time / source.clip.length < 0.98f;
        }

        public void SetLoop(AudioType audioType, bool isLoop)
        {
            _audioSources[audioType].loop = isLoop;
        }


        public override void Awake()
        {
            base.Awake();
            _ = AudioSources;
        }
    }
}
