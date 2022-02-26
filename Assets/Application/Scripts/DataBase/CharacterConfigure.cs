using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace HTLibrary.Application
{
    public class CharacterConfigure : ScriptableObject
    {
       [ReorderableList]
        public List<CharacterUnit> characters = new List<CharacterUnit>();
        //public GameObject[] characters;

    }
}