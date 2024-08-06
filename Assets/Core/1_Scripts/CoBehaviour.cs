using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CoverFrog
{
    public abstract class CoBehaviour : MonoBehaviour
    {
        [Header("[ CoBehaviour ]")]
        [SerializeField] private bool playWhenEnable;
        [SerializeField] private bool activeOffWhenStop;
        [SerializeField] private bool activeOffWhenComplete;
        
        private IEnumerator _coPlay;

        public abstract void Init(params object[] values);

        protected abstract IEnumerator CoPlay(params object[] values);

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public bool ActiveInHierarchy => gameObject.activeInHierarchy;

        public bool IsPlaying => _coPlay != null;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public virtual void Play(params object[] values)
        {
            if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }

            _coPlay = CoPlay(values);
            StartCoroutine(_coPlay);
        }

        public virtual void Stop()
        {
            if(_coPlay != null) StopCoroutine(_coPlay);
            _coPlay = null;

            if (activeOffWhenStop && gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void Pause()
        {
            if(_coPlay != null) StopCoroutine(_coPlay);
        }

        public virtual void UnPause()
        {
            if(_coPlay != null && gameObject.activeInHierarchy) StartCoroutine(_coPlay);
        }

        public virtual void Completed()
        {
            if(_coPlay != null) StopCoroutine(_coPlay);
            _coPlay = null;

            if (activeOffWhenComplete && gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void OnEnable()
        {
            if (playWhenEnable)
            {
                Play();
            }
        }

        public virtual void OnDisable()
        {
            Stop();
        }
    }
}
