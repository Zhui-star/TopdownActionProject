using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HTLibrary.Framework;
using System.IO;
namespace HTLibrary.Editor
{
    ///对象池自动序列化
    public class ObjectPoolSerizationWindow : EditorWindow
    {
        public GameObjectPoolList _objectPoolList;
        public GameObject _prefab;
        private string _prefabName;
        private string _maxCount;
        [MenuItem("HTLibrary/CreateObjectPoolWindow", false, -200)]
        static void CreateWindow()
        {
            ObjectPoolSerizationWindow window = EditorWindow.CreateWindow<ObjectPoolSerizationWindow>("对象池序列化");
            window.Show();
        }

        /// <summary>
        /// GUI布局
        /// </summary>

        private void OnGUI()
        {

            GUILayout.BeginHorizontal();
            GUILayout.Label("对象池配置表 :");
            _objectPoolList= (GameObjectPoolList)EditorGUILayout.ObjectField(_objectPoolList, typeof(GameObjectPoolList),
            true, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("生成Json文件"))
            {
                SaveObjectJson(_objectPoolList);

            }

            GUILayout.Space(20);

            _prefabName=  EditorGUILayout.TextField("对象池索引(名字): ", _prefabName);

            GUILayout.BeginHorizontal();
            GUILayout.Label("对象池预制体:");
            _prefab = (GameObject)EditorGUILayout.ObjectField(_prefab, typeof(GameObject), true, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            _maxCount= EditorGUILayout.TextField("最大个数: ", _maxCount);

            if (GUILayout.Button("创建对象池"))
            {
                CreateObjectPool(_objectPoolList, _prefabName,_prefab,int.Parse(_maxCount));
            }

            GUILayout.Space(20);

            if(GUILayout.Button("移除对象池(根据对象池索引名字)"))
            {
                RemoveObjectPoolByName(_objectPoolList, _prefabName);
            }
        }

        /// <summary>
        /// 保存Json文件
        /// </summary>
        /// <param name="gameObjectPoolList"></param>
        void SaveObjectJson(GameObjectPoolList gameObjectPoolList)
        {
            string jsonPath = UnityEngine.Application.dataPath+ "/Application/StreamingFile" + "/ObjectPool.json";

            List<GameObjectPool> objectPoolList = CreateSave(gameObjectPoolList);
            string saveJson = EditorJsonUtility.ToJson(gameObjectPoolList,true);
            Debug.Log("Save Json:" + saveJson);

            using (StreamWriter sw = new StreamWriter(jsonPath))
            {
                sw.Write(saveJson);
            }

            Debug.Log("写入成功路径: "+jsonPath);
        }
        
        /// <summary>
        /// 创建保存对象
        /// </summary>
        /// <param name="objectPoolList"></param>
        /// <returns></returns>
        List<GameObjectPool> CreateSave(GameObjectPoolList objectPoolList)
        {
            return objectPoolList.poolList;
        }

        /// <summary>
        /// 创建对象池
        /// </summary>
        /// <param name="_poolList"></param>
        /// <param name="Name"></param>
        /// <param name="Prefab"></param>
        void CreateObjectPool(GameObjectPoolList _poolList,string Name,GameObject Prefab,int MaxCount)
        {
            GameObjectPool gameObjectPool = new GameObjectPool
            {
                name = Name,
                prefab = Prefab,
                maxAmount = MaxCount,
                 
            };
            EditorUtility.SetDirty(_poolList);
            _poolList.poolList.Add(gameObjectPool);
            AssetDatabase.SaveAssets();
            Debug.Log("创建成功");
        }


        /// <summary>
        /// 移除指定对象池
        /// </summary>
        /// <param name="_poolList"></param>
        /// <param name="Name"></param>

        void RemoveObjectPoolByName(GameObjectPoolList _poolList,string Name)
        {
            List<GameObjectPool> gameObjectPool = _poolList.poolList;

            EditorUtility.SetDirty(_poolList);
            foreach (var o_bject in gameObjectPool)
            {
                if(string.Equals(Name,o_bject.name))
                {
                    gameObjectPool.Remove(o_bject);
                    Debug.Log("移除成功");
                    break;
                }
            }
            AssetDatabase.SaveAssets();
        }
    }

}
