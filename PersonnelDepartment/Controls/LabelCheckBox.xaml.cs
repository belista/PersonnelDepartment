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
    /// Логика взаимодействия для LabelCheckBox.xaml
    /// </summary>
    public partial class LabelCheckBox : UserControl
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(LabelCheckBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(bool),
            typeof(LabelCheckBox),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty VisibilityBorderProperty = DependencyProperty.Register(
            "VisibilityBorder",
            typeof(Visibility),
            typeof(LabelCheckBox),
            new FrameworkPropertyMetadata
            {
                DefaultValue = Visibility.Hidden
            });

        public LabelCheckBox()
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

        public bool Value
        {
            get
            {
                return (bool)GetValue(ValueProperty);
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
