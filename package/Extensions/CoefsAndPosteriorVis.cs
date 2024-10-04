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

[assembly: TypeVisualizer(typeof(CoefsAndPosteriorVis), Target=typeof(PosteriorCalculator))] 

public class CoefsAndPosteriorVis: DialogTypeVisualizer
{
    private static ScottPlot.FormsPlot _formsPlot1;
    private static double[] _coefs;
    private static double[] _coefs95PCI;

    public override void Load(IServiceProvider provider)
    {
        _formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };
	_coefs = CSVReader.ReadCSVToVector(@"C:\Users\user1\bonsai\repos\rfEstimationSimulatedCell\package\Extensions\params\gabor10x10.csv");
        _coefs95PCI = new double[_coefs.Count()];

        var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
        if (visualizerService != null)
        {
            visualizerService.AddControl(_formsPlot1);
        }
    }

    public override void Show(object value)
    {
	PosteriorDataItem pdi = (PosteriorDataItem) value;

        string[] groupNames = Enumerable.Range(0, _coefs.Count()).Select(d => d.ToString()).ToArray();
        string[] seriesNames = { "true", "estimated" };

        _formsPlot1.Plot.Clear();

	double[][] ys = new double[2][];
	double[][] ysErr = new double[2][];

        ys[0] = _coefs;
        ys[1] = pdi.mn.ToArray();
        ysErr[0] = _coefs95PCI;
        ysErr[1] = (pdi.Sn.Diagonal().PointwisePower(0.5) * 1.96).ToArray();

        _formsPlot1.Plot.PlotBarGroups(
            groupLabels: groupNames,
            seriesLabels: seriesNames,
            ys: ys,
            yErr: ysErr);
        _formsPlot1.Plot.XLabel("Index");
        _formsPlot1.Plot.YLabel("Coefficient");

		// customize the plot to make it look nicer
        _formsPlot1.Plot.XAxis.Grid(false); // Disable vertical grid lines
        _formsPlot1.Plot.YAxis.Grid(true);
        _formsPlot1.Plot.Legend(location: Alignment.UpperCenter);

        _formsPlot1.Refresh();
    }

    public override void Unload()
    {
    }
}
