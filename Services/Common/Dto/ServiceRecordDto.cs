namespace Dto;

public class ServiceRecordDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}

public class ServiceRecordCreateDto
{
    public int CarId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}

public class ServiceRecordUpdateDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public DateTime ServiceDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
}
