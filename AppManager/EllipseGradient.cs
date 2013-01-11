using BokehDemo.Models;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace BokehDemo.AppManager
{
    class EllipseGradient : GradientBase
    {
        public EllipseGradient(ImageControlData imageControl, BokehData bokehData)
        {
            _imageControl = imageControl;
            _bokehData = bokehData;
        }

        public override void Initialize(double insideWidth,double outsideWidth)
        {
            _bokehData.Opacity = 1;
            _bokehData.Width = outsideWidth;
            _bokehData.Height = outsideWidth;
            _bokehData.InsideWidth = insideWidth;
            _bokehData.Margin = new Thickness((_imageControl.Width - _bokehData.Width) / 2, (_imageControl.Height - _bokehData.Height) / 2, 0, 0);

            _bokehData.InsideValue = 100 * insideWidth / DataManager.Instance.MaxWidth;
            _bokehData.OutsideValue = 100 *( outsideWidth-insideWidth) / DataManager.Instance.MaxWidth; 
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
        }

        public override void SetGradient()
        {
            base.SetGradient();
            double centerX = _centerX * _scaleRale;
            double centerY = _centerY * _scaleRale;

            _bokehData.StarPoint = new Point(centerX / _imageControl.Width, centerY / _imageControl.Height);

            _bokehData.RadiusX = _bokehData.Width / (_imageControl.Width + _imageControl.Width);
            _bokehData.RadiusY = _bokehData.Width / (_imageControl.Height + _imageControl.Height);
        }

        public override void SaveGradient()
        {
            double rate = DataManager.Instance.MainData.PixelWidth / _bokehData.Width;

            GradientStopCollection collection = new GradientStopCollection();
            collection.Add(new GradientStop() { Color = Colors.Transparent, Offset = _bokehData.Rate });
            collection.Add(new GradientStop() { Color = Color.FromArgb(128, 0, 0, 0), Offset = _bokehData.Rate });
            collection.Add(new GradientStop() { Color = Color.FromArgb(255, 0, 0, 0), Offset = 1 });

            RadialGradientBrush brush = new RadialGradientBrush()
            {
                GradientOrigin = _bokehData.StarPoint,
                Center = _bokehData.StarPoint,
                RadiusX = _bokehData.RadiusX,
                RadiusY = _bokehData.RadiusY,
                GradientStops = collection,
            };
            Rectangle rectangle = new Rectangle()
            {
                Width = DataManager.Instance.MainData.PixelWidth,
                Height = DataManager.Instance.MainData.PixelHeight,
                Fill = _bokehData.MaskBrush,
                OpacityMask = brush
            };
            Canvas saveCanvas = new Canvas()
            {
                Width = rectangle.Width,
                Height = rectangle.Height,
                Background = new ImageBrush() { ImageSource = DataManager.Instance.MainData },
            };
            saveCanvas.Children.Add(rectangle);

            WriteableBitmap bmp = new WriteableBitmap(saveCanvas, null);
            DataManager.Instance.SaveToFile(bmp);
        }
    }
}
