using System;
using System.Collections.Generic;
using System.Linq;
using Entity.Servant.Knight.Upgrades;

namespace Entity.Servant.Archer.Upgrades
{
    public class ArcherParametersUpgrader : IServantParameterSetter
    {
        private readonly ArcherController _archer;
        private readonly List<ArcherParamData> _archerParams;
        private readonly ArcherServantSO _archerSO;
        
        public ArcherParametersUpgrader(ArcherServantSO servantSO, ArcherController archerController)
        {
            _archerSO = servantSO;
            _archer = archerController;
            _archerParams = servantSO.upgrades.Select(u => u.archerParamData).ToList();
        }
        
        public void UpgradeParameters(int lv)
        {
            for(int i = 0; i < lv; i++)
                SetParameters(_archerParams[i]);
            
            // Setup skin
            ArcherSkinType type = _archerParams[lv - 1].SkinType;
            ArcherSkin skin = Array.Find(_archerSO.Skins, s => s.SkinType == type);
            _archer.SetBodySprite(skin.Body);
            _archer.SetFootSprite(skin.Foot);
            _archer.SetBowSprite(skin.Bow);
        }

        private void SetParameters(ArcherParamData paramData)
        {
            _archer.ArrowPath = paramData.ArrowPath;
            _archer.SetSpeed(paramData.Speed + _archer.Speed);
            _archer.AttackDamage += paramData.Damage;
            _archer.ChangeMaxHealth(paramData.Health + _archer.MaxHealth);
            _archer.LookRadius += paramData.LookRadius;
        }
    }
}