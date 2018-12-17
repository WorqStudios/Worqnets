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
        }

        private void SetTarget()
        {
            _target = target as BinaryClassifier;
        }

        public override void OnInspectorGUI()
        {
            if (!_target) SetTarget();

            if (_target.MaxEpochs < 1) _target.MaxEpochs = 1;

            GUILayout.Space(20);

            _target.TrainingData =
                (TrainingData) EditorGUILayout.ObjectField("Training Data", _target.TrainingData,
                    typeof(TrainingData), false);

            if (!_target.TrainingData) return;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                GUILayout.Space(3);
                _target.MaxEpochs = EditorGUILayout.IntField("Max Epochs", _target.MaxEpochs);
                GUILayout.Space(3);
                _target.Epsilon = EditorGUILayout.FloatField("Epsilon", _target.Epsilon);
                GUILayout.Space(3);
                _target.RepetitionsAfterConverging = EditorGUILayout.IntField("Repeat After Converging",
                    _target.RepetitionsAfterConverging);
                GUILayout.Space(3);
                _target.EnableTrainDebugging = EditorGUILayout.Toggle("Enable Training Debugging",
                    _target.EnableTrainDebugging);
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(5);
            if (GUILayout.Button("Re-Train"))
            {
                _target.Train();
            }

            if (_target.HasTrained)
            {
                GUILayout.Space(5);
                GUI.contentColor = Color.green;
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Training Epochs Performed:", GUILayout.Width(180));
                    EditorGUILayout.LabelField(_target.EpochsPerformed.ToString(), GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
                GUI.contentColor = Color.white;
            }

            if (_target.LastTrainingCouldNotConverge)
            {
                EditorGUILayout.HelpBox("Last Training Could Not Converge", MessageType.Error);
            }

            #region Problem Section

            if (_target.ProblemData == null || _target.ProblemData.Values.Count != _target.TrainingData.Dimension)
            {
                _target.ProblemData = new TrainDataEntry(_target.TrainingData.Dimension);
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                if (_target.HasTrained)
                {
                    EditorGUILayout.LabelField("Problem Data");
                    for (var i = 0; i < _target.TrainingData.Dimension; i++)
                    {
                        _target.ProblemData.Values[i] =
                            EditorGUILayout.FloatField("Input" + (i + 1),
                                _target.ProblemData.Values[i]);
                    }

                    GUILayout.Space(5);
                    if (GUILayout.Button("Predict"))
                    {
                        _target.Predict();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Perform Training first", MessageType.Warning);
                }

                if (!_target.LastTrainingCouldNotConverge)
                {
                    EditorGUILayout.LabelField("Last Prediction Results");
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    {
                        GUILayout.Space(5);
                        GUI.contentColor = Color.green;

                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        {
                            EditorGUILayout.LabelField("Prediction", GUILayout.Width(180));
                            EditorGUILayout.LabelField(_target.ProblemData.Output.ToString(CultureInfo.CurrentCulture),
                                GUILayout.Width(100));
                        }
                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        {
                            EditorGUILayout.LabelField("Prediction Value", GUILayout.Width(180));
                            EditorGUILayout.LabelField(_target.PredictedValue.ToString(CultureInfo.CurrentCulture),
                                GUILayout.Width(100));
                        }
                        EditorGUILayout.EndHorizontal();

                        GUI.contentColor = Color.white;
                    }
                }
            }
            EditorGUILayout.EndVertical();

            #endregion
        }
    }
}