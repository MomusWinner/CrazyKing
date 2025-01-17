using System.Collections.Generic;
using System.Linq;

namespace Servant.Knight.Upgrades
{
    public class KnightParametersUpgrader : IServantParameterSetter
    {
        private KnightServantSO _knightServantSO;
        private readonly KnightParamData _startData;
        private readonly KnightController _knight;
        private readonly List<KnightParamData> _knightParams;
        
        public KnightParametersUpgrader(KnightServantSO knightServantSO, KnightController knight)
        {
            _knightServantSO = knightServantSO;
            _startData = knightServantSO.startParam;
            _knight = knight;
            _knightParams = knightServantSO.upgrades.Select(u => u.paramData).ToList();
            
        }

        public void SetStartParameter()
        {
            SetParameters(_startData);
        }

        public void UpgradeParameters(int lv)
        {
            for (int i = 0; i < lv; i++)
                SetParameters(_knightParams[i]);
        }

        private void SetParameters(KnightParamData paramData)
        {
            _knight.SetSpeed(paramData.speed + _knight.Speed);
            _knight.ChangeMaxHealth(paramData.health + _knight.MaxHealth);
            _knight.AttackDamage += paramData.damage;
        }
    }

    public interface IServantParameterSetter
    {
        void SetStartParameter();
        void UpgradeParameters(int lv);
    }
}