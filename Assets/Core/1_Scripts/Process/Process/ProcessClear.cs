using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoverFrog
{
    public class ProcessClear : Process
    {
        public override IEnumerator CoPlay(params object[] values)
        {
            yield return null;
        }

        public override void Init(params object[] values)
        {
            
        }
    }
}
