using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace RemoteFlix.UI.Desktop.Converters
{
    public abstract class BooleanConditionConverter<T> : IValueConverter
    {
        protected abstract Func<object, bool> EvaluationFunction { get; }

        public BooleanConditionConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public T TrueValue { get; set; }
        public T FalseValue { get; set; }

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EvaluationFunction(value) ? TrueValue : FalseValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, TrueValue);
        }
    }
}
