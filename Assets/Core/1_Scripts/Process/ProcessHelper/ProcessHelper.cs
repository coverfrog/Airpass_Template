using System;
using System.Collections;
using System.Collections.Generic;
using AirpassUnity.VRSports;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CoverFrog
{
    [RequireComponent(typeof(VRSportsButton))]
    public abstract class ProcessHelper : MonoBehaviour
    {
        #region > Button
        private Button _btn;
        private Button Btn => _btn ??= GetComponent<Button>();

        public void AddAction(UnityAction action) =>
            Btn.onClick.AddListener(action);

        public void RemoveAction(UnityAction action) =>
            Btn.onClick.RemoveListener(action);
        
        public void AddAction(UnityAction<int> action, int value) =>
            Btn.onClick.AddListener(() => action?.Invoke(value));

        public void RemoveAction(UnityAction<int> action, int value) =>
            Btn.onClick.RemoveListener(() => action?.Invoke(value));

        public void AddAction(UnityAction<string> action, string value) =>
            Btn.onClick.AddListener(() => action?.Invoke(value));

        public void RemoveAction(UnityAction<string> action, string value) =>
            Btn.onClick.RemoveListener(() => action?.Invoke(value));
        
        public void AddAction(UnityAction<ProcessHelper> action, ProcessHelper value) =>
            Btn.onClick.AddListener(() => action?.Invoke(value));

        public void RemoveAction(UnityAction<ProcessHelper> action, ProcessHelper value) =>
            Btn.onClick.RemoveListener(() => action?.Invoke(value));
        #endregion

        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
