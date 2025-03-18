namespace ThreeDimensionPrinter
{
    public static class Units
    {
        public static double CountToNm(int count)
        {
            return count * 0.2;
        }

        public static int NmToCount(double nm)
        {
            return (int)(nm / 0.2);
        }
    }
}