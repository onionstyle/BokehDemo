using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media.Imaging;
namespace BokehDemo.AppManager
{
    class DataManager
    {
        DataManager()
        {
        }
        private static DataManager _dataManager;
        public static DataManager Instance
        {
            get { return _dataManager ?? (_dataManager = new DataManager()); }
        }

        //对打开图片大小处理,生成缩略图
        public void SetMainData(WriteableBitmap bmp)
        {
            _mainData = bmp; 
            CreateThumbData();
        }

        public void CreateThumbData()
        {
            //长宽比例
            float rate = _mainData.PixelHeight / (float)_mainData.PixelWidth;
            float tempWidth = _mainData.PixelWidth;
            float tempHeight = _mainData.PixelHeight;
            //在宽度范围
            if (tempWidth > MaxWidth)
            {
                tempWidth = MaxWidth;
                tempHeight = Convert.ToInt32(tempWidth * rate);
            }
            //判断高度范围
            if (tempHeight > MaxHeight)
            {
                tempHeight = MaxHeight;
                tempWidth = tempHeight / rate;
            }

            _thumbData = _mainData.Resize(Convert.ToInt32(tempWidth), Convert.ToInt32(tempHeight), WriteableBitmapExtensions.Interpolation.NearestNeighbor);
        }

        public void SaveToFile(WriteableBitmap bmp)
        {
            //Create filename for JPEG in isolated storage
            String tempJPEG = String.Format("MTXX_{0:yyyyMMddHHmmss}.jpg", DateTime.Now);

            //Create virtual store and file stream. Check for duplicate tempJPEG files.
            var myStore = IsolatedStorageFile.GetUserStoreForApplication();
            if (myStore.FileExists(tempJPEG))
            {
                myStore.DeleteFile(tempJPEG);
            }
            IsolatedStorageFileStream myFileStream = myStore.CreateFile(tempJPEG);

            //Encode the WriteableBitmap into JPEG stream and place into isolated storage.
            Extensions.SaveJpeg(bmp, myFileStream, bmp.PixelWidth, bmp.PixelHeight, 0, 85);
            myFileStream.Close();

            //Create a new file stream.
            myFileStream = myStore.OpenFile(tempJPEG, FileMode.Open, FileAccess.Read);

            //Add the JPEG file to the photos library on the device.
            MediaLibrary library = new MediaLibrary();
            Picture pic = library.SavePicture(tempJPEG, myFileStream);
            myFileStream.Close();

            MessageBox.Show("保存成功！", "提示", MessageBoxButton.OK);
        }

        /// <summary>
        /// 当前主图数据
        /// </summary>
        WriteableBitmap _mainData = null;
        public WriteableBitmap MainData
        {
            get { return _mainData; }
        }

        /// <summary>
        /// 缩略图数据
        /// </summary>
        WriteableBitmap _thumbData = null;
        public WriteableBitmap ThumbData
        {
            get { return _thumbData; }
            set { _thumbData = value; }
        }
     
  
        /// <summary>
        /// 图片显示最大高度
        /// </summary>
        int _maxHeight = (int)Application.Current.Host.Content.ActualHeight - 104 - 139;//减去Appbar&&slider
        public int MaxHeight
        {
            get { return _maxHeight; }
        }

        /// <summary>
        /// 图片显示最大宽度
        /// </summary>
        int _maxWidth = (int)Application.Current.Host.Content.ActualWidth;
        public int MaxWidth
        {
            get { return _maxWidth; }
        }

        public const int MinWidth = 100;
        public const int MinHeight = 100;
    }
}
