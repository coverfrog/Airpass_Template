using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    [RequireComponent(typeof(Image))]
    public class AnimFlipBook : Anim
    {
        [Header("[ AnimFlipBook ]")]
        [SerializeField] private bool isLoop = true;
        [SerializeField] private float duration = 2;
        [SerializeField] private float delay;
        [Space]
        [SerializeField] private AnimFlipBookName flipBookName;
        
        private Image _image;
        private Image Img => _image ??= GetComponent<Image>();

        public override void Init(params object[] values)
        {
           
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            var sprites = Resources.Load<AnimFlipBookDatabase>("FlipBook Database").GetSprites(flipBookName);
            
            for (var t = 0.0f; t < delay; t += Time.deltaTime)
            {
                yield return null;
            }

            var inter = duration / sprites.Count;
            
            for (var i = 0; i < sprites.Count + 1; i++)
            {
                if (i == sprites.Count)
                {
                    if(isLoop)
                        i = 0;
                    else
                    {
                        break;
                    }
                }
                
                Img.sprite = sprites[i];

                for (var timer = 0.0f; timer < inter; timer += Time.deltaTime)
                {
                    yield return null;
                }
            }
        }
    }
}
