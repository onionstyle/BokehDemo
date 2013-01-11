using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace BokehDemo.Models
{
    public class BokehData : INotifyPropertyChanged
    {
        public BokehData()
        {
            _ellipseData = new EllipseData();
            _linerData = new LinerData();
            SetData("EllipseGradient");
        }

        #region 公共参数
        private double _opacity;
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                RaisePropertyChanged("Opacity");
            }
        }

        private Brush _maskBrush;
        public Brush MaskBrush
        {
            get { return _maskBrush; }
            set
            {
                _maskBrush = value;
                RaisePropertyChanged("MaskBrush");
            }
        }

        public double Rate
        {
            get { if (Width != 0) return _commonData.InsideWidth / _commonData.Width; else return 1; }
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
        #endregion

        #region Ellipse
        //渐变x半径占图宽的比例
        public double RadiusX
        {
            get { return _ellipseData.RadiusX; }
            set
            {
                _ellipseData.RadiusX = value;
                RaisePropertyChanged("RadiusX");
            }
        }
        public double RadiusY
        {
            get { return _ellipseData.RadiusY; }
            set
            {
                _ellipseData.RadiusY = value;
                RaisePropertyChanged("RadiusY");
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
        #endregion

        #region Liner
        public Point EndPoint
        {
            get { return _linerData.EndPoint; }
            set
            {
                _linerData.EndPoint = value;
                RaisePropertyChanged("EndPoint");
                RaisePropertyChanged("BackEndPoint");
            }
        }
        /// <summary>
        /// 方向线性渐变的终点
        /// </summary>
        public Point BackEndPoint
        {
            get { return new Point(StarPoint.X + StarPoint.X - EndPoint.X, StarPoint.Y + StarPoint.Y - EndPoint.Y); }
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
        #endregion

        #region Common
        public Point StarPoint
        {
            get { return _commonData.StarPoint; }
            set
            {
                _commonData.StarPoint = value;
                RaisePropertyChanged("StarPoint");
            }
        }

        /// <summary>
        /// 大小拉条
        /// </summary>
        public double InsideValue
        {
            get { return _commonData.InsideValue; }
            set
            {
                _commonData.InsideValue = value;
                RaisePropertyChanged("InsideValue");
            }
        }

        /// <summary>
        /// 范围拉条
        /// </summary>
        public double OutsideValue
        {
            get { return _commonData.OutsideValue; }
            set
            {
                _commonData.OutsideValue = value;
                RaisePropertyChanged("OutsideValue");
            }
        }

        /// <summary>
        /// 外宽
        /// </summary>
        public double Width
        {
            get { return _commonData.Width; }
            set
            {
                _commonData.Width = value;
                RaisePropertyChanged("Width");
                RaisePropertyChanged("Rate");
            }
        }

        public double Height
        {
            get { return _commonData.Height; }
            set
            {
                _commonData.Height = value;
                RaisePropertyChanged("Height");
            }
        }

        /// <summary>
        /// Canvas显示区域在图片上的位置
        /// </summary>
        public Thickness Margin
        {
            get { return _commonData.Margin; }
            set
            {
                _commonData.Margin = value;
                RaisePropertyChanged("Margin");
            }
        }

        public double InsideWidth
        {
            get { return _commonData.InsideWidth; }
            set
            {
                _commonData.InsideWidth = value;
                RaisePropertyChanged("InsideWidth");
                RaisePropertyChanged("Rate");
            }
        }
        #endregion

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

        public void SetData(string typename)
        {
            switch (typename)
            {
                case "EllipseGradient":
                    _commonData = _ellipseData ; 
                    EllipseVisibility = Visibility.Visible;
                    LineVisibility = Visibility.Collapsed;
                    break;
                case "LinerGradient":
                    _commonData = _linerData;
                    EllipseVisibility = Visibility.Collapsed;
                    LineVisibility = Visibility.Visible;
                    break;
            }
            ObservedChanged();
        }

         void ObservedChanged()
        {
            RaisePropertyChanged("Height");
            RaisePropertyChanged("Width");
            RaisePropertyChanged("Rate");
            RaisePropertyChanged("InsideWidth");
            RaisePropertyChanged("Margin");
            RaisePropertyChanged("InsideValue");
            RaisePropertyChanged("OutsideValue");
        }

        private CommonData _commonData;
        private EllipseData _ellipseData;
        private LinerData _linerData;
    }
}
