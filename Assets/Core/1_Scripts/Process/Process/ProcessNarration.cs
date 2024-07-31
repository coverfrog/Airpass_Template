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
        
        [Header("[ Option ]")]
        [SerializeField] public int fontSize = 50;
        [SerializeField] public AudioName audioName;
        [SerializeField, TextArea] public string descriptionText;
    }

    public class ProcessNarration : Process
    {
        private Text _narrationText;

        private Text NarrationText =>
            _narrationText ??= transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();

        public override void OnEnable()
        {
            base.OnEnable();

            PopupManager.Instance.OnCountUp += Pause;
            PopupManager.Instance.OnCountDown += UnPause;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            
            PopupManager.Instance.OnCountUp -= Pause;
            PopupManager.Instance.OnCountDown -= UnPause;
        }
        
        //

        public override void Pause()
        {
            base.Pause();
            AudioManager.Instance.Pause(AudioType.Narration);
        }

        public override void UnPause()
        {
            base.UnPause();
            AudioManager.Instance.UnPause(AudioType.Narration);
        }

        //

        public override void Init(params object[] values)
        {
            // [0] narrationText
            NarrationText.text = 
                (string)values[0];

            // [1] fontSize
            NarrationText.fontSize =
                (int)values[1];
            
            // _
            gameObject.SetActive(true);
        }

        protected override IEnumerator CoPlay(params object[] values)
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
