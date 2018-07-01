using UnityEngine;

namespace Worqnets.Examples.ColorGenetics2D
{
    [DisallowMultipleComponent]
    public class DNA : MonoBehaviour
    {
        public float Red;
        public float Green;
        public float Blue;

        public float SurvivalTime;

        private SpriteRenderer _renderer;
        private Collider2D _collider;
        private Vector3 _size;

        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
            _size = transform.localScale;
            
            _renderer.color = new Color(Red, Green, Blue);
        }
        
        private void OnMouseDown()
        {
            SurvivalTime = PopulationManager.ElapsedTime;
            _renderer.enabled = false;
            _collider.enabled = false;
        }
    }
}