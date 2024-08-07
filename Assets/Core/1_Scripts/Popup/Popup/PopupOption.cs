using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
