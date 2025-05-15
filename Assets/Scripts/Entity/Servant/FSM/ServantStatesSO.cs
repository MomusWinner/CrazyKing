using TriInspector;
using UnityEngine;

namespace Entity.Servant.FSM
{
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(ServantStatesSO))]
    public class ServantStatesSO : ScriptableObject
    {
        [Title("Following King State")]
        public AnimationCurve speedByApproachingToTarget;
    }
}