using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.TopDownEngine;
using HTLibrary.Application;
using DG.Tweening;
namespace HTLibrary.Utility
{
    /// <summary>
    /// 显示天赋信息
    /// </summary>
    public class TalentShowInfo : MonoBehaviour
    {
        public Image itemIcon;

        public Text levelTxt;

        public Text costTxt;

        public Text descriptionTxt;

        public GameObject studyBtnGo;

        public Text spiritualSourceTxt;

        public Text itemNameTxt;

        //用于文字动画
        private int changeNum; //一次性增加的数字
        private int result = 0;
        private void Start()
        {
            spiritualSourceTxt.text = HTDBManager.Instance.GetCoins().ToString();
            UpdateUI(1);
            TalentSystemManager.Instance.PickedTalentEvent += UpdateUI;
            TalentSystemManager.Instance.UpdateStudyUIEvent += UpdateUI;
        }

        private void OnDestroy()
        {
            TalentSystemManager.Instance.PickedTalentEvent -= UpdateUI;
            TalentSystemManager.Instance.UpdateStudyUIEvent -= UpdateUI;
        }

        /// <summary>
        /// 初始化更新UI
        /// </summary>
        /// <param name="id"></param>
        void UpdateUI(int id)
        {
            TalentItem item = TalentSystemManager.Instance.talentItemDicts.TryGet<int, TalentItem>(id);
            UpdateUI(item);
        }

        /// <summary>
        /// 事件监听更新UI
        /// </summary>
        /// <param name="item"></param>

        void UpdateUI(TalentItem item)
        {
            if (item == null) return;
            itemNameTxt.text = item.Name;
            itemIcon.sprite = item.TalentIcon;
            int level = TalentSystemManager.Instance.ReturnTalentLevel(item.ID);
            levelTxt.text = level+"/"+item.talenCosts.Count.ToString();

            int cost = TalentSystemManager.Instance.ReturnNextLearnCost(item, level);
            costTxt.text = cost == 0 ? "Max level" : cost.ToString();

            descriptionTxt.text = "";

            int i = 1;
            foreach (var temp in item.Descritions)
            {
                if(i<=level)
                {
                    descriptionTxt.text += "<color=whilte>";
                }
                else
                {
                    descriptionTxt.text += "<color=grey>";
                }

                descriptionTxt.text += temp;
                descriptionTxt.text += "\n";
                descriptionTxt.text += "</color>";
                i++;
            }

            studyBtnGo.SetActive(TalentSystemManager.Instance.CanStudyTalent(item.ID));
            //spiritualSourceTxt.text = TalentSystemManager.Instance.spiritualSource.ToString();
            float _v=int.Parse(spiritualSourceTxt.text);
            DOTween.To(()=>_v
            ,v=>_v=v,HTDBManager.Instance.GetCoins(),0.5f).OnUpdate(()=>{
                spiritualSourceTxt.text=Mathf.Floor(_v).ToString();
            }).SetUpdate(true);
        }

        IEnumerator MoneyChangeText(int currentVal,int newVal)
        {
            int minVal = currentVal < newVal ? currentVal : newVal;
            int maxVal = currentVal > newVal ? currentVal : newVal;
            if(maxVal - minVal <= 10)
            {
                changeNum = 1;
            }
            else
            {
                changeNum = (maxVal - minVal) / 10;
            }
            result = currentVal;
            if (currentVal > newVal)
            { 
                for (int i = minVal; i < maxVal; i++)
                {
                    result = result - changeNum;
                    if (result <= newVal)
                    {
                        break;
                    }
                    spiritualSourceTxt.text = result.ToString();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            spiritualSourceTxt.text = newVal.ToString();
            StopCoroutine(MoneyChangeText(currentVal, newVal));
        }
        /// <summary>
        /// 更新UI
        /// </summary>
        void UpdateUI()
        {
            UpdateUI(TalentSystemManager.Instance.Pickeditem);
        }
        /// <summary>
        /// 学习天赋点击
        /// </summary>
        public void StudyTalentClick()
        {
            TalentSystemManager.Instance.StudyTalent(TalentSystemManager.Instance.Pickeditem.ID);
           
            switch(TalentSystemManager.Instance.Pickeditem.talentType)
            {
                case TalentType.AddHP:
                    Health health=CharacterManager.Instance.GetCharacter("Player1").GetComponent<Health>();
                    health.CurrentHealth+=20;
                break;
            }

        }
    }
}

