using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Worq.Worqnets.Examples.Perceptrons
{
    public class BinaryClassifier : MonoBehaviour
    {
//        public List<TrainDataEntry> TrainingData;
//        
//        public int Dimension;
//        public int OldDimension;
//        public int TrainingDataSize;
//        public int OldTrainingDataSize;
        
        public TrainingData TrainingData;

        public bool HasTrained;
        public int NumberOfEpochs;
        public float PredictedBias;
        
        public TrainDataEntry ProblemData;

        private float[] _weights;
        private float _bias;
        private float _totalError;

        public void Train()
        {
            _weights = new float[TrainingData.TrainingDataSize];
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