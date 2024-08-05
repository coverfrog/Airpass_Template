using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class AnimTitleRot : Anim
    {
        [Header("[ AnimTitleRot ]")] 
        [SerializeField] private float dir;
        [SerializeField] private float delay;
        [SerializeField] private float spd;
        [SerializeField] private float rotMaxAngle = 30.0f;
        
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

                var theta = Mathf.Sin(timer) * rotMaxAngle;
                
                transform.localRotation = Quaternion.Euler(0, 0, theta * dir);

                yield return null;
            }
        }
    }
}
