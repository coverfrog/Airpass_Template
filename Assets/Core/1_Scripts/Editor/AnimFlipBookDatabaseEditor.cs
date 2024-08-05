#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoverFrog
{
    [CustomEditor(typeof(AnimFlipBookDatabase))]
    public class AnimFlipBookDatabaseEditor : Editor
    {
        
        private AnimFlipBookDatabase _target;

        private void OnEnable()
        {
            _target ??= target as AnimFlipBookDatabase;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SelectWritePath();
        }

        private void SelectWritePath()
        {
            
        }
    }
}
#endif