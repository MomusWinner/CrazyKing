using System;
using System.Collections.Generic;
using King;
using King.Upgrades.Parameters;
using Servant;

namespace Controllers
{
    [Serializable]
    public class GameData
    {
        public int Level = 1;
        public int Coins;
        public float MusicVolume = 0.7f;
        public bool IsFirstStarting;
        public List<ServantData> Servants = new ();
        public Dictionary<KingParameterType, KingParameterData> KingParameters = new ();
    }
}