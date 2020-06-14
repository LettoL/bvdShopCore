namespace Domain.Entities.Olds
{
    public class BookingManagerOld : Entity
    {
        public int BookingId { get; private set; }

        public int ManagerId { get; private set; }

        public BookingManagerOld(int bookingId, int managerId) =>
            (BookingId, ManagerId) = (bookingId, managerId);
    }
}