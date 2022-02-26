using System.Collections;

using UnityEngine;

namespace HTLibrary.Application
{
    public class Status : MonoBehaviour
    {
        private CharacterStatus statuts = CharacterStatus.None;
        public CharacterStatus _Status
        {
            get
            {
                return statuts;
            }
            set
            {
                SwitchStatus(value);
                statuts = value;
            }
        }

        public GameObject freezeFeedback;
        public GameObject slowDownFeedBack;
        public GameObject _healFeedBack;
        public GameObject _radiculeFeedBack;
        public GameObject _frozenFeedBack;

        private void SwitchStatus(CharacterStatus status)
        {
            switch (status)
            {
                case CharacterStatus.None:
                    freezeFeedback.SetActive(false);
                    if (slowDownFeedBack != null)
                    {
                        slowDownFeedBack.SetActive(false);
                    }

                    if (_healFeedBack != null)
                    {
                        _healFeedBack.SetActive(false);
                    }

                    if (_radiculeFeedBack != null)
                    {
                        _radiculeFeedBack.SetActive(false);
                    }
                    if(_frozenFeedBack!=null)
                    {
                        _frozenFeedBack.SetActive(false);
                    }
                    break;
                case CharacterStatus.Freeze:
                    freezeFeedback.SetActive(true);
                    break;
                case CharacterStatus.KnockUp:
                    break;
                case CharacterStatus.Silence:
                    break;
                case CharacterStatus.SlowDown:
                    if (slowDownFeedBack != null)
                    {
                        slowDownFeedBack.SetActive(true);
                    }
                    break;
                case CharacterStatus.Heal:
                    if (_healFeedBack != null)
                    {
                        _healFeedBack.SetActive(true);
                    }
                    break;
                case CharacterStatus.Radicule:
                    if (_radiculeFeedBack != null)
                    {
                        _radiculeFeedBack.SetActive(true);
                    }
                    break;
                case CharacterStatus.Frozen:
                    if (_frozenFeedBack != null)
                    {
                        _frozenFeedBack.SetActive(true);
                    }
                    break;
            }
        }
    }

}
