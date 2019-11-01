using RemoteFlix.UI.Desktop.Wrappers;
using System.Windows;
using System.Windows.Controls;

namespace RemoteFlix.UI.Desktop.Controls
{
    // This entire class was taken from here: https://stackoverflow.com/a/45627524/5686352
    // Full credits to torvin (https://stackoverflow.com/users/332528/)
    public class SelectableTextBlock : TextBlock
    {
        static SelectableTextBlock()
        {
            FocusableProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata(true));
            TextEditorWrapper.RegisterCommandHandlers(typeof(SelectableTextBlock), true, true, true);

            // remove the focus rectangle around the control
            FocusVisualStyleProperty.OverrideMetadata(typeof(SelectableTextBlock), new FrameworkPropertyMetadata((object)null));
        }

        private readonly TextEditorWrapper _editor;

        public SelectableTextBlock()
        {
            _editor = TextEditorWrapper.CreateFor(this);
        }
    }
}
