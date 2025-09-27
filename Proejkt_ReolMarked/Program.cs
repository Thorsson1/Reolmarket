using System;
using ReolMarked.Data;
using ReolMarked.Models;
using ReolMarked.Repositories;

class Program
{
    static void Main()
    {
        using var ctx = new ReolMarkedContext();
        var reolRepo = new ReolRepository(ctx);
        var productRepo = new ProductRepository(ctx);

        bool kører = true;
        while (kører)
        {
            Console.WriteLine("\n--- ReolMarked Menu ---");
            Console.WriteLine("1. Vis alle reoler");
            Console.WriteLine("2. Tilføj reol");
            Console.WriteLine("3. Rediger reol");
            Console.WriteLine("4. Slet reol");
            Console.WriteLine("5. Vis alle produkter");
            Console.WriteLine("6. Tilføj produkt");
            Console.WriteLine("7. Rediger produkt");
            Console.WriteLine("8. Slet produkt");
            Console.WriteLine("9. Afslut");
            Console.Write("Vælg handling: ");
            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1": VisReolListe(reolRepo); break;
                case "2": TilføjReol(reolRepo); break;
                case "3": RedigerReol(reolRepo); break;
                case "4": SletReol(reolRepo); break;
                case "5": VisProduktListe(productRepo); break;
                case "6": TilføjProdukt(productRepo); break;
                case "7": RedigerProdukt(productRepo); break;
                case "8": SletProdukt(productRepo); break;
                case "9": kører = false; break;
                default: Console.WriteLine("Ugyldigt valg!"); break;
            }
        }
    }

    static void VisReolListe(IReolRepository repo)
    {
        var reoler = repo.GetAll();
        Console.WriteLine("\n--- Alle Reoler ---");
        foreach (var r in reoler)
            Console.WriteLine($"ID: {r.ReolId} | Nr: {r.ReolNummer} | Status: {r.Status} | Pris: {r.Pris} | Type: {r.ReolType} | Lejer: {r.ReolLejer ?? "Ingen"} | Placering: {r.Placering}");
    }

    static void TilføjReol(IReolRepository repo)
    {
        var nyReol = new Reol();
        Console.Write("ReolNummer: "); nyReol.ReolNummer = Console.ReadLine() ?? "";
        Console.Write("Placering: "); nyReol.Placering = Console.ReadLine() ?? "";
        Console.Write("Status: "); nyReol.Status = Console.ReadLine() ?? "Ledig";
        Console.Write("ReolType: "); nyReol.ReolType = Console.ReadLine() ?? "";
        Console.Write("Pris: "); nyReol.Pris = decimal.TryParse(Console.ReadLine(), out var pris) ? pris : 0;
        Console.Write("ReolLejer (kan være tom): "); nyReol.ReolLejer = string.IsNullOrWhiteSpace(Console.ReadLine()) ? null : Console.ReadLine();

        repo.Add(nyReol);
        repo.Save();
        Console.WriteLine("Ny reol oprettet!");
    }

    static void RedigerReol(IReolRepository repo)
    {
        Console.Write("Indtast ReolId på reolen du vil redigere: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;

        var reol = repo.GetById(id);
        if (reol == null) { Console.WriteLine("Reol ikke fundet!"); return; }

        Console.Write($"ReolNummer ({reol.ReolNummer}): "); var input = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(input)) reol.ReolNummer = input;
        Console.Write($"Placering ({reol.Placering}): "); input = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(input)) reol.Placering = input;
        Console.Write($"Status ({reol.Status}): "); input = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(input)) reol.Status = input;
        Console.Write($"ReolType ({reol.ReolType}): "); input = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(input)) reol.ReolType = input;
        Console.Write($"Pris ({reol.Pris}): "); input = Console.ReadLine(); if (decimal.TryParse(input, out var pris)) reol.Pris = pris;
        Console.Write($"ReolLejer ({reol.ReolLejer ?? "Ingen"}): "); input = Console.ReadLine(); reol.ReolLejer = string.IsNullOrWhiteSpace(input) ? null : input;

        repo.Update(reol);
        repo.Save();
        Console.WriteLine("Reol opdateret!");
    }

    static void SletReol(IReolRepository repo)
    {
        Console.Write("Indtast ReolId på reolen du vil slette: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        repo.Delete(id);
        repo.Save();
        Console.WriteLine("Reol slettet!");
    }

    static void VisProduktListe(IProductRepository repo)
    {
        var produkter = repo.GetAll();
        Console.WriteLine("\n- Alle Produkter -");
        foreach (var p in produkter)
            Console.WriteLine($"ID: {p.ProductId} | Beskrivelse: {p.Beskrivelse} | Pris: {p.Pris} | Kommission: {p.Kommission} | ReolNr: {p.ReolNummer}");
    }

    static void TilføjProdukt(IProductRepository repo)
    {
        var produkt = new Product();
        Console.Write("Beskrivelse: "); produkt.Beskrivelse = Console.ReadLine() ?? "";
        Console.Write("Pris: "); produkt.Pris = decimal.TryParse(Console.ReadLine(), out var pris) ? pris : 0;
        Console.Write("Kommission: "); produkt.Kommission = decimal.TryParse(Console.ReadLine(), out var kom) ? kom : 0;
        Console.Write("ReolNummer: "); produkt.ReolNummer = Console.ReadLine() ?? "";

        repo.Add(produkt);
        repo.Save();
        Console.WriteLine("Produkt oprettet!");
    }

    static void RedigerProdukt(IProductRepository repo)
    {
        Console.Write("Indtast ProductId på produktet du vil redigere: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;

        var produkt = repo.GetById(id);
        if (produkt == null) { Console.WriteLine("Produkt ikke fundet!"); return; }

        Console.Write($"Beskrivelse ({produkt.Beskrivelse}): "); var input = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(input)) produkt.Beskrivelse = input;
        Console.Write($"Pris ({produkt.Pris}): "); input = Console.ReadLine(); if (decimal.TryParse(input, out var pris)) produkt.Pris = pris;
        Console.Write($"Kommission ({produkt.Kommission}): "); input = Console.ReadLine(); if (decimal.TryParse(input, out var kom)) produkt.Kommission = kom;
        Console.Write($"ReolNummer ({produkt.ReolNummer}): "); input = Console.ReadLine(); if (!string.IsNullOrWhiteSpace(input)) produkt.ReolNummer = input;

        repo.Update(produkt);
        repo.Save();
        Console.WriteLine("Produkt opdateres");
    }

    static void SletProdukt(IProductRepository repo)
    {
        Console.Write("Indtast ProductId på produktet du vil slette: ");
        if (!int.TryParse(Console.ReadLine(), out var id)) return;
        repo.Delete(id);
        repo.Save();
        Console.WriteLine("Produkt slettet!");
    }
}
