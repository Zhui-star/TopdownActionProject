using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HTLibrary.Utility;
using System.IO;
using UnityEditorInternal;
namespace HTLibrary.Editor
{
    public class SkillSystemSerlizationWindow : EditorWindow
    {
        private SkillUnitList _skillUnitList;

        private string _skillName;
        private string _skillID;
        private string _skillDescription;
        private SkillSpawn _skillSpawn;
        private WhileAttack _whileAttack;
        private Sprite _skillIcon;
        private string AnimationName;
        private bool _isLoop;
        private bool _permitDash;

        private int _coolDown;
        private bool _canReduceSkillCoolDown;

        private AudioClip _castAudiClip;
        private AudioClip _soundClip;

        private string _castEffect;
        private float _castTime;
        private Vector3 _castEffectOffset;

        private string _skillPrefab;
        private Vector3 _skillPositionOffset;
        private float _beforeReleseSkillTime;
        private float _skillDelay;
        private float _skillTime;

        private ChangePosition _changePosition;
        private float _changeValue;
        private float _changeTimer;
        private bool _preventThrough;
        private LayerMask _layerMask;

        private bool canJump;
        private float _jumpForce;
        private float _jumpZSpeed;
        private float _jumpTime;
        private float _startFrezzeAnim;
        private float _frezzeAnimDuration;

        private bool _fly;
        private float _targetHeight;
        private float _upSpeed;
        private float _downSpeed;
        private float _flyDuration;
        private float _downDecelerationMultiple;
        private float _followTargetSpeed;
        private float _upDecelerationMultiple;

        private bool _waitSkillTrigger;

        private string _playableAssetKey;

        private bool _cantCoolDown;
        Vector2 scrollPos;

        [MenuItem("HTLibrary/CreatSkillSystemWindow", false, -210)]
        static void CreateWindow()
        {
            SkillSystemSerlizationWindow window = EditorWindow.CreateWindow<SkillSystemSerlizationWindow>("技能系统序列化");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("技能系统配置表 :");
            _skillUnitList = (SkillUnitList)EditorGUILayout.ObjectField(_skillUnitList, typeof(SkillUnitList), true,
                GUILayout.Width(200));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("生成技能系统Json文件"))
            {
                SaveObjectToJson();
            }

            GUILayout.Space(20);
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            GUIStyle _style = new GUIStyle();
            _style.fontSize = 20;
            _style.fontStyle = FontStyle.BoldAndItalic;
            GUILayout.Label("属性配置", _style);
            _skillName = EditorGUILayout.TextField("技能名:", _skillName);
            _skillID = EditorGUILayout.TextField("技能ID:", _skillID);
            _skillDescription = EditorGUILayout.TextField("技能描述:", _skillDescription, GUILayout.Width(500));
            _skillSpawn = (SkillSpawn)EditorGUILayout.EnumPopup("孵化类型:", _skillSpawn, GUILayout.Width(500));
            _whileAttack = (WhileAttack)EditorGUILayout.EnumPopup("当攻击时:", _whileAttack, GUILayout.Width(500));
            _skillIcon = (Sprite)EditorGUILayout.ObjectField("技能Icon:", _skillIcon, typeof(Sprite), true, GUILayout.Width(500));
            AnimationName = EditorGUILayout.TextField("技能动画名称:", AnimationName);
            _isLoop = EditorGUILayout.Toggle("动画是否循环:", _isLoop);
            _permitDash = EditorGUILayout.Toggle("是否允许冲刺:", _permitDash);

            GUILayout.Space(20);
            GUILayout.Label("冷却", _style);
            _coolDown = EditorGUILayout.IntField("冷却时间:", _coolDown);
            _canReduceSkillCoolDown = EditorGUILayout.Toggle("能否减少冷却时间:", _canReduceSkillCoolDown);

            GUILayout.Space(20);
            GUILayout.Label("音频", _style);
            _castAudiClip = (AudioClip)EditorGUILayout.ObjectField("读条音效:", _castAudiClip, typeof(AudioClip), true);
            _soundClip = (AudioClip)EditorGUILayout.ObjectField("技能音效:", _soundClip, typeof(AudioClip), true);

            GUILayout.Space(20);
            GUILayout.Label("读条属性", _style);
            _castEffect = EditorGUILayout.TextField("读条特效:", _castEffect);
            _castTime = EditorGUILayout.FloatField("读条时间", _castTime);
            _castEffectOffset = EditorGUILayout.Vector3Field("读条特效偏移", _castEffectOffset, GUILayout.Width(1000));

            GUILayout.Space(20);
            GUILayout.Label("技能属性", _style);
            _skillPrefab = EditorGUILayout.TextField("技能特效:", _skillPrefab);
            _skillPositionOffset = EditorGUILayout.Vector3Field("技能位置偏移", _skillPositionOffset);
            _beforeReleseSkillTime = EditorGUILayout.FloatField("动画冻结开始时间", _beforeReleseSkillTime);
            _skillDelay = EditorGUILayout.FloatField("技能释放开始时间", _skillDelay);
            _skillTime = EditorGUILayout.FloatField("技能剩余时间", _skillTime);

            GUILayout.Space(20);
            GUILayout.Label("位移选择项", _style);
            _changePosition = (ChangePosition)EditorGUILayout.EnumPopup("是否产生位移", _changePosition);
            _changeValue = EditorGUILayout.FloatField("位移距离", _changeValue);
            _changeTimer = EditorGUILayout.FloatField("位移时间", _changeTimer);
            _preventThrough = EditorGUILayout.Toggle("是否能被阻挡", _preventThrough);
            //_layerMask =EditorGUILayout.LayerField("阻挡层", _layerMask);
            LayerMask tempMask = EditorGUILayout.MaskField("阻挡层",InternalEditorUtility.LayerMaskToConcatenatedLayersMask(_layerMask),
                InternalEditorUtility.layers);
            _layerMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
            GUILayout.Space(20);

            GUILayout.Label("跳跃选项", _style);
            canJump = EditorGUILayout.Toggle("能否跳跃", canJump);
            _jumpForce = EditorGUILayout.FloatField("跳跃力度", _jumpForce);
            _jumpZSpeed = EditorGUILayout.FloatField("跳跃向前的速度", _jumpZSpeed);
            _jumpTime = EditorGUILayout.FloatField("跳跃时间", _jumpTime);
            _startFrezzeAnim = EditorGUILayout.FloatField("跳跃时开始冻结动画时间", _startFrezzeAnim);
            _frezzeAnimDuration = EditorGUILayout.FloatField("冻结动画时间", _frezzeAnimDuration);

            GUILayout.Space(20);
            GUILayout.Label("飞行选项", _style);
            _fly = EditorGUILayout.Toggle("能否飞行", _fly);
            _targetHeight = EditorGUILayout.FloatField("飞行高度", _targetHeight);
            _upSpeed = EditorGUILayout.FloatField("向上飞行速度", _upSpeed);
            _downSpeed = EditorGUILayout.FloatField("降落速度", _downSpeed);
            _flyDuration = EditorGUILayout.FloatField("飞行时间", _flyDuration);
            _downDecelerationMultiple = EditorGUILayout.FloatField("降落加速度", _downDecelerationMultiple);
            _followTargetSpeed = EditorGUILayout.FloatField("跟随目标时间", _followTargetSpeed);
            _upDecelerationMultiple = EditorGUILayout.FloatField("上升加减速度", _upDecelerationMultiple);

            GUILayout.Space(20);
            GUILayout.Label("技能二次释放", _style);
            _waitSkillTrigger = EditorGUILayout.Toggle("是否允许二次释放", _waitSkillTrigger);
            GUILayout.Label("Timeline 配置",_style);
            _playableAssetKey=EditorGUILayout.TextField("Timeline asset key",_playableAssetKey);

            if (GUILayout.Button("添加当前ID技能", GUILayout.Width(200)))
            {
                AddSkillItem();
            }

            if (GUILayout.Button("删除当前ID技能", GUILayout.Width(200)))
            {
                RemoveSkillItem();
            }

            if (GUILayout.Button("加载当前ID技能", GUILayout.Width(200)))
            {
                LoadSkillItemByID();
            }

            if (GUILayout.Button("更新当前ID技能", GUILayout.Width(200)))
            {
                ChangeSkillItem();
            }



            EditorGUILayout.EndScrollView();
        }


        void SaveObjectToJson()
        {
            string dataPath = UnityEngine.Application.dataPath + "/Application/StreamingFile/SkillSystem.Json";
            string jsonStr = EditorJsonUtility.ToJson(_skillUnitList, true);

            using (StreamWriter wrtie = new StreamWriter(dataPath))
            {
                wrtie.Write(jsonStr);
            }

            Debug.Log("生成成功  路径: " + dataPath);
        }

        void AddSkillItem()
        {
            SkillUnit skillUnit = new SkillUnit
            {
                skillName = _skillName,
                skillID = int.Parse(_skillID),
                skillDescription = _skillDescription,
                skillSpawn = _skillSpawn,
                whileAttack = _whileAttack,
                skillIcon = _skillIcon,
                animationName = this.AnimationName,
                IsLoop = this._isLoop,
                permitDash = this._permitDash,

                cooldown = this._coolDown,
                CanReduceSkillCoolDown = this._canReduceSkillCoolDown,

                castSoundEffect = this._castAudiClip,
                soundEffect = this._soundClip,

                castEffect = this._castEffect,
                castTime = this._castTime,
                CastEffctOffset = this._castEffectOffset,

                skillPrefab = this._skillPrefab,
                skillPositionOffset = this._skillPositionOffset,
                BeforeReleseSkillTime = this._beforeReleseSkillTime,
                skillDelay = this._skillDelay,
                skillTime = this._skillTime,

                changePosition = this._changePosition,
                changeValue = this._changeValue,
                changeTimer = this._changeTimer,
                prventThrough = this._preventThrough,
                layerMask = this._layerMask,

                CanJump = this.canJump,
                JumpForce = this._jumpForce,
                JumpZSpeed = this._jumpZSpeed,
                JumpTime = this._jumpTime,
                StartFrezzeAnim = this._startFrezzeAnim,
                FrezzeAnimDuration = this._frezzeAnimDuration,

                Fly = this._fly,
                TargetHeight = this._targetHeight,
                UpSpeed = this._upSpeed,
                DownSpeed = this._downSpeed,
                FlyDuration = this._flyDuration,
                DownDeclerationMultiple = this._downDecelerationMultiple,
                FollowTargetSpeed = this._followTargetSpeed,
                UpDeclerationMultiple = this._upDecelerationMultiple,

                WaitSkillTrigger = this._waitSkillTrigger,
                _playableAssetKey=this._playableAssetKey
                

            };

            EditorUtility.SetDirty(_skillUnitList);
            _skillUnitList.skillList.Add(skillUnit);
            AssetDatabase.SaveAssets();
            Debug.Log("添加 ID " + this._skillID + "成功");
        }

        void RemoveSkillItem()
        {
            List<SkillUnit> skillUnitList = _skillUnitList.skillList;
            EditorUtility.SetDirty(_skillUnitList);
            foreach (var skillUnit in skillUnitList)
            {
                if (skillUnit.skillID == int.Parse(this._skillID))
                {
                    skillUnitList.Remove(skillUnit);
                    Debug.Log("删除 ID " + this._skillID + "成功");
                    break;
                }
            }
            AssetDatabase.SaveAssets();
        }

        void LoadSkillItemByID()
        {
            List<SkillUnit> skillUnitList = _skillUnitList.skillList;
            foreach (var skillUnit in skillUnitList)
            {
                if (skillUnit.skillID == int.Parse(this._skillID))
                {

                    _skillName = skillUnit.skillName;
                    _skillID = skillUnit.skillID.ToString();
                    _skillDescription = skillUnit.skillDescription;
                    _skillSpawn = skillUnit.skillSpawn;
                    _whileAttack = skillUnit.whileAttack;
                    _skillIcon = skillUnit.skillIcon;
                    this.AnimationName = skillUnit.animationName;
                    this._isLoop = skillUnit.IsLoop;
                    this._permitDash = skillUnit.permitDash;

                    this._coolDown = (int)skillUnit.cooldown;
                    this._canReduceSkillCoolDown = skillUnit.CanReduceSkillCoolDown;

                    this._castAudiClip = skillUnit.castSoundEffect;
                    this._soundClip = skillUnit.soundEffect;

                    this._castEffect = skillUnit.castEffect;
                    this._castTime = skillUnit.castTime;
                    this._castEffectOffset = skillUnit.CastEffctOffset;

                    this._skillPrefab = skillUnit.skillPrefab;
                    this._skillPositionOffset = skillUnit.skillPositionOffset;
                    this._beforeReleseSkillTime = skillUnit.BeforeReleseSkillTime;
                    this._skillDelay = skillUnit.skillDelay;
                    this._skillTime = skillUnit.skillTime;

                    this._changePosition = skillUnit.changePosition;
                    this._changeValue = skillUnit.changeValue;
                    this._changeTimer = skillUnit.changeTimer;
                    this._preventThrough = skillUnit.prventThrough;
                    this._layerMask=  skillUnit.layerMask;
                    // Debug.Log("层级"+skillUnit.layerMask.value);
                    this.canJump = skillUnit.CanJump;
                    this._jumpForce = skillUnit.JumpForce;
                    this._jumpZSpeed = skillUnit.JumpZSpeed;
                    this._jumpTime = skillUnit.JumpTime;
                    this._startFrezzeAnim = skillUnit.StartFrezzeAnim;
                    this._frezzeAnimDuration = skillUnit.FrezzeAnimDuration;

                    this._fly = skillUnit.Fly;
                    this._targetHeight = skillUnit.TargetHeight;
                    this._upSpeed = skillUnit.UpSpeed;
                    this._downSpeed = skillUnit.DownSpeed;
                    this._flyDuration = skillUnit.FlyDuration;
                    this._downDecelerationMultiple = skillUnit.DownDeclerationMultiple;
                    this._followTargetSpeed = skillUnit.FollowTargetSpeed;
                    this._upDecelerationMultiple = skillUnit.UpDeclerationMultiple;

                    this._waitSkillTrigger = skillUnit.WaitSkillTrigger;
                    this._playableAssetKey=skillUnit._playableAssetKey;

                    Debug.Log("加载 技能成功 ID " + this._skillID);
                    break;
                }
            }
        }

        void ChangeSkillItem()
        {
            List<SkillUnit> skillUnitList = _skillUnitList.skillList;
            EditorUtility.SetDirty(_skillUnitList);
            for (int i = 0; i < skillUnitList.Count; i++)
            {
                if (skillUnitList[i].skillID == int.Parse(this._skillID))
                {
                    SkillUnit skillUnit = new SkillUnit
                    {
                        skillName = _skillName,
                        skillID = int.Parse(_skillID),
                        skillDescription = _skillDescription,
                        skillSpawn = _skillSpawn,
                        whileAttack = _whileAttack,
                        skillIcon = _skillIcon,
                        animationName = this.AnimationName,
                        IsLoop = this._isLoop,
                        permitDash = this._permitDash,

                        cooldown = this._coolDown,
                        CanReduceSkillCoolDown = this._canReduceSkillCoolDown,

                        castSoundEffect = this._castAudiClip,
                        soundEffect = this._soundClip,

                        castEffect = this._castEffect,
                        castTime = this._castTime,
                        CastEffctOffset = this._castEffectOffset,

                        skillPrefab = this._skillPrefab,
                        skillPositionOffset = this._skillPositionOffset,
                        BeforeReleseSkillTime = this._beforeReleseSkillTime,
                        skillDelay = this._skillDelay,
                        skillTime = this._skillTime,

                        changePosition = this._changePosition,
                        changeValue = this._changeValue,
                        changeTimer = this._changeTimer,
                        prventThrough = this._preventThrough,
                        layerMask = this._layerMask,

                        CanJump = this.canJump,
                        JumpForce = this._jumpForce,
                        JumpZSpeed = this._jumpZSpeed,
                        JumpTime = this._jumpTime,
                        StartFrezzeAnim = this._startFrezzeAnim,
                        FrezzeAnimDuration = this._frezzeAnimDuration,

                        Fly = this._fly,
                        TargetHeight = this._targetHeight,
                        UpSpeed = this._upSpeed,
                        DownSpeed = this._downSpeed,
                        FlyDuration = this._flyDuration,
                        DownDeclerationMultiple = this._downDecelerationMultiple,
                        FollowTargetSpeed = this._followTargetSpeed,
                        UpDeclerationMultiple = this._upDecelerationMultiple,

                        WaitSkillTrigger = this._waitSkillTrigger,
                        _playableAssetKey=this._playableAssetKey
                    };

                    skillUnitList[i] = skillUnit;
                    Debug.Log("更新 ID " + this._skillID + "成功");
                    break;
                }
            }
            AssetDatabase.SaveAssets();
        }
    }

}
