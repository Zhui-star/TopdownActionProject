using UnityEngine;
using HTLibrary.Framework;
using HTLibrary.Utility;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using UnityEngine.Rendering;
namespace HTLibrary.Application
{
    /// <summary>
    /// 转职角色的单位UI
    /// </summary>
    public class CharacterUnitUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {

        public Text NameTxt;

        private CharacterUnit characterUnit;

        public GameObject mask;

        public RawImage rawImage;

        public CharacterShow characterShow;

        [ReadOnly]private RenderTexture renderTexture;

        public Camera avatarCamera;

        public Button button;

        public GameObject changeName;
        public GameObject tips;
        public Text tipContent;
        private bool isOnTip = false; //判断鼠标是否在tip上

        /// <summary>
        /// 更新UI显示
        /// </summary>
        /// <param name="characterUnit"></param>
        public void UpdateUI(CharacterUnit characterUnit)
        {
            
            this.characterUnit = characterUnit;

            tipContent.text = this.characterUnit._characterInfo;

            characterShow.gameObjects[characterUnit.id - 1].SetActive(true);

            NameTxt.text = this.characterUnit.characterName;

            renderTexture = RenderTexture.GetTemporary(256, 256, 1);
            renderTexture.name = "Jiushiwo!";
            avatarCamera.targetTexture = renderTexture;
            rawImage.texture = avatarCamera.targetTexture;

            changeName.SetActive(true);
            button.interactable = true;
            mask.SetActive(false);
            rawImage.color = Color.white;
            characterShow.gameObjects[characterUnit.id - 1].GetComponent<Animator>().speed = 1.0f;

        }
        private void Update()
        {
            if (isOnTip)
            {
                tips.transform.position = Input.mousePosition;
            }
        }

        /// <summary>
        /// 选择角色按钮点击事件
        /// </summary>
        public void SelectCharacterClick()
        {
            CharacterSelection.Instance.ChangeCharacter(characterUnit.id);

            rawImage.texture = null;
            avatarCamera.targetTexture = null;
            avatarCamera.enabled = false;
            RenderTexture.ReleaseTemporary(renderTexture);

            UIManager.Instance.PopPanel();
            UIManager.Instance.PushPanel(UIPanelType.HTSkillSelectPanel);

        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tips.SetActive(true);
            isOnTip = true;
            
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            tips.SetActive(false);
            isOnTip = false;
        }
    }

}
