using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class ProcessLevelSelect : Process
    {
        #region > Helpers
        private List<ProcessHelperLevelSelect> _helpers;

        private List<ProcessHelperLevelSelect> Helpers =>
            _helpers ??= HelpersInit();

        private List<ProcessHelperLevelSelect> HelpersInit()
        {
            var helpers = new List<ProcessHelperLevelSelect>();
            
            foreach (Transform tr in transform.GetChild(2))
            {
                var helper = tr.GetComponent<ProcessHelperLevelSelect>();
                
                if (helper == null)
                    continue;
                
                helper.AddAction(OnEnter, helper);
                helpers.Add(helper);
            }

            return helpers;
        }
        #endregion

        private Text _levelDescriptionText;
        private Text LevelDescriptionText => _levelDescriptionText ??=
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        
        private bool _isEnter;

        public override void OnEnable()
        {
            base.OnEnable();

            _isEnter = false;
        }

        public override void Init(params object[] values)
        {
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
            
            // _
            gameObject.SetActive(true);
        }

        private void OnEnter(ProcessHelper helper)
        {
            if(_isEnter)
                return;
            
            if(helper is not ProcessHelperLevelSelect levelSelect)
                return;

            _isEnter = true;
            
            
            var selectLevel = levelSelect.Level;
            
            AudioManager.Instance.Stop(AudioType.Narration);
            
            ProcessManager.Instance.OnLevelSelected(selectLevel);
            ProcessManager.Instance.ToState(ProcessState.ConceptVideo);
        }

        public override IEnumerator CoPlay(params object[] values)
        {
            // [0] level Select AudioName
            var levelSelectAudioName = (AudioName)values[0];
            var audioInstance = AudioManager.Instance;

            audioInstance.Play(AudioType.Narration, levelSelectAudioName);
            while (audioInstance.IsPlaying(AudioType.Narration))
            {
                yield return null;
            }
        }
    }
}
