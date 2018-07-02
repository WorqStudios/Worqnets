using System.Collections.Generic;
using UnityEngine;

namespace Worq.Worqnets.Examples.Perceptrons
{
    public class Perceptron : MonoBehaviour
    {
        public List<TrainingSet> TrainingData;
        
        public int Dimension;
        public int TrainingSetSize;

        private float[] _weights;

        private void Awake()
        {
            _weights = new float[Dimension];
        }
    }
}