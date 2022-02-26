using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

namespace HTLibrary.Framework
{
    public class GameObjectPoolList : ScriptableObject
    {//继承自ScriptableObject 表示吧类GameObjectPoolList变成可以自定义资源配置的文件
        [ReorderableList]
        public List<GameObjectPool> poolList;
    }
}
