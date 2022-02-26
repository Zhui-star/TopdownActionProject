using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
namespace HTLibrary.Application
{
    /// <summary>
    /// 召唤类型
    /// </summary>
    public enum SummonType
    {
        None,
        Summon,
        NonSummon
    }

    /// <summary>
    /// 伙伴控制器
    /// </summary>
    public class PatnerController : MonoBehaviourSimplify
    {
        public int ID;

        PatnerDataManager _patnerDataManager;

        Health _health;

        [Header("组件控制")]
        AIBrain _aiBrain;
        public ButtonActivated _buttonActivated;
        private Collider boxCollider;
        private CharacterController _controller;

        public PatnerType _patnerType;
        public SummonType _summonType;

        public bool TestMode = false;

        SaveManager _saveManager;

        [HideInInspector]
        public int CorresPondingSkillID{get;set;}
        SkillReleaseTrigger _playerSkillReleseTrigger;
        private void Awake()
        {
            _health = GetComponent<Health>();
            _aiBrain = GetComponent<AIBrain>();
            _controller = GetComponent<CharacterController>();
            boxCollider = GetComponent<Collider>();
            _saveManager = SaveManager.Instance;

            _patnerDataManager = PatnerDataManager.Instance;

        }

        private void OnEnable()
        {
            _health.OnDeath += RemoveFollowPatner;

            if (_buttonActivated != null)
            {
                LevelEvent levelEvent = FindObjectOfType<LevelEvent>();
                if (levelEvent)
                {
                    levelEvent.VictoryEvent += ActiveButtonAcitated;
                }
            }
        }

        private void OnDisable()
        {
            SaveHealth();

            _health.OnDeath -= RemoveFollowPatner;

            if (_buttonActivated != null)
            {
                LevelEvent levelEvent = FindObjectOfType<LevelEvent>();
                if (levelEvent)
                {
                    levelEvent.VictoryEvent -= ActiveButtonAcitated;
                }
            }
        }

        private void Start()
        {
            _playerSkillReleseTrigger = CharacterManager.Instance.GetCharacter("Player1").GetComponent<SkillReleaseTrigger>();

            LoadHealth();

            if (TestMode)
            {
                ActivatePatnerController();
            }

            if (!CheckActivate())
            {
                if (_aiBrain != null)
                {
                    _aiBrain.BrainActive = false;
                }

                if (boxCollider != null)
                {
                    boxCollider.enabled = false;
                }

                if (_controller != null)
                {
                    _controller.enabled = false;
                }

            }
            else
            {
                if (_aiBrain != null)
                {
                    _aiBrain.BrainActive = true;
                }

                if (boxCollider != null)
                {
                    boxCollider.enabled = true;
                }

                if (_controller != null)
                {
                    _controller.enabled = true;
                }

                if (_buttonActivated != null)
                {
                    _buttonActivated.DisAciveInteract();
                }

                if (_activeController)
                {
                    return;
                }

                if (EventTypeManager.ContainHTEventType(HTEventType.AddPatner))
                {
                    EventTypeManager.Broadcast<PatnerController>(HTEventType.AddPatner, this);
                }


            }
        }

        bool _activeController = false;

        /// <summary>
        /// 激活伙伴控制
        /// </summary>
        public void ActivatePatnerController()
        {
            if (_patnerDataManager == null)
            {
                _patnerDataManager = PatnerDataManager.Instance;
            }


            _patnerDataManager.AddFollowPatner(ID);

            if (EventTypeManager.ContainHTEventType(HTEventType.AddPatner))
            {
                EventTypeManager.Broadcast<PatnerController>(HTEventType.AddPatner, this);
            }

            if (boxCollider != null)
            {
                boxCollider.enabled = true;
            }

            if (_aiBrain != null)
            {
                _aiBrain.BrainActive = true;
            }

            if (_controller != null)
            {
                _controller.enabled = true;
            }

            if (_buttonActivated != null)
            {
                _buttonActivated.DisAciveInteract();
            }

            _activeController = true;
        }

        /// <summary>
        /// 检查伙伴是否激活
        /// </summary>
        /// <returns></returns>
        bool CheckActivate()
        {
            return _patnerDataManager.CheckFollowPatner(ID);
        }

        /// <summary>
        /// 移除跟随伙伴
        /// </summary>
        void RemoveFollowPatner()
        {
            //Just only one same type summon patner
            if (_summonType == SummonType.Summon)
            {
                _playerSkillReleseTrigger.RestoreCoolDown(CorresPondingSkillID);
            }

            _patnerDataManager.RemoveFollowPatner(ID);

            if (EventTypeManager.ContainHTEventType(HTEventType.RemovePatner))
            {
                EventTypeManager.Broadcast<PatnerController>(HTEventType.RemovePatner, this);
            }
        }

        /// <summary>
        /// 重新设置伙伴目标
        /// </summary>
        /// <param name="newTarget"></param>
        public void ResetTarget(Transform newTarget)
        {
            if (_patnerType != PatnerType.None && _patnerType != PatnerType.Support)
            {
                _aiBrain.Target = newTarget;
            }
        }

        protected override void OnBeforeDestroy()
        {
            if (_summonType == SummonType.Summon)
            {
                RemoveFollowPatner();
            }
        }



        /// <summary>
        /// 保存血量
        /// </summary>
        void SaveHealth()
        {
            if (_summonType == SummonType.Summon) return;

            List<int> followID = _patnerDataManager.GetFollowPatnerIds();

            if (!followID.Contains(ID)) return;


            List<int> indexList = new List<int>();

            for (int i = 0; i < followID.Count; i++)
            {
                if (this.ID == followID[i])
                {
                    indexList.Add(i);
                }
            }

            int index = indexList[0];

            for (int i = 0; i < indexList.Count; i++)
            {
                if (_patnerDataManager.CheckPatnerHealthKey(_saveManager.LoadGameID + gameObject.name + index))
                {
                    index++;
                }
                else
                {
                    break;
                }
            }

            _patnerDataManager.AddPatnerHealthDicts(_saveManager.LoadGameID + gameObject.name + index,
             _health.CurrentHealth);

        }

        /// <summary>
        /// 加载血量
        /// </summary>
        void LoadHealth()
        {
            List<int> followID = _patnerDataManager.GetFollowPatnerIds();

            int index = followID.IndexOf(ID);

            if (_patnerDataManager.CheckPatnerHealthKey(_saveManager.LoadGameID + gameObject.name + index))
            {
                _health.RestoreHPAnim = false;
                _health.CurrentHealth += _patnerDataManager.GetPatnerHealthByKey(_saveManager.LoadGameID +
                 gameObject.name + index);
                _patnerDataManager.RemovePatnerHealthDicts(_saveManager.LoadGameID + gameObject.name + index);
            }
            else
            {
                _health.RestoreHPAnim = false;
                _health.CurrentHealth += _health.MaximumHealth;
            }

        }

        /// <summary>
        /// Active button activated children that responsible interact with player
        /// </summary>
        void ActiveButtonAcitated()
        {
            _buttonActivated.gameObject.SetActive(true);
        }


    }

}
