using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTLibrary.Framework;
using System;
using MoreMountains.TopDownEngine;
using MoreMountains.Feedbacks;
namespace HTLibrary.Utility
{
    [Serializable]
    public class Goutility
    {
        public GameObject go;
        public float timer;
        public AudioClip audioClip;
        public MMFeedbacks activeFeedBacks;
    }

    public class DelayActiveGo :MonoBehaviour
    {
        public List<Goutility> gos = new List<Goutility>();
        
        private void OnEnable()
        {
            StartCoroutine(ShowChilds());
        }

        private void OnDisable()
        {
            HideChilds();
        }


        public IEnumerator ShowChilds()
        {
            foreach(var go in gos)
            {
                yield return new WaitForSeconds(go.timer);
                go.go.SetActive(true);
                go.activeFeedBacks?.Initialization();
                go.activeFeedBacks?.PlayFeedbacks();

                if (go.audioClip != null)
                {
                    SoundManager.Instance.PlaySound(go.audioClip, transform.position, false);
                }
            }
        }

        public void HideChilds()
        {
            foreach (var go in gos)
            {
                go.go.SetActive(false);

               
            }
        }
    }

}
