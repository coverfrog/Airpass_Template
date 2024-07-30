using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class AnimTitleMonsterGrow : AnimHelper
    {
        // Notice : start frame 
        // mon1 6
        // mon2 12
        // mon3 4
        // mon4 10
        // mon5 8
        // mon6 10

        [Header("[ Monster Grow ]")] 
        [SerializeField] private int startFrame;

        
        private void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }

        public override IEnumerator CoPlay(params object[] values)
        {
            // 15 30 
            
            // Define Value
            SetScale(0);
            
            var frames = new[] 
                { 0, 5, 8, 11, 15 } ;

            var scales = new[]
                { 0.0f, 1.2f, 0.8f, 1.1f, 1.0f };
            
            // Wait Start Frame
            for (var i = 0; i < startFrame; ++i)
            {
                yield return null;
            }
            
            // Frame Next
            var index = 0;
            var startScale = scales[0];
            
            for (; index < frames.Length; ++index)
            {
                var endFrame = frames[index] * 4;
                
                for (var frame = 0; frame < endFrame; ++frame)
                {
                    var percent = (float)frame / endFrame;
                    var scale = Mathf.Lerp(startScale, scales[index], percent);
                    
                    SetScale(scale);
                    
                    yield return null;
                }

                startScale = scales[index];
            }
        }
    }
}
