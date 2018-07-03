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

            if (GUI.changed)
            {
                Debug.Log("Some values chaned");
                _target.HasTrained = false;
            }

            GUILayout.Space(20);

            _target.TrainingData =
                (TrainingData) EditorGUILayout.ObjectField("Training Data", _target.TrainingData, 
                    typeof(TrainingData), false);

            if (!_target.TrainingData) return;

            GUILayout.Space(5);
            _target.MaxEpochs = EditorGUILayout.IntField("Max Epochs", _target.MaxEpochs);
            GUILayout.Space(5);
            
            if (GUILayout.Button("Train"))
            {
                _target.Train();
            }

            #region Problem Section

            if (_target.ProblemData == null || _target.ProblemData.Values.Count != _target.TrainingData.Dimension)
            {
                _target.ProblemData = new TrainDataEntry(_target.TrainingData.Dimension);
            }

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Problem Data");
                for (var i = 0; i < _target.TrainingData.Dimension; i++)
                {
                    _target.ProblemData.Values[i] =
                        EditorGUILayout.FloatField("Input" + (i + 1),
                            _target.ProblemData.Values[i]);
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
                    EditorGUILayout.LabelField("Prediction Value", GUILayout.Width(100));
                    EditorGUILayout.LabelField(_target.PredictedValue.ToString(CultureInfo.CurrentCulture),
                        GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                {
                    EditorGUILayout.LabelField("Total Epochs", GUILayout.Width(100));
                    EditorGUILayout.LabelField(_target.EpochsPerformed.ToString(), GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();

                GUI.contentColor = Color.white;
            }
            EditorGUILayout.EndVertical();

            #endregion
        }
    }
}