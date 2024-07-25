using System;
using System.Collections;
using System.Collections.Generic;
using AirpassUnity.VRSports;
using AirpassUnity.VRSportsInputSystem;
using UnityEngine;

namespace CoverFrog
{
    [RequireComponent(typeof(VrButtonTest))]
    public class VrButton : VRSportsButton
    {
        public Action CfOnEnter{ get; set; }

        private int _enterCount;

        protected override void Awake()
        {
            base.Awake();
            
            OnEntered.AddListener(EnterCountUp);
        }

        protected override void Update()
        {
            base.Update();

            if (_enterCount <= 0) 
                return;
            
            CfOnEnter?.Invoke();
            
            _enterCount = 0;
        }

        private void EnterCountUp()
        {
            _enterCount++;
        }
    }
}
