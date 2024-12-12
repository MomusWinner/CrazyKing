using UnityEngine;

namespace Controllers
{
    public class FSPController : MonoBehaviour
    {
        [SerializeField] public int _frameRate = 30;
        public void Start()
        {
            Application.targetFrameRate = _frameRate;
        }
    }
}