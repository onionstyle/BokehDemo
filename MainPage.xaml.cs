using System;
using System.Windows.Input;
using Microsoft.Phone.Controls;
using BokehDemo.AppManager;
namespace BokehDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _bokehManger = new BokehManager();
            ShowClipGrid.DataContext = _bokehManger.ImageControlBindingData;
            BokehCanvas.DataContext = _bokehManger.BokehBindingData;
            _bokehManger.SetShowArea();


        }


        private void ImageGrid_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            _bokehManger.SetPreAngle(); 
        }

        private void ImageGrid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (e.PinchManipulation != null)
            {
                _bokehManger.RotationChange(e.PinchManipulation.Current.PrimaryContact, e.PinchManipulation.Current.SecondaryContact, e.PinchManipulation.Current.Center);
            }
            _bokehManger.PositionChange(e.DeltaManipulation.Translation);
        }
        private void ImageGrid_ManipulationCompleted(object sender,ManipulationCompletedEventArgs e)
        {
        }

        private void InSideSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (_bokehManger != null)
            {
                _bokehManger.InsideValueChanged(e.NewValue);
            }
        }
        private void OutSideSlider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (_bokehManger != null)
            {
                _bokehManger.OutsideValueChanged(e.NewValue);
            }
        }

        private void Inside_MouseMove(object sender, MouseEventArgs e)
        {
            _bokehManger.SetMode(BokehMode.InsideMode);
        }

        private void Outside_MouseMove(object sender, MouseEventArgs e)
        {
            _bokehManger.SetMode(BokehMode.OutsideMode);
        }
        private void Outside_MouseLeave(object sender, MouseEventArgs e)
        {
            _bokehManger.SetMode(BokehMode.None);
        }


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
            DataManager.Instance.SaveToFile();
        }

        private void Settings_Click(object sender, EventArgs e)
        {
        }

        BokehManager _bokehManger;



    }
}