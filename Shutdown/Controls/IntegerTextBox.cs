using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shutdown.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Shutdown.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Shutdown.Controls;assembly=Shutdown.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:IntegerTextBox/>
    ///
    /// </summary>
    public class IntegerTextBox : TextBox
    {
        public int Minimum
        {
            get => (int) GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public int Maximum
        {
            get => (int)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty
            = DependencyProperty.Register(nameof(Minimum), typeof(int), typeof(IntegerTextBox),
                new PropertyMetadata(int.MinValue));

        public static readonly DependencyProperty MaximumProperty
            = DependencyProperty.Register(nameof(Maximum), typeof(int), typeof(IntegerTextBox),
                 new PropertyMetadata(int.MaxValue));

        public IntegerTextBox() : base()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(IntegerTextBox), new FrameworkPropertyMetadata(typeof(IntegerTextBox)));
            PreviewTextInput += IntegerTextBox_PreviewTextInput;
            DataObject.AddPastingHandler(this,TextBoxPasting);
        }

        bool IsTextAllowed(string text)
        {
            if (int.TryParse(Text + text, out int number))
            {
                return number <= Maximum && number >= Minimum;
            }
            return false;
        }

        void TextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        void IntegerTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
            => e.Handled = !IsTextAllowed(e.Text);
    }
}
