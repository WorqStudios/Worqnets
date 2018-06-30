using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Worqnets.Examples.ColorGenetics2D
{
    public class PopulationManager : MonoBehaviour
    {
        public static float ElapsedTime = 0f;

        public GameObject PersonPrefab;
        public int PopulationSize = 10;
        public float TrialTime = 10f;

        private int _generation = 1;

        private List<GameObject> _population = new List<GameObject>();

        private GUIStyle _guiStyle = new GUIStyle();

        private void Awake()
        {
            for (var i = 0; i < PopulationSize; i++)
            {
                var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 6f), 0);
                var person = Instantiate(PersonPrefab, position, PersonPrefab.transform.rotation);
                person.transform.SetParent(transform);
                var dna = person.AddComponent<ColorDNA2D>();

                dna.Red = Random.Range(0, 1f);
                dna.Green = Random.Range(0, 1f);
                dna.Blue = Random.Range(0, 1f);

                _population.Add(person);
            }
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;

            if (ElapsedTime > TrialTime)
            {
                BreedNewGeneration();
                ElapsedTime = 0;
            }
        }

        private void BreedNewGeneration()
        {
            var newPopulation = new List<GameObject>();
            var sortedList = _population.OrderBy(o => o.GetComponent<ColorDNA2D>().SurvivalTime).ToList();

            _population.Clear();
            for (var i = sortedList.Count / 2 - 1; i < sortedList.Count - 1; i++)
            {
                _population.Add(Breed(sortedList[i], sortedList[i + 1]));
                _population.Add(Breed(sortedList[i + 1], sortedList[i]));
            }

            foreach (var person in sortedList)
            {
                Destroy(person);
            }

            _generation++;
        }

        private GameObject Breed(GameObject father, GameObject mother)
        {
            var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 6f), 0);
            var offspring = Instantiate(PersonPrefab, position, PersonPrefab.transform.rotation);
            offspring.transform.SetParent(transform);

            var fatherDna = father.GetComponent<ColorDNA2D>();
            var motherDna = mother.GetComponent<ColorDNA2D>();
            var offspringDna = mother.AddComponent<ColorDNA2D>();

            offspringDna.Red = Random.Range(0f, 1f) < .5f ? fatherDna.Red : motherDna.Red;
            offspringDna.Green = Random.Range(0f, 1f) < .5f ? fatherDna.Red : motherDna.Red;
            offspringDna.Blue = Random.Range(0f, 1f) < .5f ? fatherDna.Red : motherDna.Red;
            
            return offspring;
        }

        private void OnGUI()
        {
            _guiStyle.fontSize = 15;
            _guiStyle.normal.textColor = Color.gray;

            GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + _generation, _guiStyle);
            GUI.Label(new Rect(10, 70, 100, 20), "Elapsed Time: " +
                                                 (int) ElapsedTime + "/" + (int) TrialTime, _guiStyle);
        }
    }
}