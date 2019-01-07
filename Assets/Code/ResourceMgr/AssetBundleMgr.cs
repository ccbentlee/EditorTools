﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Code.BResourceMgr
{
    public class AssetBundleMgr : IResMgr
    {

        /// <summary>
        /// 资源根目录
        /// </summary>
        private string ResourceRoot = "Resources/Runtime";

        /// <summary>
        /// objsMap
        /// </summary>
        /// <value>The objects map.</value>
        private Dictionary<string, UnityEngine.Object> objsMap { get; set; }

        /// <summary>
        /// 所有资源
        /// </summary>
        /// <value>All resource list.</value>
        private List<string> allResourceList;

        public AssetBundleMgr()
        {
            objsMap = new Dictionary<string, Object>();
            allResourceList = new List<string>();

            Debug.Log(Application.dataPath);

            var root = Application.dataPath + "/" + ResourceRoot;
            allResourceList = Directory.GetFiles(root, "*.*", SearchOption.AllDirectories).ToList();
            Debug.Log(allResourceList.Count);
            for(int i = 0; i < allResourceList.Count; i++)
            {
                if(Application.platform == RuntimePlatform.WindowsEditor)
                {
                    this.allResourceList[i] = allResourceList[i].Replace(root + "\\", "").Replace("\\", "/");
                }

                if(Application.platform == RuntimePlatform.OSXEditor)
                {
                    this.allResourceList[i] = allResourceList[i].Replace("/", "");
                }
            }

        }


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <returns>The load.</returns>
        /// <param name="path">Path.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Load<T>(string path) where T : Object
        {
            if (this.objsMap.ContainsKey(path))
            {
                return objsMap[path] as T;
            }
            else
            {
                path = path + ".";
                var file = this.allResourceList.Find(a => a.Contains(path));
                Debug.Log(file);
                objsMap[path] = AssetDatabase.LoadAssetAtPath<T>("Assets/" + ResourceRoot + "/" + file);
                return GameObject.Instantiate<T>(objsMap[path] as T);
                return objsMap[path] as T;
            }
        }

    }
}
