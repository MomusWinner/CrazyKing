using System;
using System.Collections.Generic;
using System.Linq;
using Servant.Knight.Upgrades;
using Servant.Upgrade;
using TriInspector;
using UnityEngine;

namespace Servant.Knight
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
    public class KnightServantSO : ServantSO
    {
        public List<KnightUpgradeData> upgrades; 
        public override List<ServantUpgradeData> Upgrades => upgrades.Cast<ServantUpgradeData>().ToList();

        public KnightSkin[] Skins;
    }

    public enum KnightSkinType
    {
        Evolution1,
        Evolution2,
        Evolution3,
    }
}