using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public abstract class GameManager : MonoBehaviour
    {
        
        public abstract void Init();



        public abstract void Play(int selectLevel);

        private IEnumerator _coCompleted;

        protected abstract IEnumerator CoCompleted(bool isWin);
        
        public void Completed(bool isWin)
        {
            _coCompleted = CoCompleted(isWin);
            StartCoroutine(_coCompleted);
        }
    }
}
