
using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Drawing;
using Bonsai.Design;
using System.Windows.Forms;
using Bonsai.Design.Visualizers;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;
using MathNet.Numerics.Statistics;
using ScottPlot;
using ScottPlot.Plottable;

[assembly: TypeVisualizer(typeof(PredictionsVsResponsesVis), Target=typeof(PredictionsVsResponsesDummy))] 

public class PredictionsVsResponsesVis : DialogTypeVisualizer
{
    private ScottPlot.FormsPlot _formsPlot1;
    private ScottPlot.Plottable.ScatterPlot _scatterPlot;
    private double[] _axisLimits = new double[] { 0.0, 15.0, 0.0, 3.0 };

    public override void Load(IServiceProvider provider)
    {
        this._formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };
        this._scatterPlot = this._formsPlot1.Plot.AddScatter(new double[] { this._axisLimits[0], this._axisLimits[1] }, new double[] { this._axisLimits[2], this._axisLimits[3] }, lineWidth: 0, color: Color.Red);
        this._formsPlot1.Plot.XLabel("Observations");
        this._formsPlot1.Plot.YLabel("Predicted Means");
        this._formsPlot1.Plot.SetAxisLimits(this._axisLimits[0], this._axisLimits[1], this._axisLimits[2], this._axisLimits[3]);
        this._formsPlot1.Refresh();

        var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
        if (visualizerService != null)
        {
            visualizerService.AddControl(_formsPlot1);
        }
    }

    public override void Show(object value)
    // value.Item1[i].Item1: mean of prediction i
    // value.Item1[i].Item2: var of prediction i
    // value.Item2: observations
    {
        var pair = (Tuple<IList<Tuple<double, double>>, IList<double>>) value;
        double[] observations = new double[pair.Item2.Count()];
        for (int i=0; i<observations.Length; i++)
        {
            observations[i] = pair.Item2[i];
        }
        double[] predicted_means = new double[pair.Item1.Count()];
        for (int i=0; i<predicted_means.Length; i++)
        {
            predicted_means[i] = pair.Item1[i].Item1;
        }
        this._scatterPlot.Update(observations, predicted_means);
        double corCoef = Correlation.Pearson(observations, predicted_means);
        this._formsPlot1.Plot.Title(String.Format("Correlation Coefficient: {0:F2}", corCoef));
        this._formsPlot1.Refresh();
    }

    public override void Unload()
    {
    }
}
