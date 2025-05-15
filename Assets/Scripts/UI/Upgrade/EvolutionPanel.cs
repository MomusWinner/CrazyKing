using System.Collections;
using DG.Tweening;
using Servant;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Upgrade
{
    public class EvolutionPanel : MonoBehaviour
    {
        private const string PathToEvolutionElement = "UI/UpgradePanel/ServantEvolutionElement";
        
        [SerializeField] private float _width;
        [SerializeField] private GameObject _panel;
        private GameObject[] _evolutionElements;
        private GameObject _evolutionElement;
        
        public void StartAnim(ServantSO servantSO, int[] levels)
        {
            _panel.SetActive(true);
            _evolutionElements = new GameObject[levels.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                _evolutionElements[i] = CreateEvolutionElement(servantSO.GetAvatarByLevel(levels[i]));
            }
            _evolutionElement = CreateEvolutionElement(servantSO.GetAvatarByLevel(levels[0] + 1));
            

            StartCoroutine(StartAnim());
        }

        private IEnumerator StartAnim()
        {
            _evolutionElement.SetActive(false);
            for (int i = 0; i < _evolutionElements.Length; i++)
            {
                float x = (_width  * i) / (_evolutionElements.Length - 1) - _width / 2;
                Vector3 position = new Vector3(x, 0, 0);

                _evolutionElements[i].transform.DOLocalMove(position, 0.7f).SetEase(Ease.OutCubic);
            }
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < _evolutionElements.Length; i++)
            {
                _evolutionElements[i].transform.DOLocalMove(Vector3.zero, 0.5f);
                _evolutionElements[i].transform.DOScale(Vector3.one * 0.7f, 0.5f);
            }
            yield return new WaitForSeconds(0.3f);
            
            _evolutionElement.transform.localScale = Vector3.one * 0.001f;
            _evolutionElement.SetActive(true);
            _evolutionElement.transform.DOScale(Vector3.one * 1.4f, 1f).SetEase(Ease.OutElastic);
            yield return new WaitForSeconds(2f);
            Destroy(_evolutionElement);
            foreach (var e in _evolutionElements)
                Destroy(e);
            _panel.SetActive(false);
        }

        private GameObject CreateEvolutionElement(Sprite avatar)
        {
            GameObject elementObj = Resources.Load<GameObject>(PathToEvolutionElement);
            GameObject element = Instantiate(elementObj, transform);
            Image image = element.GetComponentInChildren<Image>();
            image.sprite = avatar;
            return element;
        }
    }
}