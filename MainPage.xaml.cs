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
using Microsoft.Xna.Framework.Input.Touch;
namespace BokehDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            TouchPanel.EnabledGestures = GestureType.FreeDrag;

            InitializeBinding();
            InitializeTimer();
            _bokehManger.OpenImage();
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
    
        private void ImageGrid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _bokehManger.SetPreAngel();
        }

        private void ImageGrid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                //缩放
                _bokehManger.ScaleChange(e.PinchManipulation.DeltaScale);

                _bokehManger.RotationChange(e.PinchManipulation.Current, e.PinchManipulation.Original);
            }
            else
            {
                while (TouchPanel.IsGestureAvailable)
                {
                    GestureSample sample = TouchPanel.ReadGesture();
                    Point sampleDelta = new Point(sample.Delta.X, sample.Delta.Y);
                    _bokehManger.PositionChange(sampleDelta);
                }
                _bokehManger.SetPreAngel();
            }
            _bokehManger.SetGradient();
        }

        private void Inside_MouseEnter(object sender, MouseEventArgs e)
        {
            _dispearTimer.Stop();
            _bokehManger.SetOpacity(0.3);

            _bokehManger.SetMode(BokehMode.InsideMode);
        }

        private void Outside_MouseEnter(object sender, MouseEventArgs e)
        {
            _dispearTimer.Stop();
            _bokehManger.SetOpacity(0.3);

            _bokehManger.SetMode(BokehMode.OutsideMode);
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