namespace ThreeDimensionPrinter
{
    public static class MotorFactory
    {
        public enum VendorType
        {
            VendorA,
            VendorB
        }

        public static IMotor CreateMotor(string name, VendorType vendorType)
        {
            return vendorType switch
            {
                VendorType.VendorA => new VendorAMotorAdapter(name),
                VendorType.VendorB => new VendorBMotorAdapter(name),
                _ => throw new ArgumentException($"Unsupported vendor type: {vendorType}")
            };
        }
    }
}