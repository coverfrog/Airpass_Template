using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


namespace CoverFrog
{
    [Serializable]
    public class OptionData
    {
        public Sprite sprite;
        public Color txtColor;
    }

    [Serializable]
    public class OptionGroup
    {
        [SerializeField] private OptionName optionName;
        [SerializeField] private HelperOption onHelper;
        [SerializeField] private HelperOption offHelper;
        [Space]
        
        private OptionData _onData, _offData;
        
        public OptionName Name => optionName;

        public bool CurrentValue { get; private set; } = true;

        private GameAutoQuit _gameAutoQuit;
        
        public void Init(OptionData onData, OptionData offData, GameAutoQuit gameAutoQuit)
        {
            _onData = onData;
            _offData = offData;
            _gameAutoQuit = gameAutoQuit;
            
            onHelper.AddAction(OnClick_On, onHelper);
            offHelper.AddAction(OnClick_Off, offHelper);
        }

        private void OnClick_On(Helper helper)
        {
            if(CurrentValue)
                return;

            CurrentValue = true;
            
            onHelper.Set(_onData);
            offHelper.Set(_offData);
            
            AudioManager.Instance.SetVolume(optionName, 1.0f);
            
            _gameAutoQuit?.Init();
        }

        private void OnClick_Off(Helper helper)
        {
            if(!CurrentValue)
                return;

            CurrentValue = false;
            
            onHelper.Set(_offData);
            offHelper.Set(_onData);
            
            AudioManager.Instance.SetVolume(optionName, 0.0f);
            
            _gameAutoQuit?.Init();
        }
    }

    public enum OptionName
    {
        Bgm,
        Sfx,
        Narration,
    }

    public class PopupOption : Popup
    {
        [Header("[ PopupOption - Data ]")] 
        [SerializeField] private OptionData onData;
        [SerializeField] private OptionData offData;

        [Header("[ PopupOption - Group ]")] 
        [SerializeField] private GameAutoQuit gameAutoQuit;
        [SerializeField] private List<OptionGroup> optionGroups;

        [SerializeField] GameManager Gm;
        [SerializeField] Button back_btn;
        [SerializeField] RectTransform Back_rect;

        public override void OnEnable()
        {
            base.OnEnable();
            if (Gm.IsPlaying)
            {
                back_btn.gameObject.SetActive(true);
                back_btn.onClick.RemoveAllListeners();
                back_btn.onClick.AddListener(() => { AirpassUnity.VRSports.VRSportsButton.interacting = null; Time.timeScale = 0; transform.gameObject.SetActive(false); Back_rect.gameObject.SetActive(true); });
            }
            else
                back_btn.gameObject.SetActive(false);
        }


        public override void OnDisable()
        {
            PopupManager.Instance.PopupOpenCountDown(this);
        }




        private void Awake()
        {
            optionGroups.ForEach(og => og.Init(onData, offData, gameAutoQuit));
        }

        public override void Init(params object[] values)
        {
            
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            yield return null;
        }
    }
}
