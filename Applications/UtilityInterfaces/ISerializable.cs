namespace BookingApp.Applications.UtilityInterfaces
{
    public interface ISerializable
    {
        string[] ToCSV();
        void FromCSV(string[] values);
    }
}
