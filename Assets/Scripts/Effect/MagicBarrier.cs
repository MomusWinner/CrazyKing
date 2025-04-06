using System.Collections;
using UnityEngine;

public class MagicBarrier : MonoBehaviour
{
    [SerializeField] private float _hideSpeed;
    
    private static readonly int Step = Shader.PropertyToID("_Step");
    private static readonly int Color = Shader.PropertyToID("_Color");
    private static readonly int ColorIn = Shader.PropertyToID("_ColorIn");
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private float _startStep;

    public void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        
        _startStep = _spriteRenderer.material.GetFloat(Step);
    }

    public void Off()
    {
        _collider.enabled = false;
        StartCoroutine(HideMagicBarrier());
    }

    private IEnumerator HideMagicBarrier()
    {
        while (_startStep > 0)
        {
            _startStep -= _hideSpeed * Time.deltaTime * 4f;
            _spriteRenderer.material.SetFloat(Step, _startStep);
            yield return null;
        }
        while (_startStep < 2)
        {
            _startStep += _hideSpeed * Time.deltaTime;
            _spriteRenderer.material.SetFloat(Step, _startStep);
            yield return null;
        }

        float colorAlpha = _spriteRenderer.material.GetColor(Color).a;
        float colorInAlpha = _spriteRenderer.material.GetColor(Color).a;
        
        while (colorInAlpha > 0 && colorAlpha > 0)
        {
            Color color = _spriteRenderer.material.GetColor(Color);
            Color colorIn = _spriteRenderer.material.GetColor(ColorIn);
            color.a -= _hideSpeed * Time.deltaTime * 0.01f;
            colorIn.a -= _hideSpeed * Time.deltaTime * 0.01f;
            colorAlpha -= color.a;
            colorInAlpha -= colorIn.a;
            _spriteRenderer.material.SetColor(Color, color);
            _spriteRenderer.material.SetColor(ColorIn, colorIn);
            yield return null;
        }
        Destroy(gameObject);
    }
}