using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for TimeSpanPicker.xaml
    /// </summary>
    public partial class TimeSpanPicker : UserControl
    {
        #region properties

        /// <summary>
        /// Contains the time value
        /// </summary>
        public TimeSpan Value
        {
            get => (TimeSpan)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// The hours
        /// </summary>
        public int Hours
        {
            get => (int)GetValue(HoursProperty);
            set => SetValue(HoursProperty, value);
        }

        /// <summary>
        /// The minutes
        /// </summary>
        public int Minutes
        {
            get => (int)GetValue(MinutesProperty);
            set => SetValue(MinutesProperty, value);
        }

        /// <summary>
        /// The seconds
        /// </summary>
        public int Seconds
        {
            get => (int)GetValue(SecondsProperty);
            set => SetValue(SecondsProperty, value);
        }

        #endregion

        #region dependency properties

        /// <summary>
        /// The TimeSpan value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(TimeSpan), typeof(TimeSpanPicker),
                new UIPropertyMetadata(new TimeSpan(0), OnValueChanged));

        /// <summary>
        /// The Hours property
        /// </summary>
        public static readonly DependencyProperty HoursProperty =
            DependencyProperty.Register(nameof(Hours), typeof(int), typeof(TimeSpanPicker),
                new UIPropertyMetadata(0, OnTimeChanged));

        /// <summary>
        /// The minutes property
        /// </summary>
        public static readonly DependencyProperty MinutesProperty =
            DependencyProperty.Register(nameof(Minutes), typeof(int), typeof(TimeSpanPicker),
                new UIPropertyMetadata(0, OnTimeChanged));

        /// <summary>
        /// The seconds property
        /// </summary>
        public static readonly DependencyProperty SecondsProperty =
            DependencyProperty.Register(nameof(Seconds), typeof(int), typeof(TimeSpanPicker),
                new UIPropertyMetadata(0, OnTimeChanged));

        #endregion

        #region properties changed callback

        /// <summary>
        /// Change the time value with the minutes, seconds, hours properties
        /// </summary>
        /// <param name="d">TimeSpanPicker</param>
        /// <param name="e">The event</param>
        static void OnTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => d.SetValue(ValueProperty,
                new TimeSpan(
                    (int)d.GetValue(HoursProperty),
                    (int)d.GetValue(MinutesProperty),
                    (int)d.GetValue(SecondsProperty))
                );

        /// <summary>
        /// Change hours, minutes, seconds with the TimeSpan value in the event
        /// </summary>
        /// <param name="d">TimeSpanPicker</param>
        /// <param name="e">The event</param>
        static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TimeSpan time = (TimeSpan)e.NewValue;
            d.SetValue(HoursProperty, time.Hours);
            d.SetValue(MinutesProperty, time.Minutes);
            d.SetValue(SecondsProperty, time.Seconds);
        }

        #endregion

        #region constructor

        /// <summary>
        /// Construct a TimeSpanPicker and set the DataContext to this
        /// </summary>
        public TimeSpanPicker()
        {
            InitializeComponent();
            (Content as FrameworkElement).DataContext = this;
        }

        #endregion
    }
}
