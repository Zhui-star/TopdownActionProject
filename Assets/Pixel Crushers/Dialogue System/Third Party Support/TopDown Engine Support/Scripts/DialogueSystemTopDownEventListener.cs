using UnityEngine;
using MoreMountains.Tools;

namespace PixelCrushers.DialogueSystem.TopDownEngineSupport
{

    /// <summary>
    /// Adds option to integrate Dialogue System saving with Topdown Engine SaveLoadManager.
    /// </summary>
    [AddComponentMenu("Pixel Crushers/Dialogue System/Third Party/TopDown Engine/Dialogue System TopDown Event Listener")]
    public class DialogueSystemTopDownEventListener : MonoBehaviour, MMEventListener<MMGameEvent>
    {

        [Tooltip("Save & load Dialogue System data when More Mountains SaveLoadManager requests. If using DialogueSystemInventoryEventListener, UNtick this on one or the other.")]
        public bool handleMMSaveLoadEvents = false;

        /// <summary>
        /// On enable, we start listening for MMGameEvents.
        /// </summary>
        protected virtual void OnEnable()
        {
            if (handleMMSaveLoadEvents) this.MMEventStartListening<MMGameEvent>();
        }

        /// <summary>
        /// On disable, we stop listening for MMGameEvents.
        /// </summary>
        protected virtual void OnDisable()
        {
            if (handleMMSaveLoadEvents) this.MMEventStopListening<MMGameEvent>();
        }


        public virtual void OnMMEvent(MMGameEvent gameEvent)
        {
            if (gameEvent.EventName == "Save")
            {
                SaveDialogueSystem();
            }
            if (gameEvent.EventName == "Load")
            {
                LoadDialogueSystem();
            }
        }

        protected const string _saveFolderName = "DialogueSystem/";
        protected const string _saveFileExtension = ".data";

        public void SaveDialogueSystem()
        {
            var data = PersistentDataManager.GetSaveData();
            MMSaveLoadManager.Save(data, gameObject.name + _saveFileExtension, _saveFolderName);

        }

        public void LoadDialogueSystem()
        {
            string data = (string)MMSaveLoadManager.Load(typeof(string), gameObject.name + _saveFileExtension, _saveFolderName);
            PersistentDataManager.ApplySaveData(data);
        }

    }
}
