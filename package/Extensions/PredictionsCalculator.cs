using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Reactive.Linq;
using MathNet.Numerics.LinearAlgebra;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class PredictionsCalculator
{
    public double beta {get; set;}

    public IObservable<ValueTuple<double, double>> Process(IObservable<Tuple<Vector<double>,PosteriorDataItem>> source)
    {
        Console.WriteLine("PredictionsCalculator::Process called");
        IObservable<ValueTuple<double, double>> answer = source.Select(phiAndPDI =>
        {
            // Assuming BayesianLinearRegression.Predict returns a Tuple<double, double>
            var prediction = BayesianLinearRegression.Predict(
                phi: phiAndPDI.Item1, 
                mn: phiAndPDI.Item2.mn, 
                Sn: phiAndPDI.Item2.Sn, 
                beta: this.beta
            );

            // Convert Tuple to ValueTuple
            return ValueTuple.Create(prediction.Item1, prediction.Item2);
        });
        return answer;
    }
}
