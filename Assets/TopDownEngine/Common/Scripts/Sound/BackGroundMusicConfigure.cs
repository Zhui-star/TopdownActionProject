using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace HTLibrary.Application
{
    public class BackGroundMusicConfigure : ScriptableObject
    {
        [ReorderableList]
        public List<BackGroundMusicUnit> backGroundMusic = new List<BackGroundMusicUnit>();
    }
}

