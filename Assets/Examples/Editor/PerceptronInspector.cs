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
            _target.TrainingData = _target.TrainingData ?? new List<TrainingSet>();
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(20);

            _target.TrainingSetSize = EditorGUILayout.IntField("Training Set Size", _target.TrainingSetSize);
            _target.Dimension = EditorGUILayout.IntField("Dimension", _target.Dimension);

            if (_target.TrainingData == null || _target.TrainingData.Count < 1)
            {
                _target.TrainingData = new List<TrainingSet>();
                for (var i = 0; i < _target.TrainingSetSize; i++)
                {
                    _target.TrainingData.Add(new TrainingSet(_target.Dimension));
                }
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
        }
    }
}