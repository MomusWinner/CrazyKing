using System.Collections;
using UnityEngine;

namespace Health
{
    public class DamageFlash : MonoBehaviour
    {
        private static readonly int FlashColor = Shader.PropertyToID("_FlashColor");
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        [SerializeField] private Color _flashColor;
        [SerializeField] private float _flashTime = 0.25f;
        [SerializeField] private ParticleSystem _damageParticle;

        private SpriteRenderer[] _spriteRenderers;
        private Material[] _materials;
        private Coroutine _flashCoroutine;

        private void Awake()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            _materials = new Material[_spriteRenderers.Length];
            for (int i = 0; i < _spriteRenderers.Length; i ++)
                _materials[i] = _spriteRenderers[i].material;
            SetFlashColor();
        }

        public void CallDamageFlash()
        {
            _flashCoroutine = StartCoroutine(DamageFlasher());
            if (_damageParticle != null)
                Instantiate(_damageParticle, transform.position, Quaternion.identity);
        }

        private IEnumerator DamageFlasher()
        {
            float elapsedTime = 0;
            float flashAmount = 0;

            while (elapsedTime < _flashTime)
            {
                elapsedTime += Time.deltaTime;
                SetFlashAmount(Mathf.Lerp(1f, 0f, elapsedTime / _flashTime));
                yield return null;
            }
        }

        private void SetFlashColor()
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetColor(FlashColor, _flashColor);
            }
        }

        private void SetFlashAmount(float flashAmount)
        {
            for (int i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(FlashAmount, flashAmount);
            }
        }
    }
}