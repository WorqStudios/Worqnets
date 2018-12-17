using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Worq.Worqnets.Scripts;
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
        public bool EnableTrainDebugging = true;
        public bool LastTrainingCouldNotConverge;

        public TrainDataEntry ProblemData;

        private List<float> _weights;
        private float _bias;
        private float _totalError;
        private int _repeatedAfterConverging;

        /// <summary>
        /// Sets the initial weights and bias for our network
        /// </summary>
        private void InitializeWeights()
        {
            //Initialize the weights
            _weights = new List<float>();

            //For the dimension of the training data, add a new entry and set it's value to a
            //random value between -1 and 1.
            for (var i = 0; i < TrainingData.NumberOfInputs; i += 1)
            {
                //Add a new entry
                _weights.Add(0.0f);
                //set it's value to a random value between -1 and 1.
                _weights[i] = Random.Range(-1.0f, 1.0f);
            }

            //Set a random value for the bias.
            _bias = Random.Range(-1.0f, 1.0f);
        }

        /// <summary>
        /// Trains the perceptron network.
        /// </summary>
        public void Train()
        {
            //Perform the actual training in a co-routine.
            StartCoroutine(DoTrain());
        }

        /// <summary>
        /// Co-routine to perform the training of the perceptron. This ensures that training is done on a separate thread so as to avoid freezing the program.
        /// </summary>
        /// <returns>Returns Wait For End Of Frame. Continues at the end of the current frame.</returns>
        private IEnumerator DoTrain()
        {
            InitializeWeights();

            EpochsPerformed = 0;
            _repeatedAfterConverging = 0;
            LastTrainingCouldNotConverge = false;

            //Get out of training if train data is null.
            if (!TrainingData) yield break;
            CheckForTrainData();
            //Train at least once.
            do
            {
                //Set total error euals zero.
                _totalError = 0;

                //Get out of training if max number of epochs has been reached (training could not converge).
                if (EpochsPerformed == MaxEpochs - 1)
                {
                    WorqnetsUtils.PrintMessage("Could Not Converge", WorqnetsUtils.MessageType.Error);
                    LastTrainingCouldNotConverge = true;
                    HasTrained = false;
                    yield break;
                }

                for (var i = 0; i < TrainingData.TrainingDataSize; i++)
                {
                    UpdateWeights(i);

                    if (EnableTrainDebugging)
                        Debug.Log("W1: " + _weights[0] + " W2: " + _weights[1] + " Bias: " + _bias);
                }

                if (EnableTrainDebugging)
                    Debug.Log("Total Error: " + _totalError);

                if (_totalError <= Epsilon) _repeatedAfterConverging++;
                EpochsPerformed += 1;
            } while (EpochsPerformed < MaxEpochs && _repeatedAfterConverging <= RepetitionsAfterConverging);

            TrainingData.CalculatedWeights = _weights;
            TrainingData.CalculatedBias = _bias;

            yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// Updates the weights of training data at the current index.
        /// </summary>
        /// <param name="index">The position of the training data in the training set.</param>
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

        /// <summary>
        /// Computes the output of the training data at a particular index.
        /// </summary>
        /// <param name="index">The position of the training data in the training set.</param>
        /// <returns></returns>
        private float CalculateOutput(int index)
        {
            return CalculateWeightedSum(TrainingData.AllDataEntries[index].Values, _weights) > 0 ? 1 : 0;
        }

        /// <summary>
        /// Predicts the output of the provided data after training has occured.
        /// </summary>
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

        /// <summary>
        /// Check to see if a training data has been provided. No use training if training data does not exist.
        /// </summary>
        private void CheckForTrainData()
        {
            if (TrainingData.CalculatedWeights == null || TrainingData.CalculatedWeights.Count < 1)
            {
                HasTrained = false;
            }
            else
            {
                HasTrained = true;
            }
        }
    }
}