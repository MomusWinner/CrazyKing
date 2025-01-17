using TriInspector;
using UnityEngine;

namespace Servant.FSM
{
    [CreateAssetMenu(menuName = "Game/Servant/" + nameof(ServantStatesSO))]
    public class ServantStatesSO : ScriptableObject
    {
        [Title("Following King State")]
        public AnimationCurve speedByApproachingToTarget;
    }
}