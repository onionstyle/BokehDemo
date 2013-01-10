using System.Windows;
namespace BokehDemo.Models
{
    class CommonData
    {
        /// <summary>
        /// Canvas显示区域在图片上的位置
        /// </summary>
        protected Thickness _margin;
        protected double _height;
        /// <summary>
        /// 外宽
        /// </summary>
        protected double _width;
        protected double _insideValue;
        protected double _outsideValue;
        protected Point _starPoint;
        protected double _insideWidth;

        public Point StarPoint
        {
            get { return _starPoint; }
            set
            {
                _starPoint = value;
            }
        }

        public double InsideValue
        {
            get { return _insideValue; }
            set
            {
                _insideValue = value;
            }
        }

        public double OutsideValue
        {
            get { return _outsideValue; }
            set
            {
                _outsideValue = value;
            }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public double Height
        {
            get { return _height; }
            set{_height = value;  }
        }       

        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
            }
        }

        public double InsideWidth
        {
            get { return _insideWidth; }
            set
            {
                _insideWidth = value;
            }
        }
    }
}
