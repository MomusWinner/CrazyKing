using System.Collections;
using Effect;
using UnityEngine;

namespace Health
{
    public class DamageColorizer : SpriteColorizer 
    {
        [SerializeField] private Color _flashColor;
        [SerializeField] private float _flashTime = 0.25f;
        [SerializeField] private ParticleSystem _damageParticle;

        private Coroutine _flashCoroutine;

        protected override void Awake()
        {
            base.Awake();
            SetFlashColor(_flashColor);
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

            while (elapsedTime < _flashTime)
            {
                elapsedTime += Time.deltaTime;
                SetFlashAmount(Mathf.Lerp(1f, 0f, elapsedTime / _flashTime));
                yield return null;
            }
        }
    }
}