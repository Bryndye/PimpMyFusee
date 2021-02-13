using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ModulesData01", menuName = "Scriptable objects/Modules data")]
public class ModulesData : ScriptableObject
{
    [System.Serializable]
    public struct Module
    {
        public string name;
        public GameObject prefab;
        public Sprite icon;
    }

    [SerializeField] public List<Module> modulesList = new List<Module>();
}
