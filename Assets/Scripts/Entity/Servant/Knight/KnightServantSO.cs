using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Servant.Knight.Upgrades;
using Servant;
using Servant.Upgrade;
using UnityEngine;

namespace Entity.Servant.Knight
{
    [Serializable]
    public class KnightSkin
    {
        public KnightSkinType SkinType;
        public Sprite Body;
        public Sprite Sword;
        public Sprite Foot;
    }

    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(KnightServantSO))]
    public class KnightServantSO : ServantSO, IServantParameterContainer
    {
        public List<KnightUpgradeData> upgrades; 
        public override List<ServantUpgradeData> Upgrades => upgrades.Cast<ServantUpgradeData>().ToList();

        public KnightSkin[] Skins;
        public string[] GetAvailableParameters()
        {
            return new string[] { "Health", "Damage", "AttackSpeed"};
        }

        public object GetParameterValue(string name, int lv)
        {
            switch (name)
            {
                case "Health":
                    return upgrades.GetRange(0, lv).Sum(u => u.paramData.Health);
                case "Damage":
                    return upgrades.GetRange(0, lv).Sum(u => u.paramData.Damage);
                case "AttackSpeed":
                    return upgrades.GetRange(0, lv).Sum(u => u.paramData.AttackSpeed);
            }
            return null;
        }
    }

    public enum KnightSkinType
    {
        Evolution1,
        Evolution2,
        Evolution3,
    }
}