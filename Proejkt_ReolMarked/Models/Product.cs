namespace ReolMarked.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Beskrivelse { get; set; } = "";
    public decimal Pris { get; set; }
    public decimal Kommission { get; set; }
    public string ReolNummer { get; set; } = "";
}
