using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using MathNet.Numerics.Distributions;
using System.Xml.Serialization;
using JoacoRapela.Bonsai.ML.OnlineBayesianLinearRegression;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Transform)]
public class VisualCellResponsesDataSource
{
    private Matrix<double> _images;
    private Vector<double> _responses;

    public int delay { get; set; }

    [Description("The name of the images file.")]
    [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
    public string imagesFilename { set; get; }

    [Description("The name of the responses file.")]
    [Editor("Bonsai.Design.OpenFileNameEditor, Bonsai.Design", DesignTypes.UITypeEditor)]
    public string responsesFilename { set; get; }

    public IObservable<RegressionObservation> Process(IObservable<long> timerO)
    {
        Console.WriteLine("VisualCellResponsesDataSource::Process called");

	var iBuffer = Utils.ReadCSVTo2DArray(this.imagesFilename, ' ');
	var dIBuffer = Utils.DelayImages(this.delay, iBuffer);
	this._images = Matrix<double>.Build.DenseOfArray(dIBuffer);

	var rBuffer = Utils.ReadCSVTo1DArray(this.responsesFilename, ' ');
	var dRBuffer = Utils.DelayResponses(this.delay, rBuffer);
        this._responses = Vector<double>.Build.DenseOfArray(dRBuffer);

        System.Random rng = SystemRandomSource.Default;
        int n = 0;
        int maxIndex = Math.Min(this._images.RowCount, this._responses.Count); // Set max bounds
        return timerO.Select(time =>
        {
            // Console.WriteLine(string.Format("Providing visual cell response number: {}",n));
            if (n >= maxIndex)
            {
                Console.WriteLine("Reached end of data, stopping sequence.");
                return null; // You can change this if you prefer to handle it differently
            }

	    Vector<double> phi = Vector<double>.Build.Dense(this._images.ColumnCount + 1);
	    phi[0] = 1.0;
	    for (int i = 1; i <= this._images.ColumnCount; i++)
	    {
                phi[i] = this._images[n, i-1];
	    }

            RegressionObservation observation = new RegressionObservation();
            observation.phi = phi;
            observation.t = this._responses.At(n);
            n = n + 1;
            return observation;
        })
        .TakeWhile(observation => observation != null);
    }
}
