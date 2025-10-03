namespace ReolMarked.Models
{
    public class Lejer
    {
        public int LejerId { get; set; }       
        public string Navn { get; set; } = "";
        public string Email { get; set; } = "";
        public string Tlf { get; set; } = "";
        public string Adresse { get; set; } = "";

        // Fremmednøgle til Reol
        public int ReolId { get; set; }
        public Reol? Reol { get; set; }

        public decimal MånedligLeje { get; set; }
        public decimal Indtjent { get; set; }
        public string Betalingsmetode { get; set; } = "";
    }
}