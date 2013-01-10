
using System.Windows;
namespace BokehDemo.Models
{
    class LinerData : CommonData
    {
        private Point _endPoint;
        public Point EndPoint
        {
            get { return _endPoint; }
            set
            {
                _endPoint = value;
            }
        }
    }
}
