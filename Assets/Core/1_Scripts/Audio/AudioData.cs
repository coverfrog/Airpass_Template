using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    [Serializable]
    public class AudioData
    {
        [SerializeField] private AudioName audioName;
        [SerializeField] private string codeName;
        [SerializeField] private AudioClip audioClip;

        public AudioData(string cName, AudioClip aClip)
        {
            codeName = cName;
            audioClip = aClip;
        }

        public void Convert()
        {
            audioName = (AudioName)Enum.Parse(typeof(AudioName), codeName);
        }

        public AudioName Name => audioName;

        public AudioClip Clip => audioClip;
    }
}
