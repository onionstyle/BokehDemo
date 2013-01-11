using BokehDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BokehDemo.AppManager
{
    class LinerGradient : GradientBase
    {
        public LinerGradient(ImageControlData imageControl, BokehData bokehData)
        {
            _imageControl = imageControl;
            _bokehData = bokehData;
        }

        public override void Initialize(double insideWidth, double outsideWidth)
        {
            _bokehData.Height = Math.Sqrt(_imageControl.Width * _imageControl.Width + _imageControl.Height * _imageControl.Height);
            _bokehData.Height += _bokehData.Height;

            _bokehData.Opacity = 1;
            _bokehData.Angle = 90;
            _bokehData.InsideWidth = insideWidth;
            _bokehData.Width = outsideWidth; 
            _bokehData.Margin = new Thickness((_imageControl.Width - _bokehData.Width) / 2, -(_bokehData.Height - _imageControl.Height) / 2, 0, 0);

            _bokehData.InsideValue = 100 * insideWidth / DataManager.Instance.MaxWidth;
            _bokehData.OutsideValue = 100 *( outsideWidth-insideWidth) / DataManager.Instance.MaxWidth;
            SetGradient();
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

                    }
                    break;
            }
            SetGradient();
        }

        public override void SetGradient()
        {
            base.SetGradient();
            double centerX = _centerX * _scaleRale; ;
            double centerY = _centerY * _scaleRale; ;
            double insideLength = (_bokehData.InsideWidth / 2) * _scaleRale;
            double outsideLength = (_bokehData.Width / 2) * _scaleRale;

            //与中心点坐标差值
            double inLinDevX = insideLength * Math.Cos(_bokehData.Angle * Math.PI / 180);
            double inLinDevY = insideLength * Math.Sin(_bokehData.Angle * Math.PI / 180);
            double outLinDevX = outsideLength * Math.Cos(_bokehData.Angle * Math.PI / 180);
            double outLinDevY = outsideLength * Math.Sin(_bokehData.Angle * Math.PI / 180);
            //外线坐标
            double outsideLinX = centerX + outLinDevX;
            double outsideLinY = centerY + outLinDevY;

            _bokehData.StarPoint = new Point(centerX / _imageControl.Width, centerY / _imageControl.Height);
            _bokehData.EndPoint = new Point(outsideLinX / _imageControl.Width, outsideLinY / _imageControl.Height);

        }

        public override void SaveGradient()
        {
            double rate = DataManager.Instance.MainData.PixelWidth / _bokehData.Width;

            GradientStopCollection collection = new GradientStopCollection();
            collection.Add(new GradientStop() { Color = Colors.Transparent, Offset = _bokehData.Rate });
            collection.Add(new GradientStop() { Color = Color.FromArgb(128, 0, 0, 0), Offset = _bokehData.Rate });
            collection.Add(new GradientStop() { Color = Color.FromArgb(255, 0, 0, 0), Offset = 1 });

            LinearGradientBrush brush = new LinearGradientBrush()
            {
                StartPoint = _bokehData.StarPoint,
                EndPoint = _bokehData.EndPoint,
                GradientStops = collection,
            };
            Rectangle rectangle = new Rectangle()
            {
                Width = DataManager.Instance.MainData.PixelWidth,
                Height = DataManager.Instance.MainData.PixelHeight,
                Fill = _bokehData.MaskBrush,
                OpacityMask = brush
            };

            GradientStopCollection backcollection = new GradientStopCollection();
            backcollection.Add(new GradientStop() { Color = Colors.Transparent, Offset = _bokehData.Rate });
            backcollection.Add(new GradientStop() { Color = Color.FromArgb(128, 0, 0, 0), Offset = _bokehData.Rate });
            backcollection.Add(new GradientStop() { Color = Color.FromArgb(255, 0, 0, 0), Offset = 1 });
            LinearGradientBrush backBrush = new LinearGradientBrush()
            {
                StartPoint = _bokehData.StarPoint,
                EndPoint = _bokehData.BackEndPoint,
                GradientStops = backcollection,
            };
            Rectangle backRectangle = new Rectangle()
            {
                Width = rectangle.Width,
                Height = rectangle.Height,
                Fill = rectangle.Fill,
                OpacityMask = backBrush
            };

            Canvas saveCanvas = new Canvas()
            {
                Width = rectangle.Width,
                Height = rectangle.Height,
                Background = new ImageBrush() { ImageSource = DataManager.Instance.MainData },
            };
            saveCanvas.Children.Add(rectangle);
            saveCanvas.Children.Add(backRectangle);

            WriteableBitmap bmp = new WriteableBitmap(saveCanvas, null);
            DataManager.Instance.SaveToFile(bmp);
        }
    }
}
