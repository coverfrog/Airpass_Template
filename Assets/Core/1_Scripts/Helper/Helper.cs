using System;
using System.Collections;
using System.Collections.Generic;
using AirpassUnity.VRSports;
using AirpassUnity.VRSportsInputSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CoverFrog
{
    [RequireComponent(typeof(VRSportsButton))]
    [RequireComponent(typeof(VRSportsButtonTest))]
    public abstract class Helper : MonoBehaviour
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
        
        public void AddAction(UnityAction<Helper> action, Helper value) =>
            Btn.onClick.AddListener(() => action?.Invoke(value));

        public void RemoveAction(UnityAction<Helper> action, Helper value) =>
            Btn.onClick.RemoveListener(() => action?.Invoke(value));
        #endregion

        #region > Vr

        private VRSportsButton _vrBtn;

        private VRSportsButton VrBtn => 
            _vrBtn ??= GetComponent<VRSportsButton>();

        private VRSportsButtonTest _vrBtnTest;

        private VRSportsButtonTest VrBtnTest => 
            _vrBtnTest ??= GetComponent<VRSportsButtonTest>();

        #endregion
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetInteract(bool isInteract)
        {
            Btn.enabled = isInteract;
            VrBtn.enabled = isInteract;
            VrBtnTest.enabled = isInteract;
        }
    }
}
