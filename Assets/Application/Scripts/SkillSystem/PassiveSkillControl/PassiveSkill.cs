using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    public class PassiveSkill : MonoBehaviourSimplify
    {
        public int skillId;
        protected SkillReleaseTrigger _skillReleseTrigger;
        protected HTCharacterSkill _htCharacterSkill;
        protected int _index;
        protected virtual void Awake()
        {
            _htCharacterSkill = GetComponent<HTCharacterSkill>();
            _skillReleseTrigger = GetComponent<SkillReleaseTrigger>();
        }

        protected virtual void Start()
        {
            _index = _htCharacterSkill.LearnPassiveSkill(skillId);
        }

        protected override void OnBeforeDestroy()
        {
        }
    }

}
