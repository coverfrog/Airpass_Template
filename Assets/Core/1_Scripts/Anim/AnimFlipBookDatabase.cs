using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CoverFrog
{
    [Serializable]
    public class AnimFlipBookData
    {
        [SerializeField] private string codeName;
        [SerializeField] private List<Sprite> spriteList;
    }

    [CreateAssetMenu(menuName = "CoverFrog/FlipBookDatabase", fileName = "FlipBookDatabase")]
    public class AnimFlipBookDatabase : ScriptableObject
    {
        [Header("[ Path ]")]
        [SerializeField, ReadOnly] private string pathWrite;

        [Header("[ Data ]")]
        [SerializeField] private List<AnimFlipBookData> flipBookDataList;
    }
}
