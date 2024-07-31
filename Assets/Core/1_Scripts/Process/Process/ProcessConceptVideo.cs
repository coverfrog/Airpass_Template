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
        #region > Values

        private Image _previewImage;
        private Image PreviewImage =>
            _previewImage ??= transform.GetChild(0).GetComponent<Image>();
        
        private RawImage _rawImage;
        private RawImage RawImage => 
            _rawImage ??= transform.GetChild(1).GetComponent<RawImage>();

        private VideoPlayer _player;
        private VideoPlayer Player => 
            _player ??= transform.GetChild(1).GetComponent<VideoPlayer>();

        private HelperConceptVideo _skipHelper;

        private HelperConceptVideo SkipBtn
        {
            get
            {
                if (_skipHelper != null)
                    return _skipHelper;
                
                _skipHelper = transform.GetChild(2).GetComponent<HelperConceptVideo>();
                _skipHelper.AddAction(Onclick_Skip, _skipHelper);

                return _skipHelper;
            }
        }
        #endregion

        private bool _isEnter;

        private void Awake()
        {
            _ = SkipBtn;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _isEnter = false;
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

        protected override IEnumerator CoPlay(params object[] values)
        {
            Player.Play();
            yield return null;
        }

        public override void Pause()
        {
            Player.Pause();
            base.Pause();
        }

        private void Onclick_Skip(Helper helper)
        {
            if(_isEnter)
                return;
            
            if(helper is not HelperConceptVideo conceptVideo)
                return;

            _isEnter = true;
            
            Pause();
            
            ProcessManager.Instance.ToState(ProcessState.Narration);
        }
    }
}
