using System;
using System.Windows;

namespace RemoteFlix.UI.Desktop.Converters
{
    public class NullToVisibilityConverter : BooleanConditionConverter<Visibility>
    {
        protected override Func<object, bool> EvaluationFunction => (value) => value == null;

        public NullToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
