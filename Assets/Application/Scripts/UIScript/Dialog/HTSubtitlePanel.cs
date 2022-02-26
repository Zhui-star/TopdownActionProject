using UnityEngine;
using PixelCrushers.DialogueSystem;
using PixelCrushers.Wrappers;
using HTLibrary.Framework;
using HTLibrary.Utility;
namespace HTLibrary.Application
{
    /// <summary>
    /// Subtitle UI for dialog UI
    /// </summary>
    public class HTSubtitlePanel : PixelCrushers.DialogueSystem.StandardUISubtitlePanel
    {
        private string _playerName;

        public override void OpenOnStartConversation(Sprite portraitSprite, string portraitName, DialogueActor dialogueActor)
        {
            portraitName = SetPlayerTitleName(portraitName);
            base.OpenOnStartConversation(portraitSprite, portraitName, dialogueActor);
        }

        public override void SetContent(Subtitle subtitle)
        {
            subtitle.speakerInfo.Name = SetPlayerTitleName(subtitle.speakerInfo.Name);
            base.SetContent(subtitle);
        }

        string SetPlayerTitleName(string portraitName)
        {
            if (_playerName == null)
            {
                _playerName = PlayerPrefs.GetString(Consts.Name + SaveManager.Instance.LoadGameID);
            }

            if (portraitName.StartsWith("[Player]"))
            {
                portraitName = _playerName;
            }

            return portraitName;
        }
    }
}

