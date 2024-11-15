
using System;
using System.IO;

public class Utils
{
    public static double[] ReadCSVTo1DArray(string filename, char sep=',')
    {
        // Read all lines from the CSV file
        string[] lines = File.ReadAllLines(filename);
        string[] values_str = lines[0].Split(sep);

        // Determine the dimensions of the matrix
        int numElem = values_str.Length;

        // Create the double vector
        double[] values_double = new double[numElem];

        // Fill the matrix with the data from the CSV
        for (int i = 0; i < numElem; i++)
        {
            values_double[i] = Convert.ToDouble(values_str[i]);
        }

        return values_double;
    }

    public static double[,] ReadCSVTo2DArray(string filename, char sep=',')
    {
        string[] lines = File.ReadAllLines(filename);
        string[] values_str = lines[0].Split(sep);
        int nRows = lines.Length;
        int nCols = values_str.Length;
        // Console.WriteLine($"nRows={nRows}, nCols={nCols}");
        double[,] answer = new double[nRows, nCols];
        for (int i = 0; i < nRows; i++)
        {
            values_str = lines[i].Split(sep);
            for (int j = 0; j < nCols; j++)
            {
            	answer[i, j] = Convert.ToDouble(values_str[j]);
            }
        }
        return answer;
    }

    public static double[] DelayResponses(int delay, double[] responses)
    {
        double[] dResponses = new double[responses.Length - delay];
        for (int i=delay; i<responses.Length; i++)
        {
            dResponses[i-delay] = responses[i];
        }
        return dResponses;
    }

    public static double[,] DelayImages(int delay, double[,] images)
    {
        int rows = images.GetLength(0);
        int cols = images.GetLength(1);

        double[,] dImages = new double[rows-delay, cols];

        for (int i = 0; i < rows-delay; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                dImages[i, j] = images[i, j];
            }
        }
        return dImages;
    }

}

