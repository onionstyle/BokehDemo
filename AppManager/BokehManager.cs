using System;
using Microsoft.Phone.Tasks;
using BokehDemo.Models;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using Windows.Storage.Streams;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace BokehDemo.AppManager
{
    class BokehManager
    {
        public BokehManager()
        {
            _photoChooserTask = new PhotoChooserTask();
            _photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);

            _bokehData = new BokehData();
            _bokehData.PropertyChanged += BokehControl_PropertyChanged;
        }

        /// <summary>
        /// 设置可显示的区域
        /// </summary>
        public void SetShowArea()
        {
            //裁剪显示区域，隐藏超出画布的部分
            _imageControl.ClipWidth = DataManager.Instance.MaxWidth;
            _imageControl.ClipHeight = DataManager.Instance.MaxHeight;
            _imageControl.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, _imageControl.ClipWidth, _imageControl.ClipHeight) };
        }

        public void OpenImage()
        {
            _photoChooserTask.Show();
        }

        public void SaveImage()
        {
            _gradientBase.SaveGradient();
        }

        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                DataManager.Instance.SetMainData(new WriteableBitmap(bmp));
                ShowImage();
                //蒙版
                _bokehData.MaskBrush = new SolidColorBrush() {Color = Color.FromArgb(255, 218, 81, 81) };

                ModeChanger();
            }
        }

        //显示图片
        public void ShowImage()
        {
            _imageControl.Width = DataManager.Instance.ThumbData.PixelWidth;
            _imageControl.Height = DataManager.Instance.ThumbData.PixelHeight;
            _imageControl.Source = DataManager.Instance.ThumbData;
            //超最大宽高整张图按比例缩放
            Restrict();
            //设置ImageGrid边距使其居中
            _imageControl.Margin = new Thickness((_imageControl.ClipWidth - _imageControl.Width) / 2, (_imageControl.ClipHeight - _imageControl.Height) / 2, 0, 0);
        }

        public void Restrict(int maxWidth = 0, int maxHeight = 0)
        {
            //长宽比例
            double rate = _imageControl.Height / (float)_imageControl.Width;
            if (maxWidth == 0)
            {
                maxWidth = DataManager.Instance.MaxWidth;

            }
            if (maxHeight == 0)
            {
                maxHeight = DataManager.Instance.MaxHeight;
            }

            int minWidth = DataManager.MinWidth;
            int minHeight = DataManager.MinHeight;

            //在宽度范围
            if (_imageControl.Width > maxWidth)
            {
                _imageControl.Width = maxWidth;
                _imageControl.Height = Convert.ToInt32(_imageControl.Width * rate);
            }
            else
            {
                if (_imageControl.Width < minWidth)
                {
                    _imageControl.Width = minWidth;
                    _imageControl.Height = Convert.ToInt32(_imageControl.Width * rate);
                }
            }
            //判断高度范围
            if (_imageControl.Height > maxHeight)
            {
                _imageControl.Height = maxHeight;
                _imageControl.Width = Convert.ToInt32(_imageControl.Height / rate);
            }
            else
            {
                if (_imageControl.Height < minHeight)
                {
                    _imageControl.Height = minHeight;
                    _imageControl.Width = Convert.ToInt32(_imageControl.Height / rate);
                }
            }

            if (_imageControl.Width < minWidth)
            {
                _imageControl.Width = minWidth;
                _imageControl.Height = Convert.ToInt32(_imageControl.Width * rate);
            }
        }

        public void ModeChanger()
        {
            string typename = _gradientBase == null ? null : _gradientBase.GetType().Name;
            switch (typename)
            {
                case "EllipseGradient":
                    if (_linerGradient == null)
                    {
                        _linerGradient = new LinerGradient(_imageControl, _bokehData);
                        _gradientBase = _linerGradient;
                        _bokehData.SetData(_gradientBase.GetType().Name);
                        _linerGradient.Initialize(_imageControl.Height * 2 / 3, _imageControl.Height);
                        return;
                    }
                    _gradientBase = _linerGradient;
                    break;
                case "LinerGradient":
                    _gradientBase = _ellipseGradient;
                    break;
                default:
                    if (_ellipseGradient == null)
                    {
                        _ellipseGradient = new EllipseGradient(_imageControl, _bokehData);
                        _gradientBase = _ellipseGradient;
                        _ellipseGradient.Initialize(_imageControl.Width / 3, _imageControl.Width);
                    }
                    break;
            }
            _bokehData.SetData(_gradientBase.GetType().Name);
            SetGradient();
        }

        public void SetPrePoint(Point p)
        {
           _prePosition = p;
        }

        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="p">新位置</param>
        public void PositionChange(Point p)
        {
            //中心点距离边距的大小
            Point discenter = new Point(_bokehData.Margin.Left + _bokehData.Width / 2 + p.X - _prePosition.X, _bokehData.Margin.Top + _bokehData.Height / 2 + p.Y - _prePosition.Y);
            _prePosition = p;

            _gradientBase.Translation(discenter);

             SetGradient();
        }

        //缩放
        public void ScaleChange(double scale)
        {
            double value;
            switch (_bokehMode)
            {
                case BokehMode.InsideMode:
                    value = _bokehData.InsideValue * scale;
                    if (value > 0 && value < 100)
                        _bokehData.InsideValue = value;
                    break;
                case BokehMode.OutsideMode:
                    value = _bokehData.OutsideValue * scale;
                    if (value > 0 && value < 100)
                        _bokehData.OutsideValue = value;
                    break;
            }
        }

        public void RotationChange(Point currPoint, Point pinPoint, Point center)
        {
            if (_gradientBase is EllipseGradient) return;
            if (_bokehMode==BokehMode.None||_preAngle == 0)
            {
                _preAngle = _gradientBase.Radians(currPoint, pinPoint);
                return;
            }
            double curAngle = _gradientBase.Radians(currPoint, pinPoint);
            double deltaAngle = 180 * (curAngle - _preAngle) / Math.PI;
            _preAngle = 0;
            if (deltaAngle > 30 || deltaAngle < -30) return;
            _bokehData.Angle += deltaAngle;
        }

        public void SetGradient()
        {
            _gradientBase.SetGradient();
        }

        public void SetMode(BokehMode bokehMode)
        {
            if (_bokehMode != bokehMode)
            {
                _bokehMode = bokehMode;
            }
        }

        public void SetOpacity(double opacity)
        {
            _bokehData.Opacity = opacity;
        }

        private void BokehControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            double width;
            BokehMode bokehMode;
            switch (e.PropertyName)
            {
                case "InsideValue":
                    width = _bokehData.InsideValue * DataManager.Instance.MaxWidth / 100;
                    if (width == _bokehData.InsideWidth) return;
                    bokehMode = BokehMode.InsideMode;
                    _gradientBase.ScaleRestrict(width, bokehMode);
                    SetGradient();
                    break;
                case "OutsideValue":
                    width = _bokehData.OutsideValue * DataManager.Instance.MaxWidth / 100 + _bokehData.InsideWidth;
                    if (width == _bokehData.Width) return;
                    //改变外圈
                    bokehMode = BokehMode.OutsideMode;
                    _gradientBase.ScaleRestrict(width, bokehMode);
                    SetGradient();
                    break;
            }
        }

        #region Properties

        BokehMode _bokehMode;
        /// <summary>
        /// 起始位置
        /// </summary>
        private Point _prePosition;
        /// <summary>
        /// 起始旋转角度
        /// </summary>
        private double _preAngle;

        private LinerGradient _linerGradient;
        private EllipseGradient _ellipseGradient;
        private GradientBase _gradientBase;

        private PhotoChooserTask _photoChooserTask;
        /// <summary>
        /// 图片数据绑定
        /// </summary>
        protected ImageControlData _imageControl;
        public ImageControlData ImageControlBindingData
        {
            get { return _imageControl ?? (_imageControl = new ImageControlData()); }
        }
        /// <summary>
        /// 虚化显示数据绑定
        /// </summary>
        protected BokehData _bokehData;
        public BokehData BokehBindingData
        {
            get { return _bokehData; }
        }
        #endregion
    }


    /// <summary>
    /// 鼠标操作模式
    /// </summary>
    public enum BokehMode
    {
        None,

        /// <summary>
        /// 内部的缩放
        /// </summary>
        InsideMode,

        /// <summary>
        /// 外部的缩放
        /// </summary>
        OutsideMode,
    }
}
