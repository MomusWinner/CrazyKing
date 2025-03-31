using System;
using UnityEngine;

namespace Controllers
{
    /*[Serializable]
    public class LevelSO
    {
        public string SceneName;
        
        

    }*/
    
    [CreateAssetMenu(menuName = "Game/LevelSO")]
    public class LevelsSO : ScriptableObject
    {
        public string[] levels;
    }
}