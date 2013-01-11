using BokehDemo.Models;
using System;
using System.Windows;
using System.Windows.Media;
namespace BokehDemo.AppManager
{
    /// <summary>
    /// 虚化基类
    /// </summary>
    class GradientBase
    {
        public virtual void Initialize(double insideWidth, double outsideWidth)
        {
        }

        /// <summary>
        ///  放大缩小时限制图形最大最小宽高,并调整位置
        /// </summary>
        /// <param name="width">新宽高</param>
        public virtual void ScaleRestrict(double width, BokehMode bokeh)
        {
        }

        public virtual void SetGradient()
        {
            _centerX = _bokehData.Margin.Left + _bokehData.Width / 2;
            _centerY = _bokehData.Margin.Top + _bokehData.Height / 2;
        }

        public virtual void SaveGradient()
        {
        }

        public void Translation(Point p)
        {
            Point discenter = new Point(_bokehData.Margin.Left + _bokehData.Width / 2 + p.X, _bokehData.Margin.Top + _bokehData.Height / 2 + p.Y);

            if (discenter.X < 0 || discenter.X > _imageControl.Width)
            {
                discenter.X = _bokehData.Margin.Left + _bokehData.Width / 2;
            }

            if (discenter.Y < 0 || discenter.Y > _imageControl.Height)
            {
                discenter.Y = _bokehData.Margin.Top + _bokehData.Height / 2;
            }
            _bokehData.Margin = new Thickness(discenter.X - _bokehData.Width / 2, discenter.Y - _bokehData.Height / 2, 0, 0);
        }

        #region Property
        //界面的缩放比
        protected double _scaleRale { get { return DataManager.Instance.ThumbData.PixelWidth / _imageControl.Width; } }

        protected double _centerX;
        protected double _centerY;

        /// <summary>
        /// 内部最小宽度
        /// </summary>
        protected int _minWidth = 20;

        protected ImageControlData _imageControl = null;

        protected BokehData _bokehData = null;
        #endregion
    }
}
