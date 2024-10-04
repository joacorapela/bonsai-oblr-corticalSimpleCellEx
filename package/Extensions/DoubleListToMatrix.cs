using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Globalization;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class DoubleListToMatrix
{
    public IObservable<double[,]> Process(IObservable<double[][]> source)
    {
        return source.Select(value => 
        {
            int nRows = value.Length;
            int nCols = value[0].Length;
            double[,] answer = new double[nRows, nCols];
            for (int i = 0; i < nRows; i++)
            {
                for (int j = 0; j < nCols; j++)
                {
                    answer[i, j] = value[i][j];
                }
            }
            return answer;
        });
    }
}
