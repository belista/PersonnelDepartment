using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace PersonnelDepartment.Core.Services.Printing
{
    public class StoreDataSetPaginator : DocumentPaginator
    {
        private DataTable _table;
        private Typeface _typeFace;
        private double _fontSize;
        private double _margin;
        private Size _size;

        private int _pageCount;
        private int _rowsPerPage;
        private bool _typePrinting;

        public StoreDataSetPaginator(DataTable dt, Typeface typeface, double fontSize, double margin, Size pageSize, bool typePrinting)
        {
            this._table = dt;
            this._typeFace = typeface;
            this._fontSize = fontSize;
            this._margin = margin;
            this._size = pageSize;
            this._typePrinting = typePrinting;
            PaginateData();
        }
        public override Size PageSize
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                PaginateData();
            }
        }

        public void PaginateData()
        {
            var text = GetFormattedText("A");

            _rowsPerPage = (int)((_size.Height - _margin * 2) / text.Height);

            _rowsPerPage -= 1;

            _pageCount = (int)Math.Ceiling((double)_table.Rows.Count / _rowsPerPage);
        }

        private FormattedText GetFormattedText(string text)
        {
            return GetFormattedText(text, _typeFace);
        }

        private FormattedText GetFormattedText(string text, Typeface typeface)
        {
            return new FormattedText(
                 text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                      typeface, _fontSize, Brushes.Black);
        }

        public override bool IsPageCountValid
        {
            get
            {
                return true;
            }
        }

        public override int PageCount
        {
            get
            {
                return _pageCount;
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return null;
            }
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            if (_typePrinting)
            {
                FormattedText text = GetFormattedText("A");

                double col1_X = _margin;
                double col2_X = col1_X + text.Width * 5;
                double col3_X = col1_X + text.Width * 30;
                double col4_X = col1_X + text.Width * 50;

                int minRow = pageNumber * _rowsPerPage;
                int maxRow = minRow + _rowsPerPage;

                // Создать визуальный элемент для страницы
                DrawingVisual visual = new DrawingVisual();

                // Установить позицию в верхний левый угол печатаемой области
                Point point = new Point(_margin, _margin);

                using (DrawingContext dc = visual.RenderOpen())
                {
                    // Нарисовать заголовки столбцов
                    Typeface columnHeaderTypeface = new Typeface(_typeFace.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                    point.X = col1_X;
                    text = GetFormattedText("Id", columnHeaderTypeface);
                    dc.DrawText(text, point);
                    text = GetFormattedText("Фамилия", columnHeaderTypeface);
                    point.X = col2_X;
                    dc.DrawText(text, point);
                    text = GetFormattedText("Имя", columnHeaderTypeface);
                    point.X = col3_X;
                    dc.DrawText(text, point);
                    text = GetFormattedText("Отчество", columnHeaderTypeface);
                    point.X = col4_X;
                    dc.DrawText(text, point);

                    // Нарисовать линию подчеркивания
                    dc.DrawLine(new Pen(Brushes.Black, 2),
                        new Point(_margin, _margin + text.Height),
                        new Point(_size.Width - _margin, _margin + text.Height));

                    point.Y += text.Height;

                    // Нарисовать значения столбцов
                    for (int i = minRow; i < maxRow; i++)
                    {
                        // Проверить конец последней (частично заполненной) страницы
                        if (i > (_table.Rows.Count - 1)) break;

                        point.X = col1_X;
                        text = GetFormattedText(_table.Rows[i]["Id"].ToString());
                        dc.DrawText(text, point);

                        // Добавить второй столбец                    
                        text = GetFormattedText(_table.Rows[i]["FirstSurname"].ToString());
                        point.X = col2_X;
                        dc.DrawText(text, point);

                        text = GetFormattedText(_table.Rows[i]["Name"].ToString());
                        point.X = col3_X;
                        dc.DrawText(text, point);


                        text = GetFormattedText(_table.Rows[i]["Patronymic"].ToString());
                        point.X = col4_X;
                        dc.DrawText(text, point);
                        point.Y += text.Height;
                    }
                }
                return new DocumentPage(visual, _size, new Rect(_size), new Rect(_size));
            }
            else
            {
                FormattedText text = GetFormattedText("A");

                double col1_X = _margin;

                string textRow;

                int minRow = pageNumber * _rowsPerPage;
                int maxRow = minRow + _rowsPerPage;

                // Создать визуальный элемент для страницы
                DrawingVisual visual = new DrawingVisual();

                // Установить позицию в верхний левый угол печатаемой области
                Point point = new Point(_margin, _margin);

                using (DrawingContext dc = visual.RenderOpen())
                {
                    // Нарисовать заголовки столбцов
                    Typeface columnHeaderTypeface = new Typeface(_typeFace.FontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
                    point.X = col1_X;
                    text = GetFormattedText("Информация о работнике", columnHeaderTypeface);
                    dc.DrawText(text, point);
                    dc.DrawText(text, point);

                    // Нарисовать линию подчеркивания
                    dc.DrawLine(new Pen(Brushes.Black, 2),
                        new Point(_margin, _margin + text.Height),
                        new Point(_size.Width - _margin, _margin + text.Height));

                    point.Y += text.Height;

                    // Нарисовать значения столбцов
                    for (int i = minRow; i < maxRow; i++)
                    {
                        // Проверить конец последней (частично заполненной) страницы
                        if (i > (_table.Rows.Count - 1)) break;

                        point.X = col1_X;
                        textRow = _table.Rows[i]["Id"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Id - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        // Добавить вторую строку     
                        textRow = _table.Rows[i]["FirstSurname"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Первая фамилия - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["SecondSurname"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Вторая фамилия - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["ThirdSurname"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Третья фамилия - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["Name"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Имя - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["Patronymic"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Отчество - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["PassportInfo"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Паспортные данные - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["Registration"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Прописка - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["RegistrationNumber"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Инн - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["Phone"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Телефон - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["FirstPosition"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Первая должность - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["SecondPosition"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Вторая должность - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["ThirdPosition"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Третья должность - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["FirstOrder"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Первый приказ - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["SecondOrder"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Второй приказ - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["Additionally"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Дополнительно - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        if (DateTime.TryParse(_table.Rows[i]["DateOfDismissal"].ToString(), out DateTime dod))
                        {
                            textRow = dod.ToShortDateString();

                            text = GetFormattedText("Дата увольнения - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }
                        
                        textRow = Convert.ToDateTime(_table.Rows[i]["EmploymentDate"]).ToShortDateString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Дата приема на работу - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = Convert.ToDateTime(_table.Rows[i]["DateOfBirth"]).ToShortDateString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Дата рождения - " + textRow);
                            dc.DrawText(text, point);
                            point.Y += text.Height;
                        }

                        textRow = _table.Rows[i]["WorkDays"].ToString();
                        if (!String.IsNullOrEmpty(textRow))
                        {
                            text = GetFormattedText("Дни работы - " + textRow);
                            dc.DrawText(text, point);
                        }
                    }
                }
                return new DocumentPage(visual, _size, new Rect(_size), new Rect(_size));
            }
            
        }
    }
}
