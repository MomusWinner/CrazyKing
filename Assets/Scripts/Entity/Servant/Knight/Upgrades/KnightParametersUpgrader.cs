using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Entity.Servant.Knight.Upgrades
{
    public class KnightParametersUpgrader : IServantParameterSetter
    {
        private static readonly int AttackSpeed = Animator.StringToHash("AttackSpeed");
        private readonly KnightController _knight;
        private readonly List<KnightParamData> _knightParams;
        private readonly KnightServantSO _knightSO;
        
        public KnightParametersUpgrader(KnightServantSO knightServantSO, KnightController knight)
        {
            _knightSO = knightServantSO;
            _knight = knight;
            _knightParams = knightServantSO.upgrades.Select(u => u.paramData).ToList();
        }

        public void UpgradeParameters(int lv)
        {
            for (int i = 0; i < lv; i++)
                SetParameters(_knightParams[i]);
            
            _knight.Animator.SetFloat(AttackSpeed, _knight.AttackSpeed / 10f);

            // Setup skin
            KnightSkinType type = _knightParams[lv - 1].KnightSkin;
            KnightSkin skin = Array.Find(_knightSO.Skins, s => s.SkinType == type);
            _knight.SetBodySprite(skin.Body);
            _knight.SetFootSprite(skin.Foot);
            _knight.SetSwordSprite(skin.Sword);
        }

        private void SetParameters(KnightParamData paramData)
        {
            _knight.SetSpeed(paramData.Speed + _knight.Speed);
            _knight.ChangeMaxHealth(paramData.Health + _knight.MaxHealth);
            _knight.AttackDamage += paramData.Damage;
            _knight.AttackSpeed += paramData.AttackSpeed;
        }
    }

    public interface IServantParameterSetter
    {
        void UpgradeParameters(int lv);
    }
}