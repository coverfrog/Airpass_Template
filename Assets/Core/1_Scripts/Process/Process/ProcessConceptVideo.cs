using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CoverFrog
{
    public class ProcessConceptVideo : Process
    {
        private Image _previewImage;
        private Image PreviewImage =>
            _previewImage ??= transform.GetChild(0).GetComponent<Image>();
        
        private RawImage _rawImage;
        private RawImage RawImage => 
            _rawImage ??= transform.GetChild(1).GetComponent<RawImage>();

        private VideoPlayer _player;
        private VideoPlayer Player => 
            _player ??= transform.GetChild(1).GetComponent<VideoPlayer>();

        private Button _skipBtn;
        private Button SkipBtn => 
            _skipBtn ??= transform.GetChild(2).GetComponent<Button>();

        private void Awake()
        {
            SkipBtn.onClick.AddListener(Skip);
        }

        public override void Init(params object[] values)
        {
            // [0] = conceptVideoSprite
            // [1] = conceptVideo
            
            var renderTexture = 
                new RenderTexture(1920, 1080, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
            
            PreviewImage.sprite = 
                (Sprite)values[0];

            RawImage.texture =
                renderTexture;

            Player.clip 
                = (VideoClip)values[1];

            Player.targetTexture
                = renderTexture;
            
            gameObject.SetActive(true);
        }
        
        public override IEnumerator CoPlay(params object[] values)
        {
            Player.Play();
            yield return null;
        }

        private void Skip()
        {
            Pause();
            ProcessManager.Instance.ToState(ProcessState.Narration);
        }
    }
}
