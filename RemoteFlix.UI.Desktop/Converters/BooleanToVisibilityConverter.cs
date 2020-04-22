using System;
using System.Windows;

namespace RemoteFlix.UI.Desktop.Converters
{
    public class BooleanToVisibilityConverter : BooleanConditionConverter<Visibility>
    {
        protected override Func<object, bool> EvaluationFunction => (value) => (value as bool?) == true;

        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
