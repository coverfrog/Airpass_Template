using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public enum DirectionState
    {
        InActive,
        Playing,
        Pause,
        UnPauseWait,
        Completed
    }

    public abstract class Direction : CoBehaviour
    {
        [SerializeField] protected bool playWhenOnEnable = false;
        [SerializeField] protected DirectionState currentState;
        
        public DirectionState CurrentState => currentState;

        public override void Play()
        {
            base.Play();
            currentState = DirectionState.Playing; 
        }

        public override void Stop()
        {
            base.Stop();
            currentState = DirectionState.InActive;
        }

        public override void Pause()
        {
            base.Pause();
            currentState = DirectionState.Pause;
        }

        public override void UnPause()
        {
            base.UnPause();
            currentState = DirectionState.UnPauseWait;
        }

        public override void Completed()
        {
            base.Completed();
            currentState = DirectionState.Completed;
        }
    }
}
