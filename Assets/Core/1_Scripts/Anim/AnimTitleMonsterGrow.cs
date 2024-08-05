using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CoverFrog
{
    public class AnimTitleMonsterGrow : Anim
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
        [SerializeField] private float delay;

        
        private void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }

        public override void Init(params object[] values)
        {
            
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            // 15 30 
            
            // Define Value
            SetScale(0);

            yield return new WaitForSeconds(delay);
            
            var frames = new[] 
                { 0 / 15.0f, 5 / 15.0f, 8 / 15.0f, 11 / 15.0f, 15 / 15.0f};

            var scales = new[]
                { 0.0f, 1.2f, 0.8f, 1.1f, 1.0f };
            
            // Wait Start Frame
            for (var f = 0.0f; f < startFrame / 15.0f; f += Time.deltaTime)
            {
                yield return null;
            }
            
            // Frame Next
            var index = 0;
            var startScale = scales[0];
            
            for (; index < frames.Length; ++index)
            {
                var endFrame = frames[index];
                
                for (var frame = 0.0f; frame < endFrame; frame += Time.deltaTime * 2.85f)
                {
                    var percent = frame / endFrame;
                    var scale = Mathf.Lerp(startScale, scales[index], percent);
                    
                    SetScale(scale);
                    
                    yield return null;
                }

                startScale = scales[index];
            }
        }
    }
}
