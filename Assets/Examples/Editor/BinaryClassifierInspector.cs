﻿using System.Globalization;
using DefaultNamespace;
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
                Debug.Log("Some values chaned");
                _target.HasTrained = false;
            }

            GUILayout.Space(20);

            _target.TrainingData =
                (TrainingData) EditorGUILayout.ObjectField("Training Data", _target.TrainingData, typeof(TrainingData));

            if (!_target.TrainingData) return;

            GUILayout.Space(10);
            if (GUILayout.Button("Train"))
            {
                _target.Train();
            }

            #region Problem Section

            if (_target.ProblemData == null)
            {
                _target.ProblemData = new TrainDataEntry(_target.TrainingData.Dimension);
            }

            if (GUILayout.Button("Problem", EditorStyles.boldLabel))
            {
                WorqnetsVariables.BcProblemExpanded = !WorqnetsVariables.BcProblemExpanded;
            }

            // ReSharper disable once InvertIf
            if (WorqnetsVariables.BcProblemExpanded)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Problem Data");
                    for (var i = 0; i < _target.TrainingData.Dimension; i++)
                    {
                        _target.ProblemData.Values[i] =
                            EditorGUILayout.FloatField("Input" + (i + 1),
                                _target.TrainingData.AllDataEntries[i].Values[i]);
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

            #endregion
        }
    }
}