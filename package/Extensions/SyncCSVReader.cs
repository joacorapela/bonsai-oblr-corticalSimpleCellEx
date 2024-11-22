using Bonsai;
using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.IO;

[Combinator]
[Description("")]
[WorkflowElementCategory(ElementCategory.Source)]
public class SyncCSVReader
{
    public string filename { get; set; }
    public char separator { get; set; }

    public IObservable<double[]> Process()
    {
        // Create an observable that reads the CSV file line by line
        return Observable.Create<double[]>(observer =>
        {
            try
            {
                using (var reader = new StreamReader(this.filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
		        Console.WriteLine(line);
                        // Split the line into fields (assuming comma as the separator)
                        string[] fields = line.Split(this.separator);
		        for (int i=0; i<fields.Length; i++)
			{
		            Console.WriteLine(fields[i]);
			}
   
			double[] doubleFields = fields.Select(double.Parse).ToArray();
		        for (int i=0; i<fields.Length; i++)
			{
		            Console.WriteLine(String.Format("{0}", doubleFields[i]));
			}
   

                        // Notify the observer of the parsed line
                        observer.OnNext(doubleFields);
                    }
                }
    
                // Signal completion when all lines are read
                observer.OnCompleted();
            }
            catch (Exception ex)
            {
                // Signal any errors to the observer
                observer.OnError(ex);
            }

            // Return a no-op disposable since there's no active resource to dispose
            return System.Reactive.Disposables.Disposable.Empty;
        });
    }
}
