using BokehDemo.Models;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BokehDemo.AppManager
{
    class BokehManager
    {
        public BokehManager()
        {
            _photoChooserTask = new PhotoChooserTask();
            _photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }
        /// <summary>
        /// 设置可显示的区域
        /// </summary>
        public void SetShowArea()
        {
            //裁剪显示区域，隐藏超出画布的部分
            _imageControl.ClipWidth = _maxWidth;
            _imageControl.ClipHeight = _maxHeight;
            _imageControl.Clip = new RectangleGeometry() { Rect = new Rect(0, 0, _imageControl.ClipWidth, _imageControl.ClipHeight) };
            _imageControl.ClipMargin = new Thickness(10, 10, 0, 0);
            _imageControl.PropertyChanged += ImageControl_PropertyChanged;
        }


        public void OpenImage()
        {
            _photoChooserTask.Show();
        }
        private void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                _imageControl.Width = bmp.PixelWidth;
                _imageControl.Height = bmp.PixelHeight;
                _imageControl.Source = bmp;
            }
        }

        private void SaveToFile(Byte[] imageByte, int width, int height)
        {
            ////Create filename for JPEG in isolated storage
            //String tempJPEG = String.Format("MTXX_{0:yyyyMMddHHmmss}.jpg", DateTime.Now);

            ////Create virtual store and file stream. Check for duplicate tempJPEG files.
            //var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            //if (myStore.FileExists(tempJPEG))
            //{
            //    myStore.DeleteFile(tempJPEG);
            //}
            //IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);

            //WriteableBitmap wb = ChangetoBmp(imageByte, width, height);

            ////Encode the WriteableBitmap into JPEG stream and place into isolated storage.
            //Extensions.SaveJpeg(wb, myFileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
            //myFileStream.Close();

            ////Create a new file stream.
            //myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);

            ////Add the JPEG file to the photos library on the device.
            //MediaLibrary library = new MediaLibrary();
            //Picture pic = library.SavePicture(tempJPEG, myFileStream);
            //myFileStream.Close();

            //MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK);
        }

        private void ImageControl_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }




        private PhotoChooserTask _photoChooserTask;

        private int _maxWidth = (int)Application.Current.Host.Content.ActualWidth;
        private int _maxHeight = (int)Application.Current.Host.Content.ActualHeight;
        /// <summary>
        /// 图片数据绑定
        /// </summary>
        protected ImageControlData _imageControl = null;
        public ImageControlData ImageControlBindingData
        {
            get { return _imageControl ?? (_imageControl = new ImageControlData()); }
        }
        /// <summary>
        /// 虚化显示数据绑定
        /// </summary>
        protected BokehData _bokehData = null;
        public BokehData BokehBindingData
        {
            get { return _bokehData ?? (_bokehData = new BokehData()); }
        }
    }


    /// <summary>
    /// 鼠标操作模式
    /// </summary>
    public enum BokehMode
    {
        None,

        /// <summary>
        /// 内部的缩放
        /// </summary>
        InsideMode,

        /// <summary>
        /// 外部的缩放
        /// </summary>
        OutsideMode,

        /// <summary>
        /// 矩形旋转
        /// </summary>
        RotaMode,
    }
}
