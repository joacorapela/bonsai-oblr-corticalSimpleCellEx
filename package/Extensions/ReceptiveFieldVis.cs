
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

[assembly: TypeVisualizer(typeof(ReceptiveFieldVis), Target=typeof(PosteriorCalculator))] 

public class ReceptiveFieldVis : DialogTypeVisualizer
{
    private static ScottPlot.FormsPlot _formsPlot1;
    private static double[] _coefs;
    private static double[] _observations;
    private static double[] _predictions;
    private static ScottPlot.Plottable.ScatterPlot _scatterPlot;

    private double[,] _toSquareMatrix(double[] value)
    {
        int matrixDim = Convert.ToInt32(Math.Sqrt(value.Count()));
        double[,] matrix = new double[matrixDim, matrixDim];
        int count = 0;
        for (int i=0; i<matrixDim; i++)
        {
            for (int j=0; j<matrixDim; j++)
            {
                matrix[i, j] = value[count];
                count++;
            }
        }
        return matrix;
    }

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

	double[] rf = new double[pdi.mn.Count - 1];
        for(int i=1; i<pdi.mn.Count; i++)
	{
            rf[i-1] = pdi.mn[i];
	}

        _formsPlot1.Plot.Clear();
        var hm = _formsPlot1.Plot.AddHeatmap(this._toSquareMatrix(rf), lockScales: false);
        var cb = _formsPlot1.Plot.AddColorbar(hm);
        _formsPlot1.Plot.XLabel("x");
        _formsPlot1.Plot.YLabel("y");
        _formsPlot1.Refresh();
    }

    public override void Unload()
    {
    }
}
