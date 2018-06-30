using UnityEngine;

namespace Worqnets.Examples.ColorGenetics2D
{
    public class ColorDNA2D : MonoBehaviour
    {
        public float Red;
        public float Green;
        public float Blue;

        private bool _isDead;
        public float SurvivalTime;

        private SpriteRenderer _renderer;
        private Collider2D _collider;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            
            _renderer.color = new Color(Red, Green, Blue, 1);
        }

        private void Update()
        {
            if (_isDead)
            {
                
            }
        }

        private void OnMouseDown()
        {
            _isDead = true;
            SurvivalTime = PopulationManager.ElapsedTime;
            _renderer.enabled = false;
            _collider.enabled = false;
        }
    }
}