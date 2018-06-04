namespace scrcpy.VisualStudio.UI
{
    using scrcpy.VisualStudio.Android;
    using scrcpy.VisualStudio.ViewModel;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for ScrcpyToolWindowControl.
    /// </summary>
    public partial class ScrcpyToolWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrcpyToolWindowControl"/> class.
        /// </summary>
        public ScrcpyToolWindowControl()
        {
            this.InitializeComponent();
            ScrcpyViewModel vm = new ScrcpyViewModel();
            vm.ScrcpyStartRequested += (s, e) =>
            {

            };
            DataContext = vm;
        }
    }
}