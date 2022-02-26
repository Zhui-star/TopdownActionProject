using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// 军团骑士被动技能
    /// </summary>
    public class LeagionKnightsLevel2PassiveSkill :PassiveSkill
    {
        public int _maxSlodierCount = 2;
        private SaveManager _saveManager;
        public float _internal = 0.5f;
        protected override void Awake()
        {
            base.Awake();
            _saveManager = SaveManager.Instance;
            _htCharacterSkill = GetComponent<HTCharacterSkill>();
            _skillReleseTrigger = GetComponent<SkillReleaseTrigger>();
        }

        protected override void Start()
        {
           _index=  _htCharacterSkill.LearnPassiveSkill(skillId);

            StartCoroutine(TriggerPassiveSkill());
        }

        /// <summary>
        /// 释放被动技能
        /// </summary>
        IEnumerator TriggerPassiveSkill()
        {
            if(!PlayerPrefs.HasKey(Consts.LeagionKnightPassiveSkill+_saveManager.LoadGameID))
            {
                for(int i=0;i<_maxSlodierCount;i++)
                {
                    _skillReleseTrigger.TriggerSkill(_index);
                    yield return new WaitForSeconds(_internal);
                }

                PlayerPrefs.SetString(Consts.LeagionKnightPassiveSkill + _saveManager.LoadGameID, "1");
            }

        }

        protected override void OnBeforeDestroy()
        {
        }

    
    }

}
