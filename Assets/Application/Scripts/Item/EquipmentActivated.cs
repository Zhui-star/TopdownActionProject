using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using HTLibrary.Utility;
using HTLibrary.Framework;

namespace HTLibrary.Application
{
    /// <summary>
    /// 装备交互Item
    /// </summary>
    public class EquipmentActivated : ButtonActivated
    {
        [Header("Equipment Activation")]
        public int _ID;
        private HTDBManager _htDBManager;
        public SpriteRenderer _bgRender;
        public SpriteRenderer _iconRender;
        public SpriteRenderer _typeRender;
        public SpriteRenderer _typeIconRender;
        private InventoryManager _inventoryManager;
        public Animator animator;
        Item item = null;

        private void Awake()
        {
            _htDBManager = HTDBManager.Instance;
            _inventoryManager = InventoryManager.Instance;

            SetID(_inventoryManager.GetRandomActivatedItemId());
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            InitialSprite();
            InitialButtonPromtTxt();
        }



        /// <summary>
        /// 初始化Sprite
        /// </summary>
        void InitialSprite()
        {
            item = _inventoryManager.GetItemById(_ID);
            _bgRender.sprite = item.slotSprite;
            _iconRender.sprite = item.itemSprite;
            _typeRender.sprite = item.itemTypeBgSprite;
            _typeIconRender.sprite= item.itemTypeSprite;
        }

        /// <summary>
        /// 初始化ButtonPromt提示
        /// </summary>
        void InitialButtonPromtTxt()
        {
            ButtonPromptText = item.itemName+" (F)";
        }

        public override void TriggerButtonAction()
        {
            base.TriggerButtonAction();
            GameObject castEff = PoolManagerV2.Instance.GetInst("TalentChoose");
            castEff.transform.position = this.transform.position;
            _htDBManager.PickUpEquipment(_ID);

            animator.Play("EquipGotten");
            Invoke("HideGameObject", 0.5f);
        }

        void HideGameObject()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置ID
        /// </summary>
        /// <param name="id"></param>
        public void SetID(int id)
        {
            this._ID = id;
        }
    }

}
