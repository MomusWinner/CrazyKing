﻿using System;
using System.Collections.Generic;
using Servant.Upgrade;
using UnityEngine;

namespace Entity.Servant
{
    public interface IServantParameterContainer
    {
        string[] GetAvailableParameters();
        object GetParameterValue(string name, int lv);
    }
    
    public abstract class ServantSO: ScriptableObject
    {
        public string servantName;
        public string description;
        public Sprite avatar;
        public AvatarByLevel[] avatarByLevel;
        public ServantType type;
        public string prefabPath;
        public int price;
        
        public abstract List<ServantUpgradeData> Upgrades { get; }

        public Sprite GetAvatarByLevel(int lv)
        {
            foreach (var item in avatarByLevel)
                if (lv <= item.lv)
                    return item.avatar;
            return null;
        }
    }

    [Serializable]
    public class AvatarByLevel
    {
        public int lv;
        public Sprite avatar;
    }

    public enum ServantType
    {
        Knight,
        Archer,
    }
}