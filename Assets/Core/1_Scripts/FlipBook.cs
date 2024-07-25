using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CoverFrog
{
    public class FlipBook : MonoBehaviour
    {
        public bool isLoop;
        public bool startWhenEnable;
        public float duration;
        public float delay;
        public List<Sprite> sprites;
        public Image image;

        private void OnEnable()
        {
            if(startWhenEnable)
                Play();
        }

        public void Play()
        {
            _coFlip = CoFlip();
            StartCoroutine(_coFlip);
        }

        public void Stop()
        {
            if(_coFlip != null) StopCoroutine(_coFlip);
            _coFlip = null;
        }

        public void Pause()
        {
            if(_coFlip != null) StopCoroutine(_coFlip);
        }

        public void UnPause()
        {
            if(_coFlip != null) StartCoroutine(_coFlip);
        }

        private IEnumerator _coFlip;

        private IEnumerator CoFlip()
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
                
                image.sprite = sprites[i];

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
