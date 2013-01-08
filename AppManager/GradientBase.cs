using BokehDemo.Models;
using System;
using System.Windows;
namespace BokehDemo.AppManager
{
    /// <summary>
    /// 虚化基类
    /// </summary>
    class GradientBase
    {
        public virtual void Initialize(ImageControlData imageControl, BokehData bokehData)
        {
            if (_imageControl == null)
            {
                _imageControl = imageControl;
                _bokehData = bokehData;
            }
        }

        /// <summary>
        /// 图片大小位置变化后对应变化图形位置大小
        /// </summary>
        public virtual void ReSet()
        {
        }

        /// <summary>
        /// 拉动实现放大缩小图形
        /// </summary>
        /// <param name="p">距中心点的位置</param>
        /// <param name="bokehMode">当前点击模式</param>    
        public virtual void ModeMove(Point p)
        {
            _centerX = _bokehData.Margin.Left + _bokehData.Width / 2;
            _centerY = _bokehData.Margin.Top + _bokehData.Height / 2;
            _disCen = new Point(p.X - _centerX, _centerY - p.Y);
        }

        /// <summary>
        ///  放大缩小时限制图形最大最小宽高,并调整位置
        /// </summary>
        /// <param name="width">新宽高</param>
        public virtual void ScaleRestrict(double width, BokehMode bokeh)
        {
        }

        public virtual byte[] Process(bool isApply = false)
        {
            _centerX = _bokehData.Margin.Left + _bokehData.Width / 2;
            _centerY = _bokehData.Margin.Top + _bokehData.Height / 2;

            double insideLength = _bokehData.InsideWidth / 2;
            double outsideLength = _bokehData.Width / 2;

            return null;
        }

        /// <summary>
        /// 设置鼠标形状
        /// </summary>
        /// <param name="p"></param>
        public virtual void PointerSet(Point p)
        {
        }

        public double Radians(Point currPoint, Point pinPoint)
        {
            return Math.Atan2(currPoint.Y - pinPoint.Y, currPoint.X - pinPoint.X);
        }


        #region Property
        //界面的缩放比
        protected double _scaleRale { get { return (double)DataManager.Instance.ThumbData.PixelWidth / _imageControl.Width; } }

        /// <summary>
        /// 当前点与中心的x,y差值
        /// </summary>
        protected Point _disCen;
        protected double _centerX;
        protected double _centerY;

        /// <summary>
        /// 内部最小宽度
        /// </summary>
        protected int _minWidth = 10;

        protected BokehMode _bokehMode;
        public BokehMode Mode
        {
            set { _bokehMode = value; }
        }

        protected ImageControlData _imageControl = null;

        protected BokehData _bokehData = null;
        #endregion
    }
}
