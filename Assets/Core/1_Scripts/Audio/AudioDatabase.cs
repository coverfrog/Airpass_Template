using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace CoverFrog
{
    [CreateAssetMenu(menuName = "CoverFrog/Audio Database", fileName = "Audio Database")]
    public class AudioDatabase : ScriptableObject
    {
        [Header("[ Text ]")] 
        [SerializeField] private string writePath;
        [SerializeField] private string writeNameSpaceName = "CoverFrog";
        [SerializeField] private string writeEnumName = "AudioName";
        
        [Header("[ Data ]")]
        [SerializeField] private List<string> audioPaths;
        [SerializeField] private List<AudioClip> audioClips;

        public AudioClip Get(AudioName audioName) => audioClips.FirstOrDefault(c => c.name == audioName.ToString());
        
        [ContextMenu("[ CoverFrog ] [ Path ] Audio Write Path Select")]
        private void AudioWritePathSelect()
        {
#if UNITY_EDITOR
            var selectedFolderPath = EditorUtility.OpenFolderPanel("Select Path", Application.dataPath, string.Empty);
            if(!selectedFolderPath.Contains(Application.dataPath))
                return;

            writePath = "Assets" + selectedFolderPath.Split("Assets")[^1];
            
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
#endif
        }
        
        [ContextMenu("[ CoverFrog ] [ Path ] Audio Path Add")]
        private void AudioPathAdd()
        {
#if UNITY_EDITOR
            var selectedFolderPath = EditorUtility.OpenFolderPanel("Select Path", Application.dataPath, string.Empty);
            if(!selectedFolderPath.Contains(Application.dataPath))
                return;

            audioPaths.Add("Assets" + writePath.Split("Assets")[^1]);
            
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
#endif

        }

        [ContextMenu("[ CoverFrog ] Clip Parse")]
        private void Parsed()
        {
#if UNITY_EDITOR
            const string pattern = @"[^가-힣a-zA-Z0-9]";

            var clips = new List<AudioClip>();

            foreach (var selectedFolderPath in audioPaths)
            {
                foreach (var selectedClipPath in Directory.GetFiles(selectedFolderPath))
                {
                    if (selectedClipPath.Contains("meta"))
                        continue;

                    if (selectedClipPath.Contains(".mp3") || selectedClipPath.Contains(".wav"))
                    {
                        var clip = AssetDatabase.LoadAssetAtPath<AudioClip>(selectedClipPath);
                        clip.name = Regex.Replace(clip.name, pattern, "");

                        clips.Add(clip);
                    }
                }
            }

            audioClips = clips;
            
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
#endif
        }

        [ContextMenu("[ CoverFrog ] Enum Write")]
        private void EnumWrite()
        {
#if UNITY_EDITOR
            var nameBuilder = new StringBuilder();
            
            foreach (var audioClip in audioClips)
            {
                nameBuilder.Append("\t\t").Append(audioClip.name).Append(",").Append("\n");
            }
            
            var pilBuilder = new StringBuilder();

            pilBuilder.Append($"namespace {writeNameSpaceName}\n").
                Append("{\n").
                Append($"\tpublic enum {writeEnumName}\n").
                Append("\t{\n").
                Append(nameBuilder.ToString()).
                Append("\t}\n").
                Append("}");

            var saveData = pilBuilder.ToString();
            var savePath = Path.Combine(writePath, $"{writeEnumName}.cs");

            if(File.Exists(savePath))
                File.Delete(savePath);
            
            File.WriteAllText(savePath, saveData);
            
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
