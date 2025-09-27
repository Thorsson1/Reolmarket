namespace ReolMarked.Models;

public class Reol
{
    public int ReolId { get; set; }
    public string ReolNummer { get; set; } = "";
    public string Placering { get; set; } = "";
    public string Status { get; set; } = "Ledig";
    public string ReolType { get; set; } = "";
    public decimal Pris { get; set; }
    public string? ReolLejer { get; set; }
}
