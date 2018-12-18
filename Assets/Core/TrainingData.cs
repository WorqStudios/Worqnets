using System.Collections.Generic;
using UnityEngine;
using Worq.Worqnets.Enums;

namespace Worq.Worqnets.Examples.Perceptrons
{
    [CreateAssetMenu(fileName =  "Perceptron Training Data", menuName = "Worqnets/Perceptron Training Data")]
    public class TrainingData : ScriptableObject
    {
        public List<TrainDataEntry> AllDataEntries;
        public int NumberOfInputs;
        public int OldNumberOfInputs;
        public int TrainingDataSize;
        public int OldTrainingDataSize;

        public EPerceptronOutputType OutputType;
        public EPerceptronOutputType OldOutputType;

        public string OutputClass1;
        public string OutputClass2;
        
        public string[] OutputClasses;
        public int[] SelectedIndeces;

        public bool OutputTypeHasChanged;

        public List<float> CalculatedWeights;
        public float CalculatedBias;
    }
}