namespace Services
{
    public static class ApiEndpoints
    {
        public const string BaseUrl = "https://localhost:5000/api";
        public const string CarController = $"{BaseUrl}/cars";
        public const string OwnerController = $"{BaseUrl}/owners";
        public const string BookingController = $"{BaseUrl}/bookings";
        public const string ServiceCenterController = $"{BaseUrl}/servicecenters";
        public const string InspectionController = $"{BaseUrl}/inspections";
        public const string ServiceRecordController = $"{BaseUrl}/servicerecords";
    }
}
