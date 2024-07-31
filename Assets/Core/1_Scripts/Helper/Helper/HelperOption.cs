using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class HelperOption : Helper
    {
        private Image _img;
        
        private Image Img => 
            _img ??= GetComponent<Image>();

        private Text _txt;

        private Text Txt =>
            _txt ??= transform.GetChild(0).GetComponent<Text>();
        
        public void Set(OptionData optionData)
        {
            Img.sprite = optionData.sprite;
            Txt.color = optionData.txtColor;
        }
    }
}
