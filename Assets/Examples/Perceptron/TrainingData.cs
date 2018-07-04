using System.Collections.Generic;
using UnityEngine;

namespace Worq.Worqnets.Examples.Perceptrons
{
    [CreateAssetMenu(fileName =  "Perceptron Training Data", menuName = "Worqnets/Perceptron Training Data")]
    public class TrainingData : ScriptableObject
    {
        public List<TrainDataEntry> AllDataEntries;
        public int Dimension;
        public int OldDimension;
        public int TrainingDataSize;
        public int OldTrainingDataSize;

        public List<float> CalculatedWeights;
        public float CalculatedBias;
    }
}