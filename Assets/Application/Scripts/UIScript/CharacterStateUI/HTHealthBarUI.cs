using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using Doodah.Components.Progression;
using HTLibrary.Utility;
using DG.Tweening;
using HTLibrary.Framework;
namespace HTLibrary.Application
{
    public class HTHealthBarUI : MonoBehaviour
    {
        public float delayHpTime = 0.02f;
        public bool isBoss = false;
        private bool isPlayer = false;
        private Image healthFillImg;
        private Image delayHealthFillImg;
        private GameObject levelBarGo;
        public GameObject characterGo;
        private Text levelTxt;
        private Text characterNameTxt;
        private Canvas canvas;
        private float currentHp = 1;
        private float maxHp = 1;

        //Test
        private float monsterHp = 100f;

        private bool AnimChange = true;

        public GameObject _levelOutlineGo;
        public Slider _hpSlider;
        public Slider _delayHpSlider;

        IEnumerator _hideHealthbarIenuermator;

        [Header("隐藏血条")]
        public float delayHide = 3.0f;

        Health _health;

        private Canvas _canvas;

        private HTLevelManager _htLevelManager;
        void Awake()
        {
            levelBarGo = transform.Find("LevelBar").gameObject;
            levelTxt = levelBarGo.transform.Find("Txt/Level_Txt").GetComponent<Text>();
            healthFillImg = transform.Find("HealthBar_Img").GetComponent<Image>();
            delayHealthFillImg = transform.Find("DelayHealthBar_Img").GetComponent<Image>();
            if(isBoss)
            {
                characterNameTxt = transform.Find("characterName_Txt").GetComponent<Text>();
            }

            _health = gameObject.GetComponentInParent<Health>();

            _canvas = GetComponent<Canvas>();
            _htLevelManager = HTLevelManager.Instance;
        }

        void OnEnable()
        {
            if(!isBoss)
            {
                characterGo = gameObject.GetComponentInParent<Character>().gameObject;
            }
            if (characterGo.GetComponent<Character>().CharacterType != Character.CharacterTypes.AI)
            {
                characterGo.GetComponent<Progression>().LevelUpdateEvent += UpdateLevelText;
                isPlayer = true;
            }else
            {
                switch (characterGo.tag)
                {
                    case Tags.Enemies:
                        healthFillImg.color = new Color(1.0f, 107f / 255f, 93f / 255f);
                        delayHealthFillImg.color = new Color(234 / 255f, 180 / 255f, 118 / 255f);
                        break;
                    case Tags.Patner:
                        //TODO 血条颜色
                        break;
                }

                if(_canvas!=null)
                {
                    _canvas.sortingOrder = _htLevelManager.GetHealthBarOder();
                }               
            }

            if (_health == null) return;
            _health.OnHit += ShowHealthBar;
            _health.OnDeath+=HideHealthBar;
        }

        void OnDisable()
        {
            if(isPlayer)
            {
                characterGo.GetComponent<Progression>().LevelUpdateEvent -= UpdateLevelText;
            }

            if (_health == null) return;
            _health.OnHit -= ShowHealthBar;
             _health.OnDeath-=HideHealthBar;
        }

        void Start()
        {
            if (!isBoss)
            {
                canvas = transform.GetComponent<Canvas>();
                canvas.worldCamera = Camera.main;
            }
            if (!isPlayer)
            {
                levelBarGo.SetActive(false);

                if(_levelOutlineGo!=null)
                {
                    _levelOutlineGo.SetActive(false);
                }

                if(!isBoss)
                {
                    transform.localScale = Vector3.zero;
                }             
                
            }else
            {
             
                levelBarGo.SetActive(true);
                levelTxt.text = characterGo.GetComponent<CharacterXP>().GetLevel().ToString();
            }
        }

        void LateUpdate()
        {
            if(!isBoss)
            {
                Vector3 targetPos = transform.position + Camera.main.transform.rotation * Vector3.forward;
                Vector3 targetOrient = Camera.main.transform.rotation * Vector3.up;
                transform.LookAt(targetPos, targetOrient);
            }
            HealthBarChange();
        }

        public void UpdateLevelText(int levelUp)
        {
            levelTxt.text = levelUp.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurretHp"></param>
        /// <param name="MinHp"></param>
        /// <param name="MaxHp"></param>
        /// <param name="Anim">是否执行UI更新时的动画</param>
        public void UpdateHealthBar(float CurretHp, float MinHp, float MaxHp,bool Anim=true)
        {
            currentHp = CurretHp;
            maxHp = MaxHp;

            AnimChange = Anim;
        }

        private void HealthBarChange()
        {
            //healthFillImg.fillAmount = currentHp / maxHp;
            _hpSlider.value = currentHp / maxHp;
            if (_delayHpSlider.value>_hpSlider.value &&AnimChange)
            {
                //delayHealthFillImg.fillAmount -= delayHpTime;
                _delayHpSlider.value -= delayHpTime;
            }
            else
            {
                // delayHealthFillImg.fillAmount = healthFillImg.fillAmount;
                _delayHpSlider.value = _hpSlider.value;
            }
          //  ResetHealthBar(currentHp);
        }

        private void ResetHealthBar(float CurrentHp)
        {
            if(CurrentHp <= 0)
            {
                healthFillImg.fillAmount = 1;
                delayHealthFillImg.fillAmount = 1;
            }
        }

        public void SetTheNameOfCharacter(string name)
        {
            characterNameTxt.text = name;
        }

        //TestBotton
        public void OnClickDamageTest()
        {
            monsterHp -= 20f;
            UpdateHealthBar(monsterHp, 0, 100);
            if(monsterHp <= 0)
            {
                monsterHp = 100f;
            }
        }


        /// <summary>
        /// 显示血条
        /// </summary>
       void ShowHealthBar()
        {
            if (isPlayer || isBoss) return;
            transform.DOScale(1, 0.2f).OnComplete(HideHelathBar());
            
        }

        /// <summary>
        /// 隐藏血条
        /// </summary>
        /// <returns></returns>
        TweenCallback HideHelathBar()
        {
            if(_hideHealthbarIenuermator!=null)
            {
                StopCoroutine(_hideHealthbarIenuermator);
                _delayHpSlider.value = _hpSlider.value;
            }

            _hideHealthbarIenuermator = IHideHealthBar();

            StartCoroutine(_hideHealthbarIenuermator);

            return null;
        }
        
        /// <summary>
        /// 延时隐藏血条
        /// </summary>
        /// <returns></returns>
        IEnumerator IHideHealthBar()
        {
            yield return new WaitForSeconds(delayHide);
            transform.DOScale(0, 0.2f);
            _delayHpSlider.value = _hpSlider.value;
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        void HideHealthBar()
        {
             transform.DOScale(0, 0.2f);
        }
    }
}

