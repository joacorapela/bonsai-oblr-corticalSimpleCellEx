using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class SelectBatchTs
{
    public IObservable<IList<double>> Process(IObservable<Tuple<PosteriorDataItem, IList<RegressionObservation>>> source)
    {
        return source.Select(it => 
        {
            IList<RegressionObservation> regObservations = it.Item2;
            IList<double> batchTs = new List<double>();
            foreach (RegressionObservation regObservation in regObservations)
            {
                batchTs.Add(regObservation.t);
            }
            return batchTs;
        });
    }
}
