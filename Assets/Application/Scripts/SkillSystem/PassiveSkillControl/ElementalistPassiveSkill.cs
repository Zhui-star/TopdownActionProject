using System.Collections;
using System.Collections.Generic;
using MoreMountains.TopDownEngine;
using UnityEngine;

namespace HTLibrary.Application
{
    public class ElementalistPassiveSkill : MonoBehaviour
    {
        private float probabilityMark; //概率标识
        public float slowTime; //减速的时间
        public bool isRelease; //判断是否释放被动技能
        private float timer; //计时器
        public bool isNormalAttack; //判断是否是普通攻击

        private CharacterFunctionSwitch characterFunctionSwitch;
        public static ElementalistPassiveSkill _instance;
        private DamageOnTouch damage;


        [Header("释放被动技能概率（0--1）")]
        public float probability;
        // Start is called before the first frame update
        void Start()
        {
            probabilityMark = Random.value;
            characterFunctionSwitch = GetComponent<CharacterFunctionSwitch>();
            _instance = this;
            damage = GetComponent<DamageOnTouch>();
            isRelease = false;
            isNormalAttack = false; 
            timer = 0;
        }

        private void Update()
        {
            //Debug.Log(timer);
            if (isRelease == true)
            {
                timer += Time.deltaTime;
            }
            if (timer >= slowTime)
            {
                isRelease = false;
                Debug.Log("Cancel Slow Down!!!");
                timer = 0;
            }
        }

        //元素法师被动技能释放核心
        public void ElementalistPassiveSkillCore()
        {
            probabilityMark = Random.value;
            if (probabilityMark > (1 - probability)&&isNormalAttack==true)
            {
                isRelease = true;
                characterFunctionSwitch.SlowDown(slowTime);
                Debug.Log("Enemy Slow Down!!");
                isNormalAttack = false;
            }

        }
    }
}
