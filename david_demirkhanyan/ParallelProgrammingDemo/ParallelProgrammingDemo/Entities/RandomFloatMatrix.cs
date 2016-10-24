﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelProgrammingDemo.Entities
{
    public class RandomFloatMatrix : Matrix<float>
    {
        public RandomFloatMatrix() : base()
        {
            this.RandomizeMatrix();
        }

        /// <summary>
        /// Randomizes matrix
        /// </summary>
        protected void RandomizeMatrix()
        {
            Random rand = new Random();
            
            for(int i=0; i<height; i++)
            {
                for(int j=0; j<width; j++)
                {
                    matrixValues[i][j] = this.NextFloat(rand);
                }
            }
        }

        /// <summary>
        /// magic
        /// </summary>
        /// <param name="rand"></param>
        /// <returns></returns>
        protected float NextFloat(Random rand)
        {
           return (float)(float.MaxValue * 2.0 * (rand.NextDouble() - 0.5));
        }
    }
}