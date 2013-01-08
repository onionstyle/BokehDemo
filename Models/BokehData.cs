using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace BokehDemo.Models
{
    public class BokehData : INotifyPropertyChanged
    {

        private double _width;
        /// <summary>
        /// 外宽
        /// </summary>
        public double Width
        {
            get { return _width; }
            set
            {
                _width = value;
                RaisePropertyChanged("Width");
            }
        }

        private double _height;
        public double Height
        {
            get { return _height; }
            set
            {
                _height = value;
                RaisePropertyChanged("Height");
            }
        }

        private double _angle;
        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                RaisePropertyChanged("Angle");
            }
        }

        private double _centerX;
        public double CenterX
        {
            get { return _width / 2; }
            set
            {
                _centerX = value;
                RaisePropertyChanged("CenterX");
            }
        }
        private double _centerY;
        public double CenterY
        {
            get { return _height / 2; }
            set
            {
                _centerY = value;
                RaisePropertyChanged("CenterY");
            }
        }

        /// <summary>
        /// Canvas显示区域在图片上的位置
        /// </summary>
        private Thickness _margin;
        public Thickness Margin
        {
            get { return _margin; }
            set
            {
                _margin = value;
                RaisePropertyChanged("Margin");
            }
        }

        private Visibility _ellipseVisibility;
        public Visibility EllipseVisibility
        {
            get { return _ellipseVisibility; }
            set
            {
                _ellipseVisibility = value;
                RaisePropertyChanged("EllipseVisibility");
            }
        }

        private double _insideWidth;
        public double InsideWidth
        {
            get { return _insideWidth; }
            set
            {
                _insideWidth = value;
                RaisePropertyChanged("InsideWidth");
            }
        }

        private Visibility _lineVisibility;
        public Visibility LineVisibility
        {
            get { return _lineVisibility; }
            set
            {
                _lineVisibility = value;
                RaisePropertyChanged("LineVisibility");
            }
        }

        /// <summary>
        /// 是否显示Canvas
        /// </summary>
        private Visibility _visibility;
        public Visibility Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                RaisePropertyChanged("Visibility");
            }
        }

        private RectangleGeometry _clip;
        public RectangleGeometry Clip
        {
            get { return _clip; }
            set
            {
                _clip = value;
                RaisePropertyChanged("Clip");
            }
        }

        /// <summary>
        /// 响应值变化事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
