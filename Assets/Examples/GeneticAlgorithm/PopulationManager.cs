using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Worq.Worqnets.Examples.GeneticAlgorithm
{
    public class PopulationManager : MonoBehaviour
    {
        public static float ElapsedTime;

        public GameObject PersonPrefab;
        public int PopulationSize = 10;
        public float TrialTime = 10f;
        public SelectionCriteria SelectionCriteria;
        [Range(0, 100)] public float PercentageMutation = 5f;

        private int _generation = 1;

        private readonly List<GameObject> _population = new List<GameObject>();

        private readonly GUIStyle _guiStyle = new GUIStyle();

        private void Awake()
        {
            for (var i = 0; i < PopulationSize; i++)
            {
                var position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 6f), 0);
                var entity = Instantiate(PersonPrefab, position, PersonPrefab.transform.rotation);
                
                SetRandomSize(entity);
                
                entity.transform.SetParent(transform);

                if (!entity.GetComponent<DNA>()) entity.AddComponent<DNA>();
                var dna = entity.GetComponent<DNA>();

                dna.Red = Random.Range(0, 1f);
                dna.Green = Random.Range(0, 1f);
                dna.Blue = Random.Range(0, 1f);

                _population.Add(entity);
            }
        }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;

            // ReSharper disable once InvertIf
            if (ElapsedTime > TrialTime)
            {
                BreedNewGeneration();
                ElapsedTime = 0;
            }
        }

        private void BreedNewGeneration()
        {
            var sortedList = _population.OrderBy(o => o.GetComponent<DNA>().SurvivalTime).ToList();

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

            var fatherDna = father.GetComponent<DNA>();
            var motherDna = mother.GetComponent<DNA>();

            var offspringDna = offspring.GetComponent<DNA>();

            var percentageMutation = PercentageMutation / 100 * PopulationSize;

            switch (SelectionCriteria)
            {
                case SelectionCriteria.Color:
                    if (Random.Range(1, PopulationSize) > percentageMutation)
                    {
                        InheritColor(fatherDna, motherDna, offspringDna);
                        SetRandomSize(offspring);
                    }
                    else
                    {
                        SetRandomColor(offspringDna);
                        SetRandomSize(offspring);
                    }

                    break;
                case SelectionCriteria.Size:
                    if (Random.Range(0, PopulationSize) > percentageMutation)
                    {
                        InheritSize(father, mother, offspring);
                        SetRandomColor(offspringDna);
                    }
                    else
                    {
                        SetRandomSize(offspring);
                        SetRandomColor(offspringDna);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return offspring;
        }

        private static void InheritSize(GameObject father, GameObject mother, GameObject offspring)
        {
            offspring.transform.localScale = new Vector3(
                Random.Range(father.transform.localScale.x , mother.transform.localScale.x),
                Random.Range(father.transform.localScale.y , mother.transform.localScale.y),
                Random.Range(father.transform.localScale.z , mother.transform.localScale.z)
            );
            
            offspring.transform.localScale = 
                Random.Range(0f, 1f) < .5f ? father.transform.localScale : mother.transform.localScale;
        }

        private static void SetRandomColor(DNA offspringDna)
        {
            offspringDna.Red = Random.Range(0, 1f);
            offspringDna.Green = Random.Range(0, 1f);
            offspringDna.Blue = Random.Range(0, 1f);
        }

        private static void SetRandomSize(GameObject entity)
        {
            var randomSize = Random.Range(.5f, 2f);
            
            entity.transform.localScale = new Vector3(
                entity.transform.localScale.x * randomSize,
                entity.transform.localScale.y * randomSize,
                entity.transform.localScale.z);
        }

        private static void InheritColor(DNA fatherDna, DNA motherDna, DNA offspringDna)
        {
            offspringDna.Red = Random.Range(0f, 1f) < .5f ? fatherDna.Red : motherDna.Red;
            offspringDna.Green = Random.Range(0f, 1f) < .5f ? fatherDna.Green : motherDna.Green;
            offspringDna.Blue = Random.Range(0f, 1f) < .5f ? fatherDna.Blue : motherDna.Blue;
        }

        private void OnGUI()
        {
            _guiStyle.fontSize = 15;
            _guiStyle.normal.textColor = Color.red;

            GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + _generation, _guiStyle);
            GUI.Label(new Rect(10, 70, 100, 20), "Elapsed Time: " +
                                                 (int) ElapsedTime + "/" + (int) TrialTime, _guiStyle);
        }
    }
}