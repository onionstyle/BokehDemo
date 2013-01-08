using BokehDemo.Models;
using System;
using System.Windows;
namespace BokehDemo.AppManager
{
    class EllipseGradient:GradientBase
    {
        public override void Initialize(ImageControlData imageControl, BokehData bokehData)
        {
            base.Initialize(imageControl, bokehData);
            _bokehData.EllipseVisibility = Visibility.Visible;
            _bokehData.LineVisibility = Visibility.Collapsed;
            _bokehData.Width = 300;
            _bokehData.Height = 300;
            _bokehData.InsideWidth = 200;
            _bokehData.Margin = new Thickness((_imageControl.Width - _bokehData.Width) / 2, (_imageControl.Height - _bokehData.Height) / 2, 0, 0);
        }

        public override void ReSet()
        {
            _bokehData.Margin = new Thickness((_imageControl.Width - _bokehData.Width) / 2, (_imageControl.Height - _bokehData.Height) / 2, 0, 0);
        }

        public override void ModeMove(Point p)
        {
            base.ModeMove(p);
            double width = Math.Sqrt(_disCen.X * _disCen.X + _disCen.Y * _disCen.Y);
            width += width;
            ScaleRestrict(width, _bokehMode);
        }

        public override void ScaleRestrict(double width, BokehMode bokeh)
        {
            switch (bokeh)
            {
                case BokehMode.InsideMode:
                    if (width > _minWidth)
                    {
                        _bokehData.Margin = new Thickness(_bokehData.Margin.Left - (width - _bokehData.InsideWidth) / 2, _bokehData.Margin.Top - (width - _bokehData.InsideWidth) / 2, 0, 0);
                        _bokehData.Width = _bokehData.Height = _bokehData.Height + (width - _bokehData.InsideWidth);
                        _bokehData.InsideWidth = width;
                    } break;
                case BokehMode.OutsideMode:
                    if (width > _bokehData.InsideWidth + _minWidth)//内外圆相差minWidth
                    {
                        _bokehData.Margin = new Thickness(_bokehData.Margin.Left - (width - _bokehData.Width) / 2, _bokehData.Margin.Top - (width - _bokehData.Width) / 2, 0, 0);
                        _bokehData.Width = _bokehData.Height = width;
                    } break;
            }
            base.ScaleRestrict(width, bokeh);
        }

        public override byte[] Process(bool isApply = false)
        {
            base.Process(isApply);
            double centerX = _centerX * _scaleRale;
            double centerY = _centerY * _scaleRale;
            double insideLength = (_bokehData.InsideWidth / 2) * _scaleRale;
            double outsideLength = (_bokehData.Width / 2) * _scaleRale;

            byte[] data = null;
            //主图与缩略图比例
            float scale = 1;
            if (isApply)
            {
            //    scale = (float)FaceMainManager.Instance.MainWidth / FaceMainManager.Instance.ThumbWidth;
                //因为灰度对byte直接操作，所以不能直接将主图数据应用
          //      data = new byte[FaceMainManager.Instance.MainData.Length];
           //     FaceMainManager.Instance.MainData.CopyTo(data, 0);
            }
            if (data != null)
            {
           //     data = _gradient.RadiusApply(data, FaceMainManager.Instance.MainWidth, FaceMainManager.Instance.MainHeight, (float)centerX * scale, (float)centerY * scale, (float)insideLength * scale, (float)outsideLength * scale);
            }
            else
            {
           //     data = _gradient.RadiusGradient((float)centerX, (float)centerY, (float)insideLength, (float)outsideLength);
            }
            return data;
        }
    }
}
