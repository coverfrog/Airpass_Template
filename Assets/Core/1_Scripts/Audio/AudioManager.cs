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
            _database = Resources.Load<AudioDatabase>("Audio Database");
            
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

        private AudioDatabase _database;

        public void Play(AudioType audioType, AudioName audioName)
        {
            var source = _audioSources[audioType];
            source.clip = _database.Get(audioName);
            source.Play();
        }
        
        public void Play(AudioType audioType, AudioName audioName, out AudioSource source)
        {
            source = _audioSources[audioType];
            source.clip = _database.Get(audioName);
            source.Play();
        }

        public void Stop(AudioType audioType)
        {
            var source = _audioSources[audioType];
            if (source.isPlaying)
                source.Stop();
            
            source.clip = null;
        }

        public void Pause(AudioType audioType)
        {
            var source = _audioSources[audioType];
            source.Pause();
        }

        public void UnPause(AudioType audioType)
        {
            var source = _audioSources[audioType];
            source.UnPause();
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

        public void SetVolume(OptionName optionName, float value)
        {
            value = Mathf.Clamp01(value);

            var target = optionName switch
            {
                OptionName.Bgm => AudioType.Bgm,
                OptionName.Sfx => AudioType.Sfx,
                OptionName.Narration => AudioType.Narration,
                _ => throw new ArgumentOutOfRangeException(nameof(optionName), optionName, null)
            };

            _audioSources[target].volume = value;
        }

        public void Awake()
        {
            _ = AudioSources;
        }
    }
}
