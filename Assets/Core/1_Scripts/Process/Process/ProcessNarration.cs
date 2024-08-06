using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

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
        [Header("[ Narration ]")]
        [SerializeField] private Image previewImg;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private VideoPlayer videoPlayer;
        
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
            
            videoPlayer.Pause();
            
            AudioManager.Instance.Pause(AudioType.Narration);
        }

        public override void UnPause()
        {
            base.UnPause();
            
            videoPlayer.Play();
            
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
            
            // _[2] [3]
            // [3] [4] sprite, video
            var renderTex 
                = new RenderTexture(320, 330, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);

            rawImage.texture
                = renderTex;
            
            previewImg.sprite = (Sprite)values[2];
            
            videoPlayer.targetTexture = renderTex;
            videoPlayer.clip = (VideoClip)values[3];
            videoPlayer.frame = 1;

            gameObject.SetActive(true);
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            videoPlayer.Play();
            
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
