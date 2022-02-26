using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using MoreMountains.TopDownEngine;

namespace HTLibrary.Application
{
    public class AIDeathCallBack : MonoBehaviour
    {
        public Health health;
        public List<string> ItemNames;
        public bool canSpawnTalentState;
        public bool canSpawnEquipItem;
        public bool canSpawnCoin;

        [Header("游戏结束")]
        public float delayShowUI = 1.5f;
        public AudioClip _gameOverClip;
        private SoundManager _soundManager;

        [Header("捡到符文的概率")]
        [Range(0,100)]
        public int _pickStateRate = 5;

        [Header("捡到血球的概率")]
        [Range(0, 100)]
        public int _pickHpRate = 10;

        [Header("捡到装备的概率")]
        [Range(0, 100)]
        public int _pickEquipmentRate = 5;

        [Header("捡到货币的概率")]
        [Range(0, 100)]
        public int _pickCoinRate = 10;

        private void Awake()
        {
            if(health==null)
            {
                health = GetComponent<Health>();
            }

            _soundManager = SoundManager.Instance;
        }

        public void Start()
        {
            if (health != null)
            {
                health.OnDeath += OnDeathCallBack;
            }

        }

        public void OnDestroy()
        {
            if (health != null && health.OnDeath != null)
            {
                health.OnDeath -= OnDeathCallBack;
            }
        }

        /// <summary>
        /// 当前AI死亡后的回调事件
        /// </summary>
        void OnDeathCallBack()
        {
            foreach(var temp in ItemNames)
            {
                if(temp== "HpSugarItem")
                {
                    if(!MathUtility.Percent(_pickHpRate))
                    {
                        continue;
                    }
                }

                GameObject gameObject = PoolManagerV2.Instance.GetInst(temp);
                gameObject.transform.position = transform.position + transform.up * 5;

            }

            if (canSpawnTalentState)
            {
                if(MathUtility.Percent(_pickStateRate))
                {
                    GameObject talentActivated = PoolManagerV2.Instance.GetInst("StateActivated");
                    talentActivated.transform.position = this.transform.position;
                }
            }

            if(canSpawnEquipItem)
            {
                if (MathUtility.Percent(_pickEquipmentRate))
                {
                    GameObject equipItem = PoolManagerV2.Instance.GetInst("EquipmentActivated");
                    equipItem.transform.position = this.transform.position + new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
                }
            }

            if (canSpawnCoin)
            {
                 if(MathUtility.Percent(_pickCoinRate))
                {
                    GameObject coin = PoolManagerV2.Instance.GetInst("Coin");
                    coin.transform.position = this.transform.position;
                }
            }
            TeachingSceneManager teachingSceneManager = FindObjectOfType<TeachingSceneManager>();
            if (teachingSceneManager != null) return;
            StartCoroutine(GameOver());
                
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <returns></returns>

        IEnumerator GameOver()
        {
            //显示游戏结束UI
            if (this.gameObject.tag == Tags.Player)
            {
                _soundManager.PauseBackgroundMusic();
                yield return new WaitForSeconds(delayShowUI);
                UIManager.Instance.PushPanel(UIPanelType.GameOverPanel);
                _soundManager.gameObject.GetComponent<BackgroundMusic>().PlaySpecialBGM(_gameOverClip);
            }
        }

    }

}
