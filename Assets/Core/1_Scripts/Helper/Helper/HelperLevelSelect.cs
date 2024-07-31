using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class HelperLevelSelect : Helper
    {
        public int Level => transform.GetSiblingIndex();
    }
}
