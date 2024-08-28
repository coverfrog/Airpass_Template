using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public enum PopupState
    {
        Home,
        Option,
        Result,
    }

    public class PopupManager : Singleton<PopupManager>
    {
        #region > Poups
        private Dictionary<PopupState, Popup> _popups;

        private Dictionary<PopupState, Popup> Popups => _popups ??= InitPopups();

        private Dictionary<PopupState, Popup> InitPopups()
        {
            var newPopups = new Dictionary<PopupState, Popup>();
            
            if (Enum.GetValues(typeof(PopupState)) is not PopupState[] stateArray)
            {
                Debug.Assert(false, "InitProcess : Don't Convert 'PopupState' Array");
                return newPopups;
            }

            foreach (Transform chideTr in transform)
            {
                var childProcess = chideTr.GetComponent<Popup>();
                if (!childProcess)
                {
                    continue;
                }

                var childProcessName = childProcess.GetType().Name;
                foreach (var state in stateArray)
                {
                    if (!string.Equals(childProcessName, $"Popup{state}")) 
                        continue;
                    
                    if (newPopups.TryAdd(state, childProcess))
                    {
                        
                    }
                }
            }
            return newPopups;
        }
        #endregion

        #region > Popup Count
        private uint _popupOpenCount;

        public Action OnCountUp { get; set; }
        public Action OnCountDown { get; set; }

        public void PopupOpenCountUp(Popup popup)
        {
            _popupOpenCount++;

            OnCountUp?.Invoke();
        }

        public void PopupOpenCountDown(Popup popup)
        {
            _popupOpenCount--;

            if (_popupOpenCount == 0)
            {
                OnCountDown?.Invoke();
            }
        }
        #endregion

        public void ActiveAll(bool isActive)
        {
            foreach (var value in Popups.Values)
                value.SetActive(isActive);
        }

        public void ToTitle()
        {
            ActiveAll(false);
        }

        public void PlayTo(PopupState popupState)
        {
            var popup = Popups[popupState];

            switch (popup)
            {
                case PopupHome home:
                    PlayHome(home);
                    break;
                
                case PopupOption option:
                    PlayOption(option);
                    break;
            }
        }

        private void PlayHome(PopupHome home)
        {
            //
            transform.GetChild(2).gameObject.SetActive(false);
            //

            if (home.ActiveInHierarchy)
                home.SetActive(false);
            else
                home.Play();
            
            Popups[PopupState.Option].SetActive(false);
        }

        private void PlayOption(PopupOption option)
        {
            //
            transform.GetChild(2).gameObject.SetActive(false);
            //

            if (option.ActiveInHierarchy)
                option.SetActive(false);
            else
                option.Play();
            
            Popups[PopupState.Home].SetActive(false);
        }
    }
}
