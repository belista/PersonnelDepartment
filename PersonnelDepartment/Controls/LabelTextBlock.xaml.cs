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
    /// Логика взаимодействия для LabelTextBlock.xaml
    /// </summary>
    public partial class LabelTextBlock : UserControl
    {
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register(
            "Key",
            typeof(string),
            typeof(LabelTextBlock),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(double),
            typeof(LabelTextBlock),
            new PropertyMetadata(default(double)));

        public static readonly DependencyProperty VisibilityBorderProperty = DependencyProperty.Register(
            "VisibilityBorder",
            typeof(Visibility),
            typeof(LabelTextBlock),
            new FrameworkPropertyMetadata
            {
                DefaultValue = Visibility.Hidden
            });

        public LabelTextBlock()
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
        public double Value
        {
            get
            {
                return (double)GetValue(ValueProperty);
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
                return (Visibility)GetValue(VisibilityBorderProperty);
            }
            set
            {
                SetValue(VisibilityBorderProperty, value);
            }
        }
    }
}
