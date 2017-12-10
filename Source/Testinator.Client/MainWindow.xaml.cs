using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Testinator.Core;

namespace Testinator.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set DataContext to the WindowViewModel to allow binding in xaml
            DataContext = new WindowViewModel(this);


            var q = new MultipleCheckboxesQuestion();
            var q2 = new MultipleCheckboxesQuestion();
            var aa = new MultipleCheckboxesAnswer(new List<bool> { true });

            // We have a test
            var test = new Test();
            
            // Create builder from it
            var builder = TestBuilder.GetBuilderFrom(test);
            
            // Add some questions to the test
            builder.AddQuestion(q, aa);
            builder.AddQuestion(q2, aa);

            //builder.UpdateQuestion(q, q2);

            // Convert it back to a test
            test = builder.GetTest();
        }
    }
}

