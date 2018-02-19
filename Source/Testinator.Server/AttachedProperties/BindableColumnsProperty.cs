using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Testinator.Server.Core;
using Testinator.UICore;

namespace Testinator.Server
{
    /// <summary>
    /// Binds data to a <see cref="DataGrid"/> dynamically
    /// </summary>
    public class BindableColumnsProperty : BaseAttachedProperty<BindableColumnsProperty, List<QuestionsViewItemViewModel>>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // If we don't have a control, return
            if (!(sender is DataGrid datagrid))
                return;


            // TODO: another way to do this or comment this later

            datagrid.Columns.Clear();

            var NameColumn = new DataGridTextColumn()
            {
                Header = "Imię",
                Binding = new Binding("Name"),
            };

            var SurnameColumn = new DataGridTextColumn()
            {
                Header = "Nazwisko",
                Binding = new Binding("Surname"),
            };

            datagrid.Columns.Add(NameColumn);
            datagrid.Columns.Add(SurnameColumn);

            for (var i = 0; i < (e.NewValue as List<QuestionsViewItemViewModel>)[0].QuestionsPoints.Count; i++)
            {
                var column = new DataGridTextColumn()
                {
                    Header = (i + 1).ToString(),
                    Binding = new Binding("QuestionsPoints[" + i + "]"),
                };
                datagrid.Columns.Add(column);
            }

            foreach(var item in e.NewValue as List<QuestionsViewItemViewModel>)
            {
                datagrid.Items.Add(item);
            }

        }
    }
}
