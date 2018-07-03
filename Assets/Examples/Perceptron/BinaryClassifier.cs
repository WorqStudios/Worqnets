using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Worq.Worqnets.Examples.Perceptrons
{
    public class BinaryClassifier : MonoBehaviour
    {
        public List<TrainingData> TrainingData;
        
        public int Dimension;
        public int OldDimension;
        public int TrainingDataSize;
        public int OldTrainingDataSize;

        public int NumberOfEpochs;
        public float PredictedBias;
        
        public TrainingData ProblemData;

        private float[] _weights;

        public bool HasTrained;

        public void Train()
        {
            _weights = new float[TrainingDataSize];
            HasTrained = true;
        }

        public void Predict()
        {
            if (!HasTrained)
            {
                WorqnetsUtils.PrintMessage("Please Train The Network Before Attempting To Predict", WorqnetsUtils.MessageType.Error);
                return;
            }   
            
            //Do Predict
            
        }
    }
}