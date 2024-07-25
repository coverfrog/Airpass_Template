using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class DirectionToMove : Direction
    {
        [Header("[ Option ]")] 
        [SerializeField] private Vector2 startPoint;
        [SerializeField] private Vector2 endPoint;

        [Header("[ _ ]")] 
        [SerializeField] private RectTransform rt;
        
        private void OnEnable()
        {
            Stop();
            
            if (playWhenOnEnable)
            {
                Play();
            }
        }

        public override IEnumerator CoPlay()
        {
            yield return null;
        }
    }
}
