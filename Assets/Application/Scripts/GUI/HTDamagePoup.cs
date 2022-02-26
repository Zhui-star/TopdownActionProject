using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using System;

namespace HTLibrary.Application
{
    /// <summary>
    /// 伤害数值冒泡
    /// </summary>
    public class HTDamagePoup :MMPoolableObject
    {
        
        public Transform Target { get; set; }
        //private Vector3 mTarget;
        private Vector3 mScreen; //屏幕坐标
        [HideInInspector]
        public string Value; //伤害数值
        public float ContentWidth = 800; //文本宽度
        public float ContentHeight = 500; //文本高度
        private Vector2 mPoint; //GUI坐标
        public GUIStyle guiStyle;
  
        Vector3 offset;

        [Header("字体调配")]
        public int missFontSize;
        public int normalFontSize;
        public int critFontSize;
        [Header("动画参数")]
        public float Speed = 10.0f;
        public float SubsideSpeed = 2.0f;
        public int maxFontSize = 80;
        public int changeFontSpeed = 3;
        public int _inverseChangeFontSpeed = 300;
        private float remeberSpeed;
        float timer;
        public float betweenTime = 0.2f;
        float xOffset;
        protected override void OnEnable()
        {
            base.OnEnable();

            if(Target!=null)
            {
                offset.x = Target.position.x - transform.position.x;
                offset.z = Target.position.z - transform.position.z;
            }
            Initialization();

            TransformScreenPoint();
        }

        protected override void Update()
        {
            base.Update();

            DoAnimation(PositiveAnimation,InversibleAnimation);

            TransformScreenPoint();
        }

        void Initialization()
        {
            timer = 0;
            remeberSpeed = Speed+UnityEngine.Random.Range(-50,51);
            xOffset = UnityEngine.Random.Range(-50, 51);

        }

        void PositiveAnimation()
        {
            transform.Translate(Vector3.up * remeberSpeed * Time.deltaTime);
            remeberSpeed-= remeberSpeed<= 0 ? 0 : Time.deltaTime * SubsideSpeed;
            guiStyle.fontSize += guiStyle.fontSize >= maxFontSize ? 0 :(int)( Time.deltaTime * changeFontSpeed);
        }

        void InversibleAnimation()
        {
            guiStyle.fontSize -= guiStyle.fontSize >= 0 ? (int)(Time.deltaTime *_inverseChangeFontSpeed) :0 ;
        }

        void DoAnimation(Action positiveAnimation,Action negativeAnimation)
        {
             if(remeberSpeed>=0)
            {
                positiveAnimation();
            }
            else
            {
                timer += Time.deltaTime;

                if(timer>=betweenTime)
                {
                    negativeAnimation();
                }
            }
        }

        private void OnGUI()
        {

            if (mScreen.z>0&&!GameManager.Instance.Paused)
            {
                GUI.Label(new Rect(mPoint.x, mPoint.y,ContentWidth,ContentHeight),Value.ToString(),guiStyle);
                
            }
        }

        void TransformScreenPoint()
        {

            if (Target != null)
            {
                transform.position = new Vector3(Target.position.x - offset.x, transform.position.y, 
                Target.position.z - offset.z);
            }

            mScreen = Camera.main.WorldToScreenPoint(transform.position);
            mPoint = new Vector2(mScreen.x+xOffset, Screen.height - mScreen.y);
        }

        public void SetTarget(Transform target)
        {
            Target = target;
        }

        public void SetNormalDamageColor(string value)
        {
            guiStyle.fontSize = normalFontSize;
            guiStyle.normal.textColor = Color.red;
            Value ="-"+ value;
        }

        public void SetCritDamageColor(string value)
        {
            guiStyle.fontSize=critFontSize;
            guiStyle.normal.textColor = Color.yellow;
            Value = "-"+ value;
           
        }

        public void SetMissDamageColor()
        {
            guiStyle.fontSize = missFontSize;
            guiStyle.normal.textColor = Color.white;
            Value = "Miss";
        }

        public void SetHealDamageColor(string value)
        {

            guiStyle.fontSize = missFontSize;
            guiStyle.normal.textColor = Color.green;
            Value = "+" + value;
        }

        /// <summary>
        /// Set abnormal state font style 
        /// </summary>
        public void SetAbnormalColor()
        {
            guiStyle.fontSize=missFontSize;
            guiStyle.normal.textColor=Color.gray;
            Value="免疫";
        }
    }

}
