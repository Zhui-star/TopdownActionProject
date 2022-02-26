using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HTLibrary.Utility
{
    [Serializable]
    public class SlotBg
    {
        public ItemQuality itemQuality;
        public Sprite slotBgSprite;
        public Sprite circleBgSprite;

        public SlotBg()
        {
            itemQuality = ItemQuality.White;
        }
    }
}
