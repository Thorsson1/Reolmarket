using System;
using System.Diagnostics.Metrics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
            Console.WriteLine("Velkommen til ReolMarked");
            Console.WriteLine("1. Vis alle reoler");
            Console.WriteLine("2. Tilføj reol");
            Console.WriteLine("3. Rediger reol");
            Console.WriteLine("4. Slet reol");
            Console.WriteLine("5. Vis alle produkter");
            Console.WriteLine("6. Tilføj produkt");
            Console.WriteLine("7. Rediger produkt");
            Console.WriteLine("8. Slet produkt");
            Console.WriteLine("9. Lejer menu");
            Console.WriteLine("10. Afslut");
            Console.Write("Vælg en handling (1-10): ");
            string valg = Console.ReadLine();

            switch (valg)
            {
                case "1": VisReoler(reolRepo); break;
                case "2": TilføjReol(reolRepo); break;
                case "3": RedigerReol(reolRepo); break;
                case "4": SletReol(reolRepo); break;
                case "5": VisProdukter(productRepo); break;
                case "6": TilføjProdukt(productRepo); break;
                case "7": RedigerProdukt(productRepo); break;
                case "8": SletProdukt(productRepo); break;
                case "9": LejerMenu(ctx); break;
                case "10":
                    Console.WriteLine("Program afsluttet");
                    kører = false;
                    break;
                default:
                    Console.WriteLine("Ugyldigt valg");
                    break;
            }

            Console.WriteLine();
        }
    }

    static string LæsInput(string prompt, string? standard = null)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
            return standard ?? "";
        return input;
    }

    static decimal LæsDecimal(string prompt, decimal standard = 0)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        return decimal.TryParse(input, out var værdi) ? værdi : standard;
    }

    // --- Reoler ---
    static void VisReoler(IReolRepository repo)
    {
        var reoler = repo.GetAll();
        Console.WriteLine("Alle reoler");
        foreach (var r in reoler)
        {
            string lejer = string.IsNullOrEmpty(r.ReolLejer) ? "Ingen" : r.ReolLejer;
            Console.WriteLine($"ID: {r.ReolId} | Nr: {r.ReolNummer} | Status: {r.Status} | Pris: {r.Pris:C} | Type: {r.ReolType} | Lejer: {lejer} | Placering: {r.Placering}");
        }
    }

    static void TilføjReol(IReolRepository repo)
    {
        var nyReol = new Reol
        {
            ReolNummer = LæsInput("Reolnummer: "),
            Placering = LæsInput("Placering: "),
            Status = LæsInput("Status (standard 'Ledig'): ", "Ledig"),
            ReolType = LæsInput("Reoltype: "),
            Pris = LæsDecimal("Pris: "),
            ReolLejer = LæsInput("Lejer (kan være tom): ", null)
        };

        repo.Add(nyReol);
        repo.Save();
        Console.WriteLine("Reol oprettet");
    }

    static void RedigerReol(IReolRepository repo)
    {
        int id = int.Parse(LæsInput("Indtast ReolId på reolen du vil redigere: "));
        var reol = repo.GetById(id);
        if (reol == null) { Console.WriteLine("Reol ikke fundet"); return; }

        reol.ReolNummer = LæsInput($"Reolnummer ({reol.ReolNummer}): ", reol.ReolNummer);
        reol.Placering = LæsInput($"Placering ({reol.Placering}): ", reol.Placering);
        reol.Status = LæsInput($"Status ({reol.Status}): ", reol.Status);
        reol.ReolType = LæsInput($"Reoltype ({reol.ReolType}): ", reol.ReolType);
        reol.Pris = LæsDecimal($"Pris ({reol.Pris}): ", reol.Pris);
        reol.ReolLejer = LæsInput($"Lejer ({reol.ReolLejer ?? "Ingen"}): ", reol.ReolLejer);

        repo.Update(reol);
        repo.Save();
        Console.WriteLine("Reol opdateret");
    }

    static void SletReol(IReolRepository repo)
    {
        int id = int.Parse(LæsInput("Indtast ReolId på reolen du vil slette: "));
        repo.Delete(id);
        repo.Save();
        Console.WriteLine("Reol slettet");
    }

    // --- Produkter ---
    static void VisProdukter(IProductRepository repo)
    {
        var produkter = repo.GetAll();
        Console.WriteLine("Alle produkter");
        foreach (var p in produkter)
        {
            Console.WriteLine($"ID: {p.ProductId} | {p.Beskrivelse} | Pris: {p.Pris:C} | Kommission: {p.Kommission:C} | Reol: {p.ReolNummer}");
        }
    }

    static void TilføjProdukt(IProductRepository repo)
    {
        var produkt = new Product
        {
            Beskrivelse = LæsInput("Beskrivelse: "),
            Pris = LæsDecimal("Pris: "),
            Kommission = LæsDecimal("Kommission: "),
            ReolNummer = LæsInput("Reolnummer: ")
        };

        repo.Add(produkt);
        repo.Save();
        Console.WriteLine("Produkt oprettet");
    }

    static void RedigerProdukt(IProductRepository repo)
    {
        int id = int.Parse(LæsInput("Indtast ProductId på produktet du vil redigere: "));
        var produkt = repo.GetById(id);
        if (produkt == null) { Console.WriteLine("Produkt ikke fundet"); return; }

        produkt.Beskrivelse = LæsInput($"Beskrivelse ({produkt.Beskrivelse}): ", produkt.Beskrivelse);
        produkt.Pris = LæsDecimal($"Pris ({produkt.Pris}): ", produkt.Pris);
        produkt.Kommission = LæsDecimal($"Kommission ({produkt.Kommission}): ", produkt.Kommission);
        produkt.ReolNummer = LæsInput($"Reolnummer ({produkt.ReolNummer}): ", produkt.ReolNummer);

        repo.Update(produkt);
        repo.Save();
        Console.WriteLine("Produkt opdateret");
    }

    static void SletProdukt(IProductRepository repo)
    {
        int id = int.Parse(LæsInput("Indtast ProductId på produktet du vil slette: "));
        repo.Delete(id);
        repo.Save();
        Console.WriteLine("Produkt slettet");
    }

    // Lejer menu igennem 9
    static void LejerMenu(ReolMarkedContext ctx)
    {
        bool kører = true;
        while (kører)
        {
            Console.WriteLine("\n--- Lejer Menu ---");
            Console.WriteLine("1. Opret Lejer");
            Console.WriteLine("2. Lejer Oversigt");
            Console.WriteLine("3. Rediger lejer");
            Console.WriteLine("4. Slet lejer");
            Console.WriteLine("5. Afslut");
            Console.Write("Vælg handling: ");
            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1": OpretLejer(ctx); break;
                case "2": VisLejer(ctx); break;
                case "3": RedigerLejer(ctx); break;
                case "4": SletLejer(ctx); break;
                case "5": kører = false; break;
                default: Console.WriteLine("Ugyldigt valg!"); break;
            }
        }
    }

    static void OpretLejer(ReolMarkedContext ctx)
    {
        Lejer nyLejer = new();

        Console.Write("Navn: "); nyLejer.Navn = Console.ReadLine() ?? "";
        Console.Write("Email: "); nyLejer.Email = Console.ReadLine() ?? "";
        Console.Write("Telefon: "); nyLejer.Tlf = Console.ReadLine() ?? "";
        Console.Write("Adresse: "); nyLejer.Adresse = Console.ReadLine() ?? "";

        Console.Write("ReolId: ");
        nyLejer.ReolId = int.TryParse(Console.ReadLine(), out var reolId) ? reolId : 0;

        Console.Write("Månedlig Leje: ");
        nyLejer.MånedligLeje = decimal.TryParse(Console.ReadLine(), out var leje) ? leje : 0;

        Console.Write("Indtjent: ");
        nyLejer.Indtjent = decimal.TryParse(Console.ReadLine(), out var indtjent) ? indtjent : 0;

        Console.Write("Betalingsmetode: ");
        nyLejer.Betalingsmetode = Console.ReadLine() ?? "";

        ctx.Lejere.Add(nyLejer);
        ctx.SaveChanges();
        Console.WriteLine("Lejer oprettet!");
    }

    static void VisLejer(ReolMarkedContext ctx)
    {
        var lejere = ctx.Lejere.ToList();
        Console.WriteLine("\n--- Alle Lejere ---");
        foreach (var l in lejere)
        {
            Console.WriteLine($"ID: {l.LejerId} | Navn: {l.Navn} | Email: {l.Email} | Telefon: {l.Tlf} | Adresse: {l.Adresse} | ReolId: {l.ReolId} | Månedlig Leje: {l.MånedligLeje} | Indtjent: {l.Indtjent} | Betalingsmetode: {l.Betalingsmetode}");
        }
    }

    static void RedigerLejer(ReolMarkedContext ctx)
    {
        Console.Write("Indtast LejerId på lejeren du vil redigere: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Ugyldigt ID!");
            return;
        }

        var lejer = ctx.Lejere.FirstOrDefault(l => l.LejerId == id);
        if (lejer == null) { Console.WriteLine("Lejer ikke fundet!"); return; }

        Console.Write($"Navn ({lejer.Navn}): ");
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) lejer.Navn = input;

        Console.Write($"Email ({lejer.Email}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) lejer.Email = input;

        Console.Write($"Telefon ({lejer.Tlf}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) lejer.Tlf = input;

        Console.Write($"Adresse ({lejer.Adresse}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) lejer.Adresse = input;

        Console.Write($"ReolId ({lejer.ReolId}): ");
        input = Console.ReadLine();
        if (int.TryParse(input, out var reolId)) lejer.ReolId = reolId;

        Console.Write($"Månedlig Leje ({lejer.MånedligLeje}): ");
        input = Console.ReadLine();
        if (decimal.TryParse(input, out var leje)) lejer.MånedligLeje = leje;

        Console.Write($"Indtjent ({lejer.Indtjent}): ");
        input = Console.ReadLine();
        if (decimal.TryParse(input, out var indtjent)) lejer.Indtjent = indtjent;

        Console.Write($"Betalingsmetode ({lejer.Betalingsmetode}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) lejer.Betalingsmetode = input;

        ctx.SaveChanges();
        Console.WriteLine("Lejer opdateret!");
    }

    static void SletLejer(ReolMarkedContext ctx)
    {
        Console.Write("Indtast LejerId på lejeren du vil slette: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Ugyldigt ID!");
            return;
        }

        var lejer = ctx.Lejere.FirstOrDefault(l => l.LejerId == id);
        if (lejer == null) { Console.WriteLine("Lejer ikke fundet!"); return; }

        ctx.Lejere.Remove(lejer);
        ctx.SaveChanges();
        Console.WriteLine("Lejer slettet!");
    }
}