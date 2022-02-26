using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace HTLibrary.Editor
{
    /// <summary>
    /// 自动关卡生成器
    /// </summary>
    public class AutoLevelGeneratorWindow : EditorWindow
    {
        private GameObject _managerOfManager;
        private GameObject _htLevelManager;
        private GameObject _level;
        private GameObject _camera;
        private GameObject _directionLight;
        private GameObject _uiCanvas;
        private float _indirectLightIntesity;
        private Material _skyboxMaterial;

        string _pathManagerOfManager = "ManagerOfManager";
        string _pathHTLevelManager = "HTLevelManager";
        string _pathLevel = "Level";
        string _pathCamera = "CameraPath";
        string _pathDirectionLight = "DirectionLight";
        string _pathUICanvas = "UICanvas";
        string _pathIndirectLightIntesity = "IndirectIntesity";
        string _pathSkyBoxMaterial = "SkyBoxMaterial";

        [MenuItem("HTLibrary/AutoLevelGeneratorWindow", false, -220)]
        static void CreatWindow()
        {
            AutoLevelGeneratorWindow window = EditorWindow.CreateWindow<AutoLevelGeneratorWindow>();
            window.Show();

        }
        private void OnGUI()
        {
            _managerOfManager = (GameObject)EditorGUILayout.ObjectField("关卡管理器", _managerOfManager, typeof(GameObject), true);
            _htLevelManager = (GameObject)EditorGUILayout.ObjectField("焕图管理器", _htLevelManager, typeof(GameObject), true);
            _level = (GameObject)EditorGUILayout.ObjectField("关卡结构体", _level, typeof(GameObject), true);
            _camera = (GameObject)EditorGUILayout.ObjectField("关卡相机", _camera, typeof(GameObject), true);
            _directionLight = (GameObject)EditorGUILayout.ObjectField("直射光", _directionLight, typeof(GameObject), true);
            _uiCanvas = (GameObject)EditorGUILayout.ObjectField("UI画布", _uiCanvas, typeof(GameObject), true);
            _indirectLightIntesity = EditorGUILayout.FloatField("间接光强度", _indirectLightIntesity);
            _skyboxMaterial = (Material)EditorGUILayout.ObjectField("天空盒材质", _skyboxMaterial, typeof(Material), true);

            if(GUILayout.Button("生成关卡"))
            {
                CreatPrefab();
                SetIndirectLight();
                CreatEmptyObject();
            }
        }

        /// <summary>
        /// 创建关卡管理
        /// </summary>
        void CreatPrefab()
        {
            PrefabUtility.InstantiatePrefab(_managerOfManager);
            PrefabUtility.InstantiatePrefab(_htLevelManager);
            PrefabUtility.InstantiatePrefab(_level);
            PrefabUtility.InstantiatePrefab(_camera);
            PrefabUtility.InstantiatePrefab(_directionLight);
            PrefabUtility.InstantiatePrefab(_uiCanvas);
        }
        /// <summary>
        /// 设置环境光
        /// </summary>

        void SetIndirectLight()
        {
            RenderSettings.skybox = _skyboxMaterial;
            RenderSettings.ambientIntensity = _indirectLightIntesity;
        }
        
        /// <summary>
        /// 创造空物体引用
        /// </summary>
        void CreatEmptyObject()
        {
            GameObject ai = new GameObject();
            ai.name = "AI";
            GameObject pattern = new GameObject();
            pattern.name = "Pattern";
            GameObject poolParent = new GameObject();
            poolParent.name = "PoolParent";

        }


        private void OnEnable()
        {
            string pathManagerOfManager = EditorPrefs.GetString(_pathManagerOfManager, "");
            string pathHTLevelManager = EditorPrefs.GetString(_pathHTLevelManager, "");
            string pathLevel = EditorPrefs.GetString(_pathLevel, "");
            string pathCamera = EditorPrefs.GetString(_pathCamera, "");
            string pathDirectionLight = EditorPrefs.GetString(_pathDirectionLight, "");
            string pathUICanvas = EditorPrefs.GetString(_pathUICanvas, "");
            float pathIndirectLight = EditorPrefs.GetFloat(_pathIndirectLightIntesity, 0);
            string pathSkyBoxMaterial = EditorPrefs.GetString(_pathSkyBoxMaterial, "");


            _managerOfManager = (GameObject)GetObject<GameObject>(pathManagerOfManager);

            _htLevelManager = (GameObject)GetObject<GameObject>(pathHTLevelManager);

            _level = (GameObject)GetObject<GameObject>(pathLevel);

            _camera = (GameObject)GetObject<GameObject>(pathCamera);

            _directionLight = (GameObject)GetObject<GameObject>(pathDirectionLight);

            _uiCanvas = (GameObject)GetObject<GameObject>(pathUICanvas);

            if (pathIndirectLight > 0)
            {
                _indirectLightIntesity = pathIndirectLight;
            }


            _skyboxMaterial = (Material)GetObject<Material>(pathSkyBoxMaterial);

        }

        /// <summary>
        /// 加载保存
        /// </summary>
        object GetObject<T>(string path) where T : Object
        {
            object o = null;
            if (!string.IsNullOrEmpty(path))
            {
                o = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return o;
        }

        private void OnDisable()
        {
            SaveObject<GameObject>(_pathManagerOfManager, _managerOfManager);
            SaveObject<GameObject>(_pathHTLevelManager, _htLevelManager);
            SaveObject<GameObject>(_pathLevel, _level);
            SaveObject<GameObject>(_pathCamera, _camera);
            SaveObject<GameObject>(_pathDirectionLight, _directionLight);
            SaveObject<GameObject>(_pathUICanvas, _uiCanvas);
            SaveObject<Material>(_pathSkyBoxMaterial, _skyboxMaterial);
            SaveObject<float>(_pathIndirectLightIntesity, null, _indirectLightIntesity);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="o_bject"></param>
        /// <param name="value"></param>
        void SaveObject<T>(string key, Object o_bject, object value = null)
        {

            if (typeof(T) == typeof(GameObject) || typeof(T) == typeof(Material))
            {
                string path = AssetDatabase.GetAssetPath(o_bject);
                EditorPrefs.SetString(key, path);
            }

            if (typeof(T) == typeof(float))
            {
                float t_number = (float)value;
                EditorPrefs.SetFloat(key, t_number);
            }

        }
    }

}
