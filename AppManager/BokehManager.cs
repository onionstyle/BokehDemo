using System;
using Microsoft.Phone.Tasks;
using BokehDemo.Models;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Diagnostics;
namespace BokehDemo.AppManager
{
    class BokehManager
    {
        public BokehManager()
        {
            _photoChooserTask = new PhotoChooserTask();
            _photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
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
            _imageControl.ClipMargin = new Thickness(0, 0, 0, 0);
            _imageControl.PropertyChanged += ImageControl_PropertyChanged;
        }

        public void OpenImage()
        {
            _photoChooserTask.Show();
        }
        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                DataManager.Instance.SetMainData(new WriteableBitmap(bmp));
                ShowImage();
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
                    _gradientBase = _linerGradient ?? (_linerGradient = new LinerGradient());
                    break;

                case "LinerGradient":
                    _gradientBase = _ellipseGradient;
                    break;
                default:
                    _gradientBase = _ellipseGradient ?? (_ellipseGradient = new EllipseGradient());
                    break;
            }
            _gradientBase.Initialize(_imageControl, _bokehData);
        }

        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="p">left-top改变值</param>
        public void PositionChange(Point p)
        {
            if (_bokehMode == BokehMode.None)
                return;
            //中心点距离边距的大小
            p.X = _bokehData.Margin.Left + p.X + _bokehData.Width / 2;
            p.Y = _bokehData.Margin.Top + p.Y + _bokehData.Height / 2;

            if (p.X < 0 || p.X > _imageControl.Width)
            {
                p.X = _bokehData.Margin.Left + _bokehData.Width / 2;
            }

            if (p.Y < 0 || p.Y > _imageControl.Height)
            {
                p.Y = _bokehData.Margin.Top + _bokehData.Height / 2;
            }
            _bokehData.Margin = new Thickness(p.X - _bokehData.Width / 2, p.Y - _bokehData.Height / 2, 0, 0);
         
        }
        public void SetPreAngle()
        {
            _preAngle = 0;
        }
        /// <summary>
        /// 拉动旋转变化,需要先设置变化初始角度SetPreAngle()
        /// </summary>
        public void RotationChange(Point currPoint,Point pinPoint,Point center)
        {
            if (_preAngle == 0)
            {
                _preAngle = _gradientBase.Radians(currPoint, pinPoint);
                return;
            }

            Debug.WriteLine("(" + currPoint.X + "," + currPoint.Y + ")" + "(" + pinPoint.X + "," + pinPoint.Y + ")");
            double curAngle = _gradientBase.Radians(currPoint, pinPoint);
            double deltaAngle = 180 * (curAngle - _preAngle) / Math.PI;

            _bokehData.CenterX = _bokehData.Margin.Left + center.X;
            _bokehData.CenterY = _bokehData.Margin.Top + center.Y;
            _bokehData.Angle +=deltaAngle;
            _preAngle = curAngle;
        }

        public void SetMode(BokehMode bokehMode)
        {
            if (_bokehMode != bokehMode)
            {
                _bokehMode = bokehMode;
                _gradientBase.Mode = bokehMode;
            }
        }

        public void InsideValueChanged(double value)
        {
            double width = value*DataManager.Instance.MaxWidth/100;
            if (width == _bokehData.InsideWidth) return;
            BokehMode bokehMode = BokehMode.InsideMode;
            _gradientBase.ScaleRestrict(width, bokehMode);
        }
        public void OutsideValueChanged(double value)
        {
            double width = value * DataManager.Instance.MaxWidth / 100 + _bokehData.InsideWidth;
            if (width == _bokehData.Width) return;
            //改变外圈
            BokehMode bokehMode = BokehMode.OutsideMode;
            _gradientBase.ScaleRestrict(width, bokehMode);
        }

        private void ImageControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Width" || e.PropertyName == "Height")
            {
                _bokehData.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, _imageControl.Width, _imageControl.Height) };
                if (_gradientBase != null)
                {
                    Restrict();
                    //设置ImageGrid边距使其居中
                    _imageControl.Margin = new Thickness((_imageControl.ClipWidth - _imageControl.Width) / 2, (_imageControl.ClipHeight - _imageControl.Height) / 2, 0, 0);

                    _gradientBase.ReSet();
                }
            }
        }


        #region Properties
        BokehMode _bokehMode;
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
        protected ImageControlData _imageControl = null;
        public ImageControlData ImageControlBindingData
        {
            get { return _imageControl ?? (_imageControl = new ImageControlData()); }
        }
        /// <summary>
        /// 虚化显示数据绑定
        /// </summary>
        protected BokehData _bokehData = null;
        public BokehData BokehBindingData
        {
            get { return _bokehData ?? (_bokehData = new BokehData()); }
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

        /// <summary>
        /// 矩形旋转
        /// </summary>
        RotaMode,
    }
}
