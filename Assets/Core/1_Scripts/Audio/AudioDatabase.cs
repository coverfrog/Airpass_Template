using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using StringBuilder = System.Text.StringBuilder;

namespace CoverFrog
{
    [CreateAssetMenu(menuName = "CoverFrog/Audio Database", fileName = "Audio Database")]
    public class AudioDatabase : ScriptableObject
    {
        [Header("[ Write ]")] 
        public string writePath      = @"Assets\Core\1_Scripts\Anim";
        public string writeNameSpace = "CoverFrog";
        public string writeEnumName  = "AnimFlipBookName";

        [Header("[ Data ]")]
        public List<AudioData> audioClipList;

        public AudioClip Get(AudioName audioName) => audioClipList.FirstOrDefault(c => c.Name == audioName)?.Clip;

        #region > Editor
#if UNITY_EDITOR

        [ContextMenu("[ Cf ] Update ")]
        private void Parse()
        {
            // data define
            const string rootKey = "Audios";
            var rootPath = $@"Assets/Resources/{rootKey}";

            // data init
            audioClipList = new List<AudioData>();

            // data check exist
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            // get keys
            var keys = Directory.GetFiles(rootPath).Select(x => x.Split('/', '\\')[^1]);
            
            // write enums
            var nameBuilder = new StringBuilder();
            foreach (var key in keys)
            {
                var splits = key.Split('.');
                
                if(key.Contains("meta"))
                    continue;
                
                var appendName = splits[0];
                
                nameBuilder.Append("\t\t").Append(appendName).Append(",").Append("\n");
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
            
            
            foreach (var directory in Directory.GetFiles(rootPath))
            {
                var key = directory.Split('/', '\\')[^1];
                
                if(key.Contains("meta"))
                    continue;

                var clipKey = key.Split('.')[0];
                var clip = Resources.Load<AudioClip>(Path.Combine(rootKey, clipKey));
                
                audioClipList.Add(new AudioData(clipKey, clip));
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
            foreach (var data in audioClipList)
            {
                data.Convert();
            }
        }
#endif
        #endregion

        
    }
}
