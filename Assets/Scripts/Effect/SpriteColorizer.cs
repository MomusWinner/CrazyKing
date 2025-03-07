using UnityEngine;

namespace Effect
{
    public class SpriteColorizer : MonoBehaviour
    {
        protected static readonly int FlashColor = Shader.PropertyToID("_FlashColor");
        protected static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        private SpriteRenderer[] _spriteRenderers;
        private Material[] _materials;
        private Coroutine _flashCoroutine;

        protected virtual void Awake()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _materials = new Material[_spriteRenderers.Length];
            for (int i = 0; i < _spriteRenderers.Length; i ++)
                _materials[i] = _spriteRenderers[i].material;
        }

        public void SetFlashColor(Color flashColor)
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetColor(FlashColor, flashColor);
            }
        }

        public void SetFlashAmount(float flashAmount)
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(FlashAmount, flashAmount);
            }
        }
    }
}