using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    [RequireComponent(typeof(Image))]
    public class AnimFlipBook : Anim
    {
        [Header("[ AnimFlipBook ]")]
        [SerializeField] private bool isLoop = true;
        [SerializeField] private float duration = 2;
        [SerializeField] private float delay;
        [Space]
        [SerializeField] private List<Sprite> sprites;
        
        private Image _image;
        private Image Img => _image ??= GetComponent<Image>();

        public override void Init(params object[] values)
        {
           
        }

        protected override IEnumerator CoPlay(params object[] values)
        {
            for (var t = 0.0f; t < delay; t += Time.deltaTime)
            {
                yield return null;
            }

            var inter = duration / sprites.Count;
            
            for (var i = 0; i < sprites.Count + 1; i++)
            {
                if (i == sprites.Count)
                {
                    if(isLoop)
                        i = 0;
                    else
                    {
                        break;
                    }
                }
                
                Img.sprite = sprites[i];

                for (var timer = 0.0f; timer < inter; timer += Time.deltaTime)
                {
                    yield return null;
                }
            }
        }
        
        #region > Util
#if UNITY_EDITOR
        [ContextMenu("[ Cf ] List Add")]
        public async void AudioRootPathAdd()
        {
            // get asset root path
            var assetRootPath = UnityEditor.EditorUtility.OpenFolderPanel("Select Folder", Path.Combine(Application.dataPath, "__Projects"), "");
            assetRootPath = "Assets" + assetRootPath.Substring(Application.dataPath.Length);
            ;
            if (string.IsNullOrEmpty(assetRootPath))
            {
                Debug.Assert(false, "Folder is 'Null' Or ' Not Project In Folder' ");
                return;
            }

            //
            
            var filePaths = new List<string>();
            
            foreach (var filePath in Directory.GetFiles(assetRootPath))
            {
                if (filePath.Contains("meta"))
                {
                    continue;
                }

                var isContinue = true;
                
                foreach (var extend in new string[]{"jpg",".png"})
                {
                    if (!filePath.Contains(extend)) 
                        continue;
                    isContinue = false;
                    break;
                }
                
                if(isContinue)
                    continue;
                
                filePaths.Add(filePath);
            }

            var tList = new List<Sprite>();

            foreach (var filePath in filePaths)
            {
                var t = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(filePath);
                
                if(t == null)
                    continue;
                
                tList.Add(t);
            }
            
            // parse sprite files
            sprites = tList;
            
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif
        #endregion

      
    }
}
