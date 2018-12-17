﻿using System;
using System.Collections.Generic;

namespace Worq.Worqnets.Examples.Perceptrons
{
    [Serializable]
    public class TrainDataEntry
    {
        public List<float> Values;
        public float Output;

        public TrainDataEntry(int dimension)
        {
            Values = new List<float>();

            for (var i = 0; i < dimension; i++)
            {
                Values.Add(0.0f);
            }
        }
    }
}