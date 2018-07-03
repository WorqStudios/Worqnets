using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Worq.Worqnets.Examples.Perceptrons;

namespace Worq.Worqnets.Examples.EditorScripts
{
    [CustomEditor(typeof(Perceptron))]
    public class PerceptronInspector : Editor
    {
        private Perceptron _target;

        private void OnEnable()
        {
            _target = target as Perceptron;
            _target.TrainingData = _target.TrainingData ?? new List<TrainingData>();
            _target.OldDimension = _target.Dimension;
            _target.OldTrainingSetSize = _target.TrainingSetSize;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);

            _target.TrainingSetSize = EditorGUILayout.IntField("Training Set Size", _target.TrainingSetSize);
            _target.Dimension = EditorGUILayout.IntField("Dimension", _target.Dimension);

            if (_target.TrainingData == null || _target.TrainingData.Count < 1 ||
                _target.Dimension != _target.OldDimension || _target.TrainingSetSize != _target.OldTrainingSetSize)
            {
                _target.TrainingData = new List<TrainingData>();
                _target.ProblemData = new TrainingData(_target.Dimension);

                for (var i = 0; i < _target.TrainingSetSize; i++)
                {
                    _target.TrainingData.Add(new TrainingData(_target.Dimension));
                }

                _target.OldDimension = _target.Dimension;
                _target.OldTrainingSetSize = _target.TrainingSetSize;
            }

            GUILayout.Space(10);
            for (var i = 0; i < _target.TrainingSetSize; i++)
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

            GUILayout.Space(5);
            if (GUILayout.Button("Predict"))
            {
                _target.Predict();
            }

            EditorGUILayout.LabelField("Problem Data");
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                for (var i = 0; i < _target.Dimension; i++)
                {
                    _target.ProblemData.Values[i] =
                        EditorGUILayout.FloatField("Input" + (i + 1), _target.TrainingData[i].Values[i]);
                }

                GUILayout.Space(5);
                GUI.contentColor = Color.green;
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Prediction");
                    EditorGUILayout.LabelField(_target.ProblemData.Output.ToString());
                }
                GUI.contentColor = Color.white;
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}