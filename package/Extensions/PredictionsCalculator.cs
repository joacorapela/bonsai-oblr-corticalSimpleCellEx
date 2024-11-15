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

    public IObservable<IList<Tuple<double, double>>> Process(IObservable<Tuple<PosteriorDataItem, IList<Vector<double>>>> pdiAndBatchPhisO)
    {
        Console.WriteLine("PredictionsCalculator::Process called");
        IObservable<IList<Tuple<double, double>>> answer = pdiAndBatchPhisO.Select(
            pdiAndBatchPhis =>
            {
                PosteriorDataItem pdi = pdiAndBatchPhis.Item1;
                IList<Vector<double>> phis = pdiAndBatchPhis.Item2;
                List<Tuple<double, double>> predictions = new List<Tuple<double, double>>();
                foreach (Vector<double> phi in phis)
                {
                    var prediction = BayesianLinearRegression.Predict(phi: phi, mn: pdi.mn, Sn: pdi.Sn, beta: this.beta);
		    Tuple<double, double> predictionTuple = Tuple.Create(prediction.Item1, prediction.Item2);
                    predictions.Add(predictionTuple);
                }
                // var predictions = phis.Select(phi => BayesianLinearRegression.Predict(phi: phi, mn: pdi.mn, Sn: pdi.Sn, beta: this.beta));
                return predictions;
            });
        return answer;
     }
}
