using DG.Tweening;
using UnityEngine;

namespace UI.Upgrade
{
    public static class UIHelper
    { 
        public static void ScaleButton(GameObject obj)
        {
            //DOTween.Kill(obj.transform);
            Sequence sequence = DOTween.Sequence();
            Vector3 startScale = obj.transform.localScale;
            sequence.Append(obj.transform.DOScale(startScale * 1.1f, 0.3f).SetEase(Ease.OutCubic));
            sequence.Append(obj.transform.DOScale(startScale, 0.2f));
        }

        public static void ShakeButton(GameObject obj)
        {
            // DOTween.Kill(obj.transform);
            obj.transform.DOShakePosition(0.4f, strength: 2f);
        }
    }
}