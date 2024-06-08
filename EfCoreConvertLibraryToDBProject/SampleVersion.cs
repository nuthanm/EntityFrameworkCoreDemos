using System.ComponentModel.DataAnnotations;

public class SampleVersion
{
    [Key]
    public int SampleId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}



