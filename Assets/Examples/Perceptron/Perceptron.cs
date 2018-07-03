using System.Collections.Generic;
using UnityEngine;

namespace Worq.Worqnets.Examples.Perceptrons
{
    public class Perceptron : MonoBehaviour
    {
        public List<TrainingData> TrainingData;
        
        public int Dimension;
        public int OldDimension;
        public int TrainingSetSize;
        public int OldTrainingSetSize;

        public TrainingData ProblemData;

        private float[] _weights;

        private void Awake()
        {
            _weights = new float[Dimension];
        }

        public void Train()
        {
            
        }

        public void Predict()
        {
            
        }
    }
}