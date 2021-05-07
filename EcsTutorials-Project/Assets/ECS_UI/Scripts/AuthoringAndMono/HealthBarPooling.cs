using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TMG.ECS_UI
{
    public class HealthBarPooling : MonoBehaviour
    {
        public static HealthBarPooling Instance;
        
        [SerializeField] private GameObject _healthBarPrefab;
        [SerializeField] private int _initialPoolSize;
        [SerializeField] private Transform _healthBarContainer;
        
        private Stack<Slider> _healthBars;
        private bool NoMoreHealthBars => _healthBars.Count <= 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _healthBars = new Stack<Slider>(_initialPoolSize);
            for (var i = 0; i < _initialPoolSize; i++)
            {
                SpawnSlider();
            }
        }

        private void SpawnSlider()
        {
            var newHealthBarGO =
                Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.Euler(0, 45, 0), _healthBarContainer);
            var newHealthBarSlider = newHealthBarGO.GetComponent<Slider>();
            _healthBars.Push(newHealthBarSlider);
            newHealthBarSlider.gameObject.SetActive(false);
        }
        
        public Slider GetNextSlider()
        {
            if (NoMoreHealthBars)
            {
                SpawnSlider();
            }
            var nextSlider = _healthBars.Pop();
            nextSlider.gameObject.SetActive(true);
            return nextSlider;
        }

        public void ReturnSlider(Slider sliderToReturn)
        {
            sliderToReturn.gameObject.SetActive(false);
            _healthBars.Push(sliderToReturn);
        }
    }
}