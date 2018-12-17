using System;
using System.Collections;
using System.Collections.Generic;
using Worq.Worqnets.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Worq.Worqnets.Examples.Perceptrons
{
    public class BinaryClassifier : MonoBehaviour
    {
        public TrainingData TrainingData;

        public int MaxEpochs = 100;
        public float Epsilon = 0.01f;
        public int RepetitionsAfterConverging = 2;
        public bool HasTrained;
        public int EpochsPerformed;
        public float PredictedValue;
        public bool EnableDebug = true;
        public bool LastTrainingCouldNotConverge;

        public TrainDataEntry ProblemData;

        private List<float> _weights;
        private float _bias;
        private float _totalError;
        private int _repeatedAfterConverging;

        private void InitializeWeights()
        {
            _weights = new List<float>();

            for (var i = 0; i < TrainingData.Dimension; i += 1)
            {
                _weights.Add(0.0f);
                _weights[i] = Random.Range(-1.0f, 1.0f);
            }

            _bias = Random.Range(-1.0f, 1.0f);
        }

        public void Train()
        {
            StartCoroutine(DoTrain());
        }

        private IEnumerator DoTrain()
        {
            InitializeWeights();

            EpochsPerformed = 0;
            _repeatedAfterConverging = 0;
            LastTrainingCouldNotConverge = false;

            CheckForTrainData();

            do
            {
                _totalError = 0;

                if (EpochsPerformed == MaxEpochs - 1)
                {
                    WorqnetsUtils.PrintMessage("Could Not Converge", WorqnetsUtils.MessageType.Error);
                    LastTrainingCouldNotConverge = true;
                    HasTrained = false;
                    yield break;
                }

                for (var j = 0; j < TrainingData.TrainingDataSize; j++)
                {
                    UpdateWeights(j);
                    if (EnableDebug)
                        Debug.Log("W1: " + _weights[0] + " W2: " + _weights[1] + " Bias: " + _bias);
                }

                if (EnableDebug)
                    Debug.Log("Total Error: " + _totalError);

                if (_totalError <= Epsilon) _repeatedAfterConverging++;
                EpochsPerformed += 1;
            } while (EpochsPerformed < MaxEpochs && _repeatedAfterConverging <= RepetitionsAfterConverging);

            TrainingData.CalculatedWeights = _weights;
            TrainingData.CalculatedBias = _bias;

            yield return new WaitForEndOfFrame();
        }

        private float CalculateWeightedSum(IList<float> inputs, IList<float> weights)
        {
            if (inputs.Count != weights.Count)
            {
                WorqnetsUtils.PrintMessage("Can not train. Inputs and weights do not match",
                    WorqnetsUtils.MessageType.Warning);
                return float.MinValue;
            }

            var weightedSum = 0.0f;

            for (var i = 0; i < inputs.Count; i += 1)
            {
                weightedSum += inputs[i] * weights[i];
            }

            weightedSum += _bias;

            return weightedSum;
        }

        private float CalculateOutput(int index)
        {
            return CalculateWeightedSum(TrainingData.AllDataEntries[index].Values, _weights) > 0 ? 1 : 0;
        }

        private void UpdateWeights(int index)
        {
            var error = TrainingData.AllDataEntries[index].Output - CalculateOutput(index);
            _totalError += Math.Abs(error);

            for (var i = 0; i < _weights.Count; i++)
            {
                _weights[i] += error * TrainingData.AllDataEntries[index].Values[i];
            }

            _bias += error;
        }

        public void Predict()
        {
            if (!HasTrained)
            {
                WorqnetsUtils.PrintMessage("Please Train The Network Before Attempting To Predict",
                    WorqnetsUtils.MessageType.Error);
                return;
            }

            _bias = TrainingData.CalculatedBias;
            PredictedValue = CalculateWeightedSum(ProblemData.Values, TrainingData.CalculatedWeights);
            ProblemData.Output = PredictedValue > 0 ? 1 : 0;
        }

        private void CheckForTrainData()
        {
            if (TrainingData == null) return;

            if (TrainingData.CalculatedWeights == null || TrainingData.CalculatedWeights.Count < 1)
            {
                HasTrained = false;
            }
            else HasTrained = true;
        }
    }
}