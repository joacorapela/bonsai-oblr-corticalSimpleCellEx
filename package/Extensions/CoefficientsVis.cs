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

[assembly: TypeVisualizer(typeof(CoefficientsVis), Target=typeof(PosteriorCalculator))] 

public class CoefficientsVis: DialogTypeVisualizer
{
    private static ScottPlot.FormsPlot _formsPlot1;

    public override void Load(IServiceProvider provider)
    {
        _formsPlot1 = new ScottPlot.FormsPlot() { Dock = DockStyle.Fill };

        var visualizerService = (IDialogTypeVisualizerService)provider.GetService(typeof(IDialogTypeVisualizerService));
        if (visualizerService != null)
        {
            visualizerService.AddControl(_formsPlot1);
        }
    }

    public override void Show(object value)
    {
	PosteriorDataItem pdi = (PosteriorDataItem) value;
	double[] posteriorMean = pdi.mn.ToArray();
	double[] posterior95CI = (pdi.Sn.Diagonal().PointwisePower(0.5) * 1.96).ToArray();

        _formsPlot1.Plot.Clear();
	double[] xs = DataGen.Consecutive(posteriorMean.Count());
	var bar = _formsPlot1.Plot.AddBar(posteriorMean);
	bar.ValueErrors = posterior95CI;
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
