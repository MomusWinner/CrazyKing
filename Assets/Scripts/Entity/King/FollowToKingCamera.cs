using UnityEngine;
using VContainer;

namespace Entity.King
{
    public class FollowToKingCamera : MonoBehaviour
    {
        [Inject] private KingController _king;

        private void LateUpdate()
        {
            if (!_king) return;
            Vector3 pos = Vector3.Lerp(transform.position, _king.transform.position, Time.deltaTime * 4);
            transform.position = (new Vector3(pos.x, pos.y, transform.position.z));
        }
    }
}