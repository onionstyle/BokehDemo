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
using System.Windows.Input;
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
                _bokehData.MaskBrush = new SolidColorBrush() { Color = Color.FromArgb(255, 218, 81, 81) };
                _bokehData.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, _imageControl.Width, _imageControl.Height) };

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

        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="p">新位置</param>
        public void PositionChange(Point p)
        {
            _gradientBase.Translation(p);

             SetGradient();
        }

        //缩放
        public void ScaleChange(double scale)
        {
            double value = _bokehData.InsideValue * scale;
            if (value > 0 && value < 100)
                _bokehData.InsideValue = value;
        }

        public void SetPreAngel()
        {
            _preAngle = _bokehData.Angle;
        }

        public void RotationChange(PinchContactPoints current, PinchContactPoints origin)
        {
            if (_gradientBase is EllipseGradient) return;

            double angleDelta = GetAngle(current.PrimaryContact, current.SecondaryContact) - GetAngle(origin.PrimaryContact, origin.SecondaryContact);
          
            _bokehData.Angle = _preAngle + angleDelta;
        }
  
        public void SetGradient()
        {
            _gradientBase.SetGradient();
        }

        public void SetOpacity(double opacity)
        {
            _bokehData.Opacity = opacity;
        }

        /// <summary>
        /// 获取2点构成线段的角度
        /// </summary>
        private double GetAngle(Point primaryPoint, Point secondaryPoint)
        {
            Point directionVector = new Point(secondaryPoint.X - primaryPoint.X, secondaryPoint.Y - primaryPoint.Y);

            double angle = Math.Atan2(directionVector.Y, directionVector.X);

            if (angle < 0)
            {
                angle += 2 * Math.PI;
            }

            return angle * 180 / Math.PI;
        }

        private void BokehControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            double width;
            switch (e.PropertyName)
            {
                case "InsideValue":
                    width = _bokehData.InsideValue * DataManager.Instance.MaxWidth / 100;
                    if (width == _bokehData.InsideWidth) return;
                    _gradientBase.ScaleRestrict(width, BokehMode.InsideMode);
                    SetGradient();
                    break;
                case "OutsideValue":
                    width = _bokehData.OutsideValue * DataManager.Instance.MaxWidth / 100 + _bokehData.InsideWidth;
                    if (width == _bokehData.Width) return;
                    //改变外圈
                    _gradientBase.ScaleRestrict(width, BokehMode.OutsideMode);
                    SetGradient();
                    break;
            }
        }

        #region Properties

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
    /// 操作模式
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
