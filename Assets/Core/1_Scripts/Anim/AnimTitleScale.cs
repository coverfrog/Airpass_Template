
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class AnimTitleScale : Anim
    {
        [Header("[ AnimTitleScale ]")] 
        [SerializeField] private float delay = 1.0f;
        [SerializeField] private float spd = 1.0f;
        [SerializeField] private float minScale;
        [SerializeField] private float maxScale;
        
        public override void Init(params object[] values)
        {
            
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            yield return new WaitForSeconds(delay);
            
            var timer = 0.0f;

            while (true)
            {
                timer += Time.deltaTime * spd;
                var value = Mathf.Cos(timer);
                    
                transform.localScale = Vector3.one * (minScale + (maxScale - minScale) * value);

                yield return null;
            }
        }
    }
}
