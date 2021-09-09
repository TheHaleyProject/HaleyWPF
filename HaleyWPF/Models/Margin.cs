using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows;


namespace Haley.Models
{
    public struct Margin
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Margin(double uniform)
        {
            X = uniform; Y = uniform;
        }
        public Margin (double x_value, double y_value)
        {
            X = x_value;
            Y = y_value;
        }
        public static Margin Parse(string source)
        {
            return new Margin(0.0);
        }
    }
}
