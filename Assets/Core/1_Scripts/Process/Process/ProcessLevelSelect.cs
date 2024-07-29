using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class ProcessLevelSelect : Process
    {
        #region > Helpers
        private List<ProcessHelperLevelSelect> _helpers;

        private List<ProcessHelperLevelSelect> Helpers =>
            _helpers ??= HelpersInit();

        private List<ProcessHelperLevelSelect> HelpersInit()
        {
            var helpers = new List<ProcessHelperLevelSelect>();
            
            foreach (Transform tr in transform.GetChild(2))
            {
                var helper = tr.GetComponent<ProcessHelperLevelSelect>();
                if (helper != null)
                {
                    helpers.Add(helper);
                }
            }

            return helpers;
        }
        #endregion

        public override IEnumerator CoPlay()
        {
            yield return null;
        }
    }
}
