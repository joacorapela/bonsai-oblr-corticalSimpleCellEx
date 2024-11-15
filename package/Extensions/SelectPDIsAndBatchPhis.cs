using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MathNet.Numerics.LinearAlgebra;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class SelectPDIsAndBatchPhis
{
    public IObservable<Tuple<PosteriorDataItem, IList<Vector<double>>>> Process(IObservable<Tuple<PosteriorDataItem, IList<RegressionObservation>>> source)
    {
        return source.Select(it => 
        {
            IList<RegressionObservation> regObservations = it.Item2;
            IList<Vector<double>> batchPhis = new List<Vector<double>>();
            foreach (RegressionObservation regObservation in regObservations)
            {   
                batchPhis.Add(regObservation.phi);
            }
	    Tuple<PosteriorDataItem, IList<Vector<double>>> answer =  Tuple.Create(it.Item1, batchPhis);
            return answer;
        });
    }
}
