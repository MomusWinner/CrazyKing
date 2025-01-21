using System.Collections.Generic;
using System.Linq;
using Servant.Knight.Upgrades;

namespace Servant.Archer.Upgrades
{
    public class ArcherParametersUpgrader : IServantParameterSetter
    {
        private readonly ArcherParamData _startData;
        private readonly ArcherController _archer;
        private readonly List<ArcherParamData> _archerParams;
        
        public ArcherParametersUpgrader(ArcherServantSO servantSO, ArcherController archerController)
        {
            _startData = servantSO.startParam;
            _archer = archerController;
            _archerParams = servantSO.upgrades.Select(u => u.archerParamData).ToList();
        }
        
        public void SetStartParameter()
        {
            SetParameters(_startData);
        }

        public void UpgradeParameters(int lv)
        {
            for(int i = 0; i < lv; i++)
                SetParameters(_archerParams[i]);
        }

        private void SetParameters(ArcherParamData paramData)
        {
            _archer.ArrowPrefPath = paramData.arrowPath;
            _archer.Animator.runtimeAnimatorController = paramData.animatorController;
            _archer.SetSpeed(paramData.speed + _archer.Speed);
            _archer.AttackDamage += paramData.damage;
            _archer.ChangeMaxHealth(paramData.health + _archer.MaxHealth);
        }
    }
}