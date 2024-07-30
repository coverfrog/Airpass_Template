using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class ProcessHelperLevelSelect : ProcessHelper
    {
        public int Level => transform.GetSiblingIndex();
    }
}
