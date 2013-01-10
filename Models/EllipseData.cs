namespace BokehDemo.Models
{
    class EllipseData :CommonData
    {

        private double _radiusX;
        private double _radiusY;

        public double RadiusX 
        { 
            get { return _radiusX; } 
            set { _radiusX = value; } 
        }
        public double RadiusY
        {
            get { return _radiusY; }
            set { _radiusY = value; }
        }

    }
}
