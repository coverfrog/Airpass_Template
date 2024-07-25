using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CoverFrog
{
    [Serializable]
    public class CfDicValue<TKey, TValue>
    {
        public TKey key;
        public TValue value;
    }
    
    [Serializable]
    public abstract class CfDictionary<TKey, TValue>
    {
        [SerializeField] protected List<CfDicValue<TKey, TValue>> dataList;

        public abstract TValue Get(TKey key);

    }

    [Serializable]
    public class CfDictionaryAtString<TValue> : CfDictionary<string, TValue> where TValue: Object
    {
        public override TValue Get(string key)
        {
            return dataList.FirstOrDefault(v => v.key == key)?.value;
        }
    }

    [Serializable]
    public class CfDictionaryAtEnum<TEnum, TValue> : CfDictionary<TEnum, TValue> where TEnum : Enum where TValue: Object
    {
        public override TValue Get(TEnum key)
        {
            return dataList.FirstOrDefault(v => EqualityComparer<TEnum>.Default.Equals(v.key, key))?.value;
        }
    }
}
