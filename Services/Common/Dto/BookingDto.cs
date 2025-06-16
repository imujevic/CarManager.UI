namespace Dto;

public class BookingDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int OwnerId { get; set; }
    public int ServiceCenterId { get; set; }
    public DateTime BookingDate { get; set; }
}

public class BookingCreateDto
{
    public int CarId { get; set; }
    public int OwnerId { get; set; }
    public int ServiceCenterId { get; set; }
    public DateTime BookingDate { get; set; }
}

public class BookingUpdateDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public int OwnerId { get; set; }
    public int ServiceCenterId { get; set; }
    public DateTime BookingDate { get; set; }
}
