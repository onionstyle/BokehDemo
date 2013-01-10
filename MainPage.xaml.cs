using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using BokehDemo.AppManager;
using System.Windows.Media.Imaging;
using BokehDemo.Models;
using System.Windows.Media;
using System.Windows;
using Windows.Storage;
using System.Windows.Threading;
namespace BokehDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            InitializeBinding();
            _bokehManger.OpenImage();
            InitializeTimer();
        }

        void InitializeBinding()
        {
            _bokehManger = new BokehManager();

            ShowClipGrid.DataContext = _bokehManger.ImageControlBindingData;
            BokehCanvas.DataContext = _bokehManger.BokehBindingData;
            BokehControlGrid.DataContext = _bokehManger.BokehBindingData;
            _bokehManger.SetShowArea();
        }

        void InitializeTimer()
        {
            _dispearTimer = new DispatcherTimer();
            _dispearTimer.Interval = new TimeSpan(0, 0, 0, 2);
            _dispearTimer.Tick += (s, e) =>
            {
                _bokehManger.SetOpacity(0.3);//松开2秒隐藏
                _dispearTimer.Stop();
            };
        }

        private void ImageGrid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                //缩放
                _bokehManger.ScaleChange(e.PinchManipulation.DeltaScale);

                //旋转
                _bokehManger.RotationChange(e.PinchManipulation.Current.PrimaryContact, e.PinchManipulation.Current.SecondaryContact, e.PinchManipulation.Current.Center);
            }
            _bokehManger.SetGradient();
        }

        private void Inside_MouseEnter(object sender, MouseEventArgs e)
        {
            _dispearTimer.Stop();
            _bokehManger.SetOpacity(0.3);

            _bokehManger.SetPrePoint(e.GetPosition(ImageGrid));
            _bokehManger.SetMode(BokehMode.InsideMode);
        }

        private void Outside_MouseEnter(object sender, MouseEventArgs e)
        {
            _dispearTimer.Stop();
            _bokehManger.SetOpacity(0.3);

            _bokehManger.SetPrePoint(e.GetPosition(ImageGrid));
            _bokehManger.SetMode(BokehMode.OutsideMode);
        }

        private void Inside_MouseMove(object sender, MouseEventArgs e)
        {
            _bokehManger.PositionChange(e.GetPosition(ImageGrid));
        }

        private void Outside_MouseMove(object sender, MouseEventArgs e)
        {
            _bokehManger.PositionChange(e.GetPosition(ImageGrid));
        }

        private void Allside_MouseLeave(object sender, MouseEventArgs e)
        {
            _dispearTimer.Start();
            _bokehManger.SetOpacity(1);

            _bokehManger.SetMode(BokehMode.None);
        }

        #region ApplicationBarButton

        private void Mode_Click(object sender, EventArgs e)
        {
            _bokehManger.ModeChanger();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            _bokehManger.OpenImage();
        }

        private void Save_Click(object sender, EventArgs e)
        {
           _bokehManger.SaveImage();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
        }

        #endregion


        /// <summary>
        /// 计时器
        /// </summary>
        DispatcherTimer _dispearTimer;
        BokehManager _bokehManger;
    }
}