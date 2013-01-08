using BokehDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BokehDemo.AppManager
{
    class LinerGradient:GradientBase
    {
        public override void Initialize(ImageControlData imageControl, BokehData bokehData)
        {
            base.Initialize(imageControl, bokehData);
            _bokehData.EllipseVisibility = Visibility.Collapsed;
            _bokehData.LineVisibility = Visibility.Visible;
            _bokehData.Width = 400;
            _bokehData.Height = Math.Sqrt(_imageControl.Width * _imageControl.Width + _imageControl.Height * _imageControl.Height);
            _bokehData.Height += _bokehData.Height;
            _bokehData.Margin = new Thickness((_imageControl.Width - _bokehData.Width) / 2, -(_bokehData.Height - _imageControl.Height) / 2, 0, 0);
            _bokehData.Angle = 0;
            _bokehData.InsideWidth = 200;
        }

        public override void ReSet()
        {
            _bokehData.Height = Math.Sqrt(_imageControl.Width * _imageControl.Width + _imageControl.Height * _imageControl.Height);
            _bokehData.Height += _bokehData.Height;
            _bokehData.Margin = new Thickness((_imageControl.Width - _bokehData.Width) / 2, -(_bokehData.Height - _imageControl.Height) / 2, 0, 0);
        }

        public override void ModeMove(Point p)
        {
            //base.ModeMove(p);
            ////计算线到中心垂直时中心到点与横线的夹角
            //double angle = _bokehData.Angle - Angle(_disCen,new Point(0,1));
            //double sin = Math.Sin(angle * Math.PI / 180);
            //double lengthX = _centerX - p.X;
            //double lengthY = _centerY - p.Y;
            ////垂直距离
            //double length = Math.Sqrt(lengthX * lengthX + lengthY * lengthY) * sin;

            //double width = Math.Abs(length + length);
            //ScaleRestrict(width, _bokehMode);
        }

        public override void ScaleRestrict(double width, BokehMode bokeh)
        {
            switch (bokeh)
            {
                case BokehMode.InsideMode:
                    //中心2线相距大于minWidth
                    if (width > _minWidth)
                    {
                        _bokehData.Margin = new Thickness(_bokehData.Margin.Left - (width - _bokehData.InsideWidth) / 2, _bokehData.Margin.Top, 0, 0);
                        _bokehData.Width += width - _bokehData.InsideWidth;
                        _bokehData.InsideWidth = width;
                    }
                    break;
                case BokehMode.OutsideMode:
                    //外面2线相距
                    if (width > _bokehData.InsideWidth + _minWidth)
                    {
                        _bokehData.Margin = new Thickness(_bokehData.Margin.Left - (width - _bokehData.Width) / 2, _bokehData.Margin.Top, 0, 0);
                        _bokehData.Width = width;
                    } break;
            }
            base.ScaleRestrict(width, bokeh);
        }
    }
}
