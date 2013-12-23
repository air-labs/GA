namespace Presenter
{
    public static class ViewSettings
    {
        public const double Epsilon = 0.00001;
        public static double XCount { get; set; }
        public static double YCount { get; set; }
        public static int TotalWidth { get; set; }
        public static int TotalHeight { get; set; }
        public static double CubeWidth
        {
            get { return TotalWidth / XCount; }
        }
        public static double CubeHeight
        {
            get { return TotalHeight / YCount; }
        }
        public static double MaxX { get; set; }
        public static double MinX { get; set; }
        public static double MaxY { get; set; }
        public static double MinY { get; set; }
        public static bool XIsZeroSegment
        {
            get { return MaxX - MinX < Epsilon; }
        }
        public static bool YIsZeroSegment
        {
            get { return MaxY - MinY < Epsilon; }
        }

        public static double GetViewXPoint(double x)
        {
            return (x - MinX) / (MaxX - MinX) * (TotalWidth - CubeWidth);
        }

        public static double GetViewYPoint(double y)
        {
            return (y - MinY) / (MaxY - MinY) * (TotalHeight - CubeHeight);
        }
    }
}