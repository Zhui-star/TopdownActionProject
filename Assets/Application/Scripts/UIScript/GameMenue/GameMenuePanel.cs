using System.Collections;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using DG.Tweening;
namespace HTLibrary.Application
{
    /// <summary>
    /// 战斗场景中 主UI控制
    /// </summary>
    public class GameMenuePanel : BasePanel
    {
        private CanvasGroup canvasGroup;

        public MenueWeaponSlot menueSlot;

        public Transform parent;

        public UnityEngine.UI.Text currencyTxt;

        private MainManager _mainManager;
        public GameObject _testPanel;

        private int result; //金钱变化
        private int changeNum; //一次性增加的数字
        private IEnumerator Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();

            _mainManager = MainManager.Instance;
            switch(_mainManager.Mode)
            {
                case EnviormentMode.Test:
                    _testPanel.SetActive(true);
                    break;
                default:
                    _testPanel.SetActive(false);
                    break;
            }

            InitialBag();

            yield return new WaitForSeconds(1.0f);

            HTDBManager.Instance.InitialOver = true;

        }

        public override void OnPause()
        {
            base.OnPause();
            if(canvasGroup==null)
            {
                return;
            }

            canvasGroup.blocksRaycasts = false;
        }

        public override void OnResume()
        {
            base.OnResume();
            canvasGroup.blocksRaycasts = true;
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void OnEnable()
        {
            HTDBManager.Instance.AddEquipEvent += AddEquipEvent;
            HTDBManager.Instance.RemoveEquipEvent += RemoveEquipEvent;
            HTDBManager.Instance.UpdateCoinEvent += UpdatePlayerCoin;
            currencyTxt.text = HTDBManager.Instance.GetCoins().ToString();
            UpdatePlayerCoin();
        }

        private void OnDisable()
        {
            HTDBManager.Instance.AddEquipEvent -= AddEquipEvent;
            HTDBManager.Instance.RemoveEquipEvent -= RemoveEquipEvent;
            HTDBManager.Instance.UpdateCoinEvent -= UpdatePlayerCoin;
        }
        public void AddEquipEvent(int id)
        {
           Item item= InventoryManager.Instance.GetItemById(id);

            GameObject slotGo= GameObject.Instantiate(menueSlot.gameObject, parent);

            MenueWeaponSlot slot=slotGo.GetComponent<MenueWeaponSlot>();
            slot.item = item;
            slot.UpdateUI();

            Debugs.LogInformation("Menue weapon slot UI Equip item:" + id, Color.red);
        }

        public void RemoveEquipEvent(int id)
        {
            Item item = InventoryManager.Instance.GetItemById(id);

            for(int i=0;i<parent.childCount;i++)
            {
                MenueWeaponSlot slot = parent.GetChild(i).GetComponent<MenueWeaponSlot>();

                if (slot.item.itemID==id)
                {
                    Destroy(slot.gameObject);
                    break;
                }
            }
        }

        void InitialBag()
        {
            foreach (var temp in HTDBManager.Instance.equip)
            {
                AddEquipEvent(temp);
            }

        }

        private void OnDestroy()
        {
            HTDBManager.Instance.InitialOver = false;
        }

        public void UpdatePlayerCoin()
        {
            //currencyTxt.text = HTDBManager.Instance.GetCoins().ToString();
            float _v=float.Parse(currencyTxt.text);
            DOTween.To(()=>_v,x=>_v=x,HTDBManager.Instance.GetCoins(),0.5f).OnUpdate(()=>{
                currencyTxt.text=Mathf.Floor(_v).ToString();
            }).SetUpdate(true);
        }

        IEnumerator MoneyChangeText(int currentVal, int newVal)
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
                    if(result<=newVal)
                    {
                        break;
                    }
                    currencyTxt.text = result.ToString();
                    yield return new WaitForSeconds(0.1f);
                }
            }else if (currentVal < newVal)
            {
                for (int i = minVal; i < maxVal; i++)
                {
                    result = result + changeNum;
                    if (result >= newVal)
                    {
                        break;
                    }
                    currencyTxt.text = result.ToString();
                    yield return new WaitForSeconds(0.1f);
                }
            }
            currencyTxt.text = newVal.ToString();
            StopCoroutine(MoneyChangeText(currentVal, newVal));
        }
    }

}
