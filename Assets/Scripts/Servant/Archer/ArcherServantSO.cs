using System;
using System.Collections.Generic;
using System.Linq;
using Servant.Archer.Upgrades;
using Servant.Upgrade;
using UnityEngine;

namespace Servant.Archer
{
    [Serializable]
    public class ArcherSkin
    {
        public ArcherSkinType SkinType;
        public Sprite Body;
        public Sprite Bow;
        public Sprite Foot;
    }
    
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(ArcherServantSO))]
    public class ArcherServantSO : ServantSO, IServantParameterContainer
    {
        public List<ArcherUpgradeData> upgrades;
        public override List<ServantUpgradeData> Upgrades => upgrades.Cast<ServantUpgradeData>().ToList();
        public ArcherSkin[] Skins;
        
        public string[] GetAvailableParameters()
        {
            return new string[] { "Health", "Damage" };
        }

        public object GetParameterValue(string name, int lv)
        {
            switch (name)
            {
                case "Health":
                    return upgrades.GetRange(0, lv).Sum(u => u.archerParamData.Damage);
                case "Damage":
                    return upgrades.GetRange(0, lv).Sum(u => u.archerParamData.Damage);
            }
            return null;
        }
    }

    public enum ArcherSkinType
    {
        Evolution1,
        Evolution2,
        Evolution3,
    }
}