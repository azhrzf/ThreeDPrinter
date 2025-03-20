namespace ThreeDimensionPrinter.Utils
{
    public static class Units
    {
        public static double CountToMm(int count)
        {
            return count * 0.2;
        }

        public static int MmToCount(double mm)
        {
            return (int)(mm / 0.2);
        }
    }
}