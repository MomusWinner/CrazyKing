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
        public int Coins;
        public bool IsFirstStarting;
        public List<ServantData> Servants = new ();
        public Dictionary<KingParameterType, KingParameterData> KingParameters = new ();
    }
}