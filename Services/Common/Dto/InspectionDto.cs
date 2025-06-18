namespace Dto;

public class InspectionDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public DateTime? InspectionDate { get; set; } 
    public string Result { get; set; } = string.Empty;
}

public class InspectionCreateDto
{
    public int CarId { get; set; }
    public DateTime? InspectionDate { get; set; } 
    public string Result { get; set; } = string.Empty;
}

public class InspectionUpdateDto
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public DateTime? InspectionDate { get; set; } 
    public string Result { get; set; } = string.Empty;
}
