using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public abstract class Process : CoBehaviour 
    {
        [Header("[ Process ]")]
        [SerializeField] private InProcessState currentState;

        public InProcessState CurrentState => currentState;

        public override void OnDisable()
        {
            base.OnDisable();
            currentState = InProcessState.InActive;
        }
        
        public override void Play(params object[] values)
        {
            base.Play(values);
            currentState = InProcessState.Playing;
        }

        public override void Stop()
        {
            base.Stop();
            currentState = InProcessState.InActive;
        }

        public override void Pause()
        {
            base.Pause();
            currentState = InProcessState.Pause;
        }

        public override void UnPause()
        {
            base.UnPause();
            currentState = InProcessState.UnPauseWait;
        }

        public override void Completed()
        {
            base.Completed();
            currentState = InProcessState.Completed;
        }
        
    }
}
