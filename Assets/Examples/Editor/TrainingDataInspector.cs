using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Worq.Worqnets.Core;
using Worq.Worqnets.Enums;
using Worq.Worqnets.Examples.Perceptrons;

namespace Worq.Worqnets.Examples.EditorScripts
{
    [CustomEditor(typeof(TrainingData))]
    public class TrainingDataInspector : Editor
    {
        private TrainingData _target;

//        private int _selectedIndex;

        private void OnEnable()
        {
            SetTarget();
            if (_target == null) return;
            _target.AllDataEntries = _target.AllDataEntries ?? new List<TrainDataEntry>();
            _target.OldNumberOfInputs = _target.NumberOfInputs;
            _target.OldTrainingDataSize = _target.TrainingDataSize;
        }

        private void SetTarget()
        {
            _target = target as TrainingData;
        }

        public override void OnInspectorGUI()
        {
            if (!_target) SetTarget();

            GUILayout.Space(10);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_target);
            }

            _target.TrainingDataSize = EditorGUILayout.IntField("Number Of Training Sets", _target.TrainingDataSize);
            _target.NumberOfInputs = EditorGUILayout.IntField("Inputs per Training Set", _target.NumberOfInputs);

            _target.OutputType = (EPerceptronOutputType) EditorGUILayout.EnumPopup("Output Type", _target.OutputType);

            if (_target.OutputType.Equals(EPerceptronOutputType.String))
            {
                _target.OutputClass1 = EditorGUILayout.TextField("Class 1 (0)", _target.OutputClass1);
                _target.OutputClass2 = EditorGUILayout.TextField("Class 2 (1)", _target.OutputClass2);
            }

            if (string.IsNullOrEmpty(_target.OutputClass1)) _target.OutputClass1 = Constants.Class1DefaultName;
            if (string.IsNullOrEmpty(_target.OutputClass2)) _target.OutputClass2 = Constants.Class2DefaultName;

            if (_target.OldOutputType != _target.OutputType)
            {
                _target.OutputTypeHasChanged = true;
                _target.OldOutputType = _target.OutputType;
            }

            if (_target.OutputClasses == null || _target.OutputClasses.Length < 1 || _target.OutputTypeHasChanged)
            {
                _target.OutputClass1 = Constants.Class1DefaultName;
                _target.OutputClass2 = Constants.Class2DefaultName;

                _target.OutputClasses = new[] {Constants.Class1DefaultName, Constants.Class2DefaultName};
                _target.OutputTypeHasChanged = false;
            }
            else
            {
                _target.OutputClasses[0] = _target.OutputClass1;
                _target.OutputClasses[1] = _target.OutputClass2;
            }

            if (_target.SelectedIndeces == null)
            {
                _target.SelectedIndeces = new int[_target.TrainingDataSize];
            }

            if (_target.AllDataEntries == null || _target.AllDataEntries.Count < 1 ||
                _target.NumberOfInputs != _target.OldNumberOfInputs ||
                _target.TrainingDataSize != _target.OldTrainingDataSize)
            {
                _target.AllDataEntries = new List<TrainDataEntry>();

                for (var i = 0; i < _target.TrainingDataSize; i++)
                {
                    _target.AllDataEntries.Add(new TrainDataEntry(_target.NumberOfInputs));
                }

                _target.OldNumberOfInputs = _target.NumberOfInputs;
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

                        switch (_target.OutputType)
                        {
                            case EPerceptronOutputType.Integer:
                                _target.AllDataEntries[i].IntOutput =
                                    EditorGUILayout.IntField("Output", _target.AllDataEntries[i].IntOutput);
                                break;
                            case EPerceptronOutputType.String:
                                _target.SelectedIndeces[i] = EditorGUILayout.Popup("Output Class",
                                    _target.SelectedIndeces[i], _target.OutputClasses);

                                _target.AllDataEntries[i].StringOutput =
                                    _target.OutputClasses[_target.SelectedIndeces[i]];
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

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