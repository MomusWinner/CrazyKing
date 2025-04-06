using UnityEngine;

namespace Level.Tutorial
{
    public class TutorialCheckCageDestroy : MonoBehaviour
    {
        [SerializeField] private MagicBarrier _magicBarrier;
        [SerializeField] private StartCage _cage;

        private bool _magicBarrierDisabled;
        
        public void Update()
        {
            if (_magicBarrierDisabled || _cage != null) return;
            _magicBarrier.Off();
            _magicBarrierDisabled = true;
        }
    }
}