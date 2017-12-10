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

namespace PersonnelDepartment.Controls
{
    /// <summary>
    /// Логика взаимодействия для LabelTextBox.xaml
    /// </summary>
    public partial class LabelTextBox : UserControl
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(LabelTextBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(object),
            typeof(LabelTextBox),
            new FrameworkPropertyMetadata
            {
                DefaultValue = null,
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        public static readonly DependencyProperty VisibilityBorderProperty = DependencyProperty.Register(
            "VisibilityBorder",
            typeof(Visibility),
            typeof(LabelTextBox),
            new FrameworkPropertyMetadata
            {
                DefaultValue = Visibility.Hidden
            });

        public LabelTextBox()
        {
            InitializeComponent();
        }
        
        public string Key
        {
            get
            {
                return (string)GetValue(KeyProperty);
            }
            set
            {
                SetValue(KeyProperty, value);
            }
        }

        public object Value
        {
            get
            {
                return GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public Visibility VisibilityBorder
        {
            get
            {
                return (Visibility)GetValue(VisibilityProperty);
            }
            set
            {
                SetValue(VisibilityBorderProperty, value);
            }
        }
    }
}
