using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Worq.Worqnets.Examples.Perceptrons
{
    public class BinaryClassifier : MonoBehaviour
    {
        public TrainingData TrainingData;

        public int MaxEpochs = 10;
        public bool HasTrained;
        public int NumberOfEpochs;
        public float PredictedBias;

        public TrainDataEntry ProblemData;

        private float[] _weights;
        private float _bias;
        private float _totalError;

        private void InitializeWeights()
        {
            _weights = new float[TrainingData.TrainingDataSize];

            for (var i = 0; i < _weights.Length; i += 1)
            {
                _weights[i] = Random.Range(-1.0f, 1.0f);
            }
            _bias = Random.Range(-1.0f, 1.0f);
        }

        public void Train()
        {
            InitializeWeights();

            for (var i = 0; i < NumberOfEpochs; i++)
            {
                _totalError = 0;
                
                for (var j = 0; j < TrainingData.TrainingDataSize; j++)
                {
                    UpdateWeights(j);
                    Debug.Log("W1: " + _weights[0] + "W2: " + _weights[1] + "Bias: " + _bias);
                }

                Debug.Log("Total Error: " + _totalError);
            }
            
            HasTrained = true;
        }

        private void UpdateWeights(int weight)
        {
            
        }

        public void Predict()
        {
            if (!HasTrained)
            {
                WorqnetsUtils.PrintMessage("Please Train The Network Before Attempting To Predict",
                    WorqnetsUtils.MessageType.Error);
                return;
            }

            //Do Predict
        }
    }
}