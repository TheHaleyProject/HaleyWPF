using Haley.Abstractions;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Haley.Utils;
using Haley.Enums;
using Haley.Models;
using System.Text.RegularExpressions;


namespace Haley.WPF.Controls
{
    public class Incrementer : TextBox, ICornerRadius
    {
        private readonly Regex _doubleRegex = new Regex(@"^(-?)((0|\d+)[.]?(\d+)?)?$"); // (\d) is same as ([0-9]). " *" indicates that it happens zero or more time.  [.]? indicates that "." can happen zero or one time. ^ should match starting. $ should also match ending

        static Incrementer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Incrementer), new FrameworkPropertyMetadata(typeof(Incrementer)));
        }

        public Incrementer() 
        {
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Increase, _increaseValue));
            CommandBindings.Add(new CommandBinding(AdditionalCommands.Decrease, _decreaseValue));
            this.Text = "0";
        }

        private string _getConvertedValue(bool increase = true)
        {
            var _current = this.Text;
            if (string.IsNullOrEmpty(_current))
            {
                _current = "0";
            }

            if (!_doubleRegex.IsMatch(_current))
            {
                _current = "0";
            }

            //Convert this to double and return.
            double _currentDbl  = double.Parse(_current);
            double _finalDbl = 0.0;

            //Increase or decrease
            if (increase)
            {
                _finalDbl =(_currentDbl + IncrementValue);
            }
            else
            {
                _finalDbl = (_currentDbl - IncrementValue);
            }

            //Check if we can go negative.
            if (!AllowNegative)
            {
                if (_finalDbl < 0)
                {
                    _finalDbl = 0;
                }
            }

            //Convert to string.
            string result = string.Empty;
            switch (NumberMode)
            {
                case NumericType.Integer:
                    result = Convert.ToInt32(_finalDbl).ToString();
                    break;
                case NumericType.Double:
                    result = _finalDbl.ToString();
                    break;
            }

            return result;
        }

        private void _decreaseValue(object sender, ExecutedRoutedEventArgs e)
        {
            //Whatever is the value, decrease it.
            var _res = _getConvertedValue(false);
            this.SetCurrentValue(TextProperty, _res);
        }

        private void _increaseValue(object sender, ExecutedRoutedEventArgs e)
        {
            //Whatever is the value, increase it.
            var _res = _getConvertedValue();
            this.SetCurrentValue(TextProperty, _res);
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(Incrementer), new FrameworkPropertyMetadata(ResourceHelper.cornerRadius));

        public int IncrementValue
        {
            get { return (int)GetValue(IncrementValueProperty); }
            set { SetValue(IncrementValueProperty, value); }
        }

        public static readonly DependencyProperty IncrementValueProperty =
            DependencyProperty.Register(nameof(IncrementValue), typeof(int), typeof(Incrementer), new PropertyMetadata(1));

        public bool AllowNegative
        {
            get { return (bool)GetValue(AllowNegativeProperty); }
            set { SetValue(AllowNegativeProperty, value); }
        }

        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register(nameof(AllowNegative), typeof(bool), typeof(Incrementer), new PropertyMetadata(false));

        public NumericType NumberMode
        {
            get { return (NumericType)GetValue(NumberModeProperty); }
            set { SetValue(NumberModeProperty, value); }
        }

        public static readonly DependencyProperty NumberModeProperty =
            DependencyProperty.Register(nameof(NumberMode), typeof(NumericType), typeof(Incrementer), new FrameworkPropertyMetadata(NumericType.Integer, propertyChangedCallback: NumberModePropertyChanged));

        private static void NumberModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Whenever the number mode is changed, we also need to change the attached property.
            var numtype = (NumericType)e.NewValue;
            InputConstraintType _constraint = InputConstraintType.Integer;
            switch (numtype)
            {
                case NumericType.Integer:
                    _constraint = InputConstraintType.Integer;
                    break;
                case NumericType.Double:
                    _constraint = InputConstraintType.Double;
                    break;
            }
            d.SetValue(InputAP.ConstraintProperty, _constraint); //We change the attached property value.
        }
    }
}
