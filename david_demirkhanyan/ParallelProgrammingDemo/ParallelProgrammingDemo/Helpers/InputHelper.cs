﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParallelProgrammingDemo.Entities;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using NLog;

namespace ParallelProgrammingDemo.Helpers
{
    public class InputHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Instatiates the Query Vector Set and Data Vector sets
        /// </summary>
        /// <param name="querySetFilePath">optional parameter, if set to null will read default query set file or randomize input</param>
        /// <param name="dataSetFilePath">optional parameter, if set to null will read default data set file or randomize input</param>
        /// <returns></returns>
        public ComputationRequest<float> GetQueryAndDistanceMatrixFromFile(string querySetFilePath = null, string dataSetFilePath = null)
        {
            var computationRequest = new ComputationRequest<float>();

            var fillMatrix = new Action<Matrix<float>, string, bool, string>((target, providedPath, isDefaultEnabled, defaultPath) =>
            {
                if(!String.IsNullOrEmpty(providedPath))
                {
                    var parsedFile = this.ParseCSVFile(providedPath);

                    target = (parsedFile != null) ? parsedFile : new RandomFloatMatrix();
                }
                else if(String.IsNullOrEmpty(providedPath) && isDefaultEnabled)
                {
                    var parsedFile = this.ParseCSVFile(defaultPath);

                    target = (parsedFile != null) ? parsedFile : new RandomFloatMatrix();
                }
                else
                {
                    target = new RandomFloatMatrix();
                }
            });

            fillMatrix(computationRequest.QueryVectors, querySetFilePath, 
                       ConfigurationHelper.Instance.DefaultFilesEnabled, ConfigurationHelper.Instance.QuerySetFilePath);

            fillMatrix(computationRequest.DatasetVectors, dataSetFilePath,
                      ConfigurationHelper.Instance.DefaultFilesEnabled, ConfigurationHelper.Instance.DataSetFilePath);

            return computationRequest;
        }
        
        /// <summary>
        /// parses a CSV file to a T type Matrix
        /// </summary>
        /// <param name="filePath">absolute path to the csv file</param>
        /// <returns></returns>
        private Matrix<float> ParseCSVFile(string filePath)
        {
            try
            {
                var temporaryMatrix = new List<List<float>>();

                using (var parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");

                    while (!parser.EndOfData)
                    {
                        var temporaryRow = new List<float>();

                        string[] fields = parser.ReadFields();
                        foreach (string field in fields)
                        {
                            temporaryRow.Add(float.Parse(field));
                        }

                        temporaryMatrix.Add(temporaryRow);
                    }
                }

                return new Matrix<float>(temporaryMatrix);
            }
            catch(Exception ex)
            {
                logger.Log(LogLevel.Error, ex);
                return null;
            }
            
        }



    }
}