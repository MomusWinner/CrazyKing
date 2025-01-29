using System;
using System.Collections.Generic;
using Castle;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace Controllers
{
    public class CastleManager : IStartable, ITickable
    {
        public Action OnCastlesCaptured;
        public bool IsCastlesCaptured { get; private set; }
        public int CastleCount => _castles.Count;
        public int CompletedCastlesCount { get; private set; }
        
        public IReadOnlyCollection<CastleController> CastleControllers => _castles;
        private List<CastleController> _castles = new();
        
        public void Start()
        {
            CastleController[] castles = Object.FindObjectsByType<CastleController>(FindObjectsSortMode.None);
            foreach (var castle in castles)
                RegisterCastle(castle);
        }

        public void RegisterCastle(CastleController castle)
        {
            _castles.Add(castle);
        }


        public void Tick()
        {
            int completed = 0;
            
            foreach (var castle in _castles)
            {
                if(castle.IsCaptured) completed++;
            }
            
            CompletedCastlesCount = completed;

            if (completed == _castles.Count && !IsCastlesCaptured)
            {
                OnCastlesCaptured?.Invoke();
                IsCastlesCaptured = true;
            }
        }
    }
}