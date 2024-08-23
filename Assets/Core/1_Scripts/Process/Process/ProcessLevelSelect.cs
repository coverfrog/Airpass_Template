using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CoverFrog
{
    public class ProcessLevelSelect : Process
    {
        #region > Values
        private List<HelperLevelSelect> _helpers;

        private List<HelperLevelSelect> Helpers =>
            _helpers ??= HelpersInit();

        private List<HelperLevelSelect> HelpersInit()
        {
            var helpers = new List<HelperLevelSelect>();
            
            foreach (Transform tr in transform.GetChild(2))
            {
                var helper = tr.GetComponent<HelperLevelSelect>();
                
                if (helper == null)
                    continue;
                
                helper.AddAction(OnClick_LevelSelect, helper);
                helpers.Add(helper);
            }

            return helpers;
        }

        private Text _levelDescriptionText;
        private Text LevelDescriptionText => _levelDescriptionText ??=
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        
        #endregion

        [Header("[ Level Select ]")]
        [SerializeField] private Image previewImg;
        [SerializeField] private RawImage rawImage;
        [SerializeField] private VideoPlayer videoPlayer;
        [Space] 
        [SerializeField] private GameAutoQuit gameAutoQuit;
        [SerializeField] private GameLure gameLure;
        
        //
        
        private bool _isEnter;

        public override void OnEnable()
        {
            base.OnEnable();
            
            PopupManager.Instance.OnCountUp += Pause;
            PopupManager.Instance.OnCountDown += UnPause;
            
            _isEnter = false;
            Helpers.ForEach(h => h.SetInteract(true));
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
            gameObject.SetActive(true);
            // [0] level max count
            var levelMaxCount = (int)values[0];
            
            Helpers.ForEach(h => 
                h.SetActive(false));
            
            foreach (var helper in Helpers.Take(levelMaxCount))
                helper.SetActive(true);

            // [1] levelSelectText
                
            var levelSelectText = (string)values[1];

            LevelDescriptionText.text =
                levelSelectText;
                
            // [2] levelSelectFontSize

            var levelSelectFontSize = (int)values[2];

            LevelDescriptionText.fontSize =
                levelSelectFontSize;
            
            // [3] [4] sprite, video
            var renderTex 
                = new RenderTexture(320, 330, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);

            rawImage.texture
                = renderTex;
            
            previewImg.sprite = (Sprite)values[3];
            
            videoPlayer.targetTexture = renderTex;
            videoPlayer.clip = (VideoClip)values[4];
            videoPlayer.frame = 1;
            
            // _
            // gameObject.SetActive(true);
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            videoPlayer.Play();
            
            // [0] level Select AudioName
            var levelSelectAudioName = (AudioName)values[0];
            var audioInstance = AudioManager.Instance;

            audioInstance.Play(AudioType.Narration, levelSelectAudioName);
            while (audioInstance.IsPlaying(AudioType.Narration))
            {
                yield return null;
            }
        }
        
        //
        
        private void OnClick_LevelSelect(Helper helper)
        {
            if(_isEnter)
                return;
            
            if(helper is not HelperLevelSelect levelSelect)
                return;

            _isEnter = true;

            gameAutoQuit.Stop();
            
            Helpers.ForEach(h => h.SetInteract(true));
            
            var selectLevel = levelSelect.Level;
            
            AudioManager.Instance.Stop(AudioType.Narration);
            ProcessManager.Instance.OnLevelSelected(selectLevel);
            ProcessManager.Instance.ToState(ProcessState.Narration);
        }
    }
}
