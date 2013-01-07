using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using BokehDemo.Resources;
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

        private void Mode_Click(object sender, EventArgs e)
        {

        }

        private void Open_Click(object sender, EventArgs e)
        {
            _bokehManger.OpenImage();
        }

        private void Save_Click(object sender, EventArgs e)
        {

        }

        private void Settings_Click(object sender, EventArgs e)
        {

        }




        BokehManager _bokehManger;
    }
}