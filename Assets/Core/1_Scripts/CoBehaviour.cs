using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public abstract class CoBehaviour : MonoBehaviour
    {

        private IEnumerator _coPlay;

        public abstract IEnumerator CoPlay();

        public virtual void Play()
        {
            _coPlay = CoPlay();
            StartCoroutine(_coPlay);
            ;        }

        public virtual void Stop()
        {
            if(_coPlay != null) StopCoroutine(_coPlay);
            _coPlay = null;
        }

        public virtual void Pause()
        {
            if(_coPlay != null) StopCoroutine(_coPlay);
        }

        public virtual void UnPause()
        {
            if(_coPlay != null) StartCoroutine(_coPlay);
        }

        public virtual void Completed()
        {
            if(_coPlay != null) StopCoroutine(_coPlay);
            _coPlay = null;
        }

        public virtual void OnDisable()
        {
            Stop();
        }
    }
}
