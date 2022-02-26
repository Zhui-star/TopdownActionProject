using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using DG.Tweening;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
namespace HTLibrary.Application
{
    /// <summary>
    /// 角色选择面板
    /// </summary>
    public class CharacterSelectPanel : BasePanel
    {
        public GameObject UnitGo;
        public float animSecond = 0.5f;
        public Transform grid;



        private void Start()
        {
            TriggerCharacterSelection();
        }

        private void TriggerCharacterSelection()
        {

            for (int i = 0; i < grid.childCount; i++)
            {
                Destroy(grid.GetChild(i).gameObject);
            }

            foreach (var temp in CharacterSelection.Instance.GetRandomCharIds())//CharacterSelection.Instance.GetBindIds(HTLevelManager.Instance.CurrentCharacterID))
            {
                GameObject go = GameObject.Instantiate(UnitGo, grid);
                CharacterUnitUI unit = go.GetComponent<CharacterUnitUI>();
                unit.UpdateUI(CharacterSelection.Instance.GetCharacterUnit(temp));
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();


            transform.DOScale(Vector3.one, animSecond).OnComplete(() =>
            {
                HTPauseGame.Instance.PauseGame();
            }).SetUpdate(true);

            TriggerCharacterSelection();

        }

        public override void OnExit()
        {
            base.OnExit();           
            transform.DOScale(Vector3.zero, animSecond).OnComplete(()=>{}).SetUpdate(true);
        }

        public void OnClosePanel()
        {
            UIManager.Instance.PopPanel();
            Debug.Log("Pop panel start");
        }

        public void OnCloseBtnClick()
        {
            transform.DOScale(Vector3.zero, 0.2f);
            Invoke("OnClosePanel", 0.2f);
        }

    }

}
