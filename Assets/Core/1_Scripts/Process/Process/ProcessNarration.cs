using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    [Serializable]
    public class ProcessNarrationData
    {
        [SerializeField] public string codeName;
        [SerializeField] public int index;
        [SerializeField] public AudioName audioName;
        [SerializeField, TextArea] public string descriptionText;
    }

    public class ProcessNarration : Process
    {
        private Text _narrationText;

        private Text NarrationText =>
            _narrationText ??= transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        
        public override void Init(params object[] values)
        {
            // [0] narrationText
            NarrationText.text = 
                (string)values[0];
            
            // _
            gameObject.SetActive(true);
        }
        
        public override IEnumerator CoPlay(params object[] values)
        {
            // [0] NarrationAudioName            
            var narrationAudioName = (AudioName)values[0];
            var audioInstance = AudioManager.Instance;
            
            audioInstance.Play(AudioType.Narration, narrationAudioName);
            while (audioInstance.IsPlaying(AudioType.Narration))
            {
                yield return null;
            }
            
            // complete
            ProcessManager.Instance.ToState(ProcessState.GamePlay);
        }
    }
}
