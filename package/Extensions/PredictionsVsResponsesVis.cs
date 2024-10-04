
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
using ScottPlot;
using ScottPlot.Plottable;

[assembly: TypeVisualizer(typeof(PredictionsAndResponsesVis), Target=typeof(BufferPredictionsAndResponses))] 

public class PredictionsAndResponsesVis : DialogTypeVisualizer
{
    private static ScottPlot.FormsPlot _formsPlot1;
    private static double[] _coefs;
    private static double[] _observations;
    private static double[] _predictions;
    private static ScottPlot.Plottable.ScatterPlot _scatterPlot;

    public override void Load(IServiceProvider provider)
    {
        _formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };
	_coefs = CSVReader.ReadCSVToVector(@"C:\Users\user1\bonsai\repos\rfEstimationSimulatedCell\package\Extensions\params\gabor10x10.csv");
        int numPointsToSimDisplay = 20;
        _observations = new double[numPointsToSimDisplay];
        _predictions = new double[numPointsToSimDisplay];
        _formsPlot1.Plot.AddScatter(new double[] { -4.0, 4.0 }, new double[] { -4.0, 4.0 }, lineWidth: 1, color: Color.Red);
        _scatterPlot = _formsPlot1.Plot.AddScatter(_observations, _predictions, lineWidth: 0, color: Color.Blue);
        _formsPlot1.Plot.XLabel("Observations");
        _formsPlot1.Plot.YLabel("Predictions");
        _formsPlot1.Plot.SetAxisLimits(-4.0, 4.0, -4.0, 4.0);
        _formsPlot1.Refresh();

        var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
        if (visualizerService != null)
        {
            visualizerService.AddControl(_formsPlot1);
        }
    }

    public override void Show(object value)
    {
        Tuple<double, double> pair = (Tuple<double, double>) value;
        Array.Copy(_observations, 1, _observations, 0, _observations.Length - 1);
        Array.Copy(_predictions, 1, _predictions, 0, _predictions.Length - 1);
        _observations[_observations.Length-1] = pair.Item1;
        _predictions[_observations.Length-1] = pair.Item2;
        _scatterPlot.Update(_observations, _predictions);
        _formsPlot1.Refresh();
    }

    public override void Unload()
    {
    }
}
