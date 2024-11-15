using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class PredictionsVsResponsesDummy
{
    public IObservable<Tuple<IList<Tuple<double, double>>, IList<double>>> Process(IObservable<Tuple<IList<Tuple<double, double>>, IList<double>>> source)
    {
        return source.Select(value => value);
    }
}
