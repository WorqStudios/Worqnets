using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using Worq.Worqnets.Examples.Perceptrons;

namespace Worq.Worqnets.Examples.EditorScripts
{
    [CustomEditor(typeof(BinaryClassifier))]
    public class BinaryClassifierInspector : Editor
    {
        private BinaryClassifier _target;

        private void OnEnable()
        {
            SetTarget();
            if (_target == null) return;
            _target.TrainingData = _target.TrainingData ?? new List<TrainingData>();
            _target.OldDimension = _target.Dimension;
            _target.OldTrainingDataSize = _target.TrainingDataSize;
        }

        private void SetTarget()
        {
            _target = target as BinaryClassifier;
        }

        public override void OnInspectorGUI()
        {
            if (!_target) SetTarget();

            if (GUI.changed)
            {
                _target.HasTrained = false;
            }

            GUILayout.Space(20);

            _target.TrainingDataSize = EditorGUILayout.IntField("Training Set Size", _target.TrainingDataSize);
            _target.Dimension = EditorGUILayout.IntField("Dimension", _target.Dimension);

            if (_target.TrainingData == null || _target.TrainingData.Count < 1 ||
                _target.Dimension != _target.OldDimension || _target.TrainingDataSize != _target.OldTrainingDataSize)
            {
                _target.TrainingData = new List<TrainingData>();
                _target.ProblemData = new TrainingData(_target.Dimension);

                for (var i = 0; i < _target.TrainingDataSize; i++)
                {
                    _target.TrainingData.Add(new TrainingData(_target.Dimension));
                }

                _target.OldDimension = _target.Dimension;
                _target.OldTrainingDataSize = _target.TrainingDataSize;
            }

            GUILayout.Space(10);
            //Training Section
            EditorGUILayout.LabelField("Training");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                for (var i = 0; i < _target.TrainingDataSize; i++)
                {
                    EditorGUILayout.LabelField("Training Data " + (i + 1));
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        for (var j = 0; j < _target.Dimension; j++)
                        {
                            _target.TrainingData[i].Values[j] =
                                EditorGUILayout.FloatField("Input" + (j + 1), _target.TrainingData[i].Values[j]);
                        }

                        GUILayout.Space(5);
                        GUI.contentColor = Color.green;
                        _target.TrainingData[i].Output =
                            EditorGUILayout.FloatField("Output", _target.TrainingData[i].Output);
                        GUI.contentColor = Color.white;
                    }
                    EditorGUILayout.EndVertical();
                }

                GUILayout.Space(10);
                if (GUILayout.Button("Train"))
                {
                    _target.Train();
                }
            }
            EditorGUILayout.EndVertical();
            
            //Prediction Section
            EditorGUILayout.LabelField("Problem");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Problem Data");
                for (var i = 0; i < _target.Dimension; i++)
                {
                    _target.ProblemData.Values[i] =
                        EditorGUILayout.FloatField("Input" + (i + 1), _target.TrainingData[i].Values[i]);
                }
            }


            GUILayout.Space(5);
            if (GUILayout.Button("Predict"))
            {
                _target.Predict();
            }

            EditorGUILayout.LabelField("Results");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.Space(5);
                GUI.contentColor = Color.green;

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Prediction", GUILayout.Width(100));
                    EditorGUILayout.LabelField(_target.ProblemData.Output.ToString(CultureInfo.CurrentCulture),
                        GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Total Epochs", GUILayout.Width(100));
                    EditorGUILayout.LabelField(_target.NumberOfEpochs.ToString(), GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Predicted Bias", GUILayout.Width(100));
                    EditorGUILayout.LabelField(_target.PredictedBias.ToString(CultureInfo.CurrentCulture),
                        GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();

                GUI.contentColor = Color.white;
            }
            EditorGUILayout.EndVertical();
        }
    }
}