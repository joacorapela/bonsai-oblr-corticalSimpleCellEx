using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MathNet.Numerics.LinearAlgebra;

[Combinator]
[Description("This Node converts ither double[] or double[][] to MathNetVector or MathNetMatrix respectively")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class ArrayToMathNet
{
    public IObservable<Vector<double>> Process(IObservable<double[]> source)
    {
        return source.Select(value => Vector<double>.Build.DenseOfArray(value));
    }
    public IObservable<Matrix<double>> Process(IObservable<double[][]> source)
    {
        return source.Select(value => Matrix<double>.Build.DenseOfColumns(value));
    }
}
