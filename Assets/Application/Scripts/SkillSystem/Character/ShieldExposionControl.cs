using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HTLibrary.Application
{
    public class ShieldExposionControl : MonoBehaviour
    {
        public CharacterConfig characterConfig;

        [Header("增加攻击力")]
        public float attackIncrease;
        public float _attackAddPercent;
        [Header("增加防御力")]
        public float defenceIncrease;
        public float _defenceAddPercent;

        // Start is called before the first frame update

        private void OnEnable()
        {
            attackIncrease = (characterConfig.additiveAttack + characterConfig.characterAttack) * _attackAddPercent;
            _defenceAddPercent = (characterConfig.additiveDefence+ characterConfig.characterDefence) * _defenceAddPercent;
            characterConfig.additiveAttack += attackIncrease;
            characterConfig.additiveDefence += defenceIncrease;
        }

        private void OnDisable()
        {
            characterConfig.additiveAttack -= attackIncrease;
            characterConfig.additiveDefence -= defenceIncrease;
        }
    }

}
