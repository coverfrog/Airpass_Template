using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CoverFrog
{
    [Serializable]
    public class AnimFlipBookData
    {
        [SerializeField] private AnimFlipBookName name;
        [SerializeField] private string codeName;
        [SerializeField] private Sprite[] spriteArr;

        public AnimFlipBookData(string cName, Sprite[] sArr)
        {
            codeName = cName;
            spriteArr = sArr;
        }

        public void Covert()
        {
            name = (AnimFlipBookName)Enum.Parse(typeof(AnimFlipBookName), codeName);
        }
        
        public AnimFlipBookName CodeName => name;

        public Sprite[] SpriteArr => spriteArr;
    }

    [CreateAssetMenu(menuName = "CoverFrog/FlipBookDatabase", fileName = "FlipBookDatabase")]
    public class AnimFlipBookDatabase : ScriptableObject
    {
        [Header("[ Write ]")] 
        [SerializeField] private string writePath      = @"Assets\Core\1_Scripts\Anim";
        [SerializeField] private string writeNameSpace = "CoverFrog";
        [SerializeField] private string writeEnumName  = "AnimFlipBookName";
        
        [Header("[ Data ]")]
        [SerializeField] private List<AnimFlipBookData> flipBookDataList;


        public IReadOnlyList<Sprite> GetSprites(AnimFlipBookName flipBookName) =>
            flipBookDataList.FirstOrDefault(x => x.CodeName == flipBookName)?.SpriteArr;
            
        #region > Editor
#if UNITY_EDITOR

        [ContextMenu("[ Cf ] Update ")]
        private void Parse()
        {
            // data define
            const string rootKey = "FlipBooks";
            var rootPath = $@"Assets/Resources/{rootKey}";

            // data init
            flipBookDataList = new List<AnimFlipBookData>();

            // data check exist
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            // get keys
            var keys = Directory.GetDirectories(rootPath).Select(x => x.Split('/', '\\')[^1]);
            
            // write enums
            var nameBuilder = new StringBuilder();
            foreach (var key in keys)
            {
                nameBuilder.Append("\t\t").Append(key).Append(",").Append("\n");
            }
            
            var pilBuilder = new StringBuilder();
            pilBuilder.Append($"namespace {writeNameSpace}\n").
                Append("{\n").
                Append($"\tpublic enum {writeEnumName}\n").
                Append("\t{\n").
                Append(nameBuilder.ToString()).
                Append("\t}\n").
                Append("}");

            var saveData = pilBuilder.ToString();
            var savePath = Path.Combine(writePath, $"{writeEnumName}.cs");
            
            
            foreach (var directory in Directory.GetDirectories(rootPath))
            {
                var key = directory.Split('/', '\\')[^1];
                var spriteArr = Resources.LoadAll<Sprite>(Path.Combine(rootKey, key));
                
                flipBookDataList.Add(new AnimFlipBookData(key, spriteArr));
            }
            
            if(File.Exists(savePath))
                File.Delete(savePath);
            
            File.WriteAllText(savePath, saveData);
            
            // save
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [ContextMenu("[ Cf ] Convert ")]
        private void Convert()
        {
            foreach (var data in flipBookDataList)
            {
                data.Covert();
            }
        }
#endif
        #endregion
    }
}
