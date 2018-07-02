using System;
using System.Collections.Generic;

namespace Worq.Worqnets.Examples.Perceptrons
{
    [Serializable]
    public class TrainingSet
    {
        public List<float> Values;
        public float Output;

        public TrainingSet(int dimension)
        {
            Values = new List<float>();

            for (var i = 0; i < dimension; i++)
            {
                Values.Add(0.0f);
            }
        }
    }
}