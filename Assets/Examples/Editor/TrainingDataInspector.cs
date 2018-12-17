using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Worq.Worqnets.Examples.Perceptrons;

namespace Worq.Worqnets.Examples.EditorScripts
{
    [CustomEditor(typeof(TrainingData))]
    public class TrainingDataInspector : Editor
    {
        private TrainingData _target;

        private void OnEnable()
        {
            SetTarget();
            if (_target == null) return;
            _target.AllDataEntries = _target.AllDataEntries ?? new List<TrainDataEntry>();
            _target.OldDimension = _target.NumberOfInputs;
            _target.OldTrainingDataSize = _target.TrainingDataSize;
        }

        private void SetTarget()
        {
            _target = target as TrainingData;
        }

        public override void OnInspectorGUI()
        {
            if (!_target) SetTarget();

            GUILayout.Space(20);

            if (GUI.changed) EditorUtility.SetDirty(_target);

            _target.TrainingDataSize = EditorGUILayout.IntField("Number Of Training Sets", _target.TrainingDataSize);
            _target.NumberOfInputs = EditorGUILayout.IntField("Inputs per Training Set", _target.NumberOfInputs);

            if (_target.AllDataEntries == null || _target.AllDataEntries.Count < 1 ||
                _target.NumberOfInputs != _target.OldDimension ||
                _target.TrainingDataSize != _target.OldTrainingDataSize)
            {
                _target.AllDataEntries = new List<TrainDataEntry>();

                for (var i = 0; i < _target.TrainingDataSize; i++)
                {
                    _target.AllDataEntries.Add(new TrainDataEntry(_target.NumberOfInputs));
                }

                _target.OldDimension = _target.NumberOfInputs;
                _target.OldTrainingDataSize = _target.TrainingDataSize;
            }

            GUILayout.Space(10);

            #region Training Section

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                for (var i = 0; i < _target.TrainingDataSize; i++)
                {
                    EditorGUILayout.LabelField("Training Set " + (i + 1));
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        for (var j = 0; j < _target.NumberOfInputs; j++)
                        {
                            _target.AllDataEntries[i].Values[j] =
                                EditorGUILayout.FloatField("Input " + (j + 1), _target.AllDataEntries[i].Values[j]);
                        }

                        GUILayout.Space(5);
                        GUI.contentColor = Color.green;
                        _target.AllDataEntries[i].Output =
                            EditorGUILayout.FloatField("Output", _target.AllDataEntries[i].Output);
                        GUI.contentColor = Color.white;
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }

        #endregion
    }
}