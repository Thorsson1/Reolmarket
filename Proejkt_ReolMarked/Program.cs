using System;
using System.Linq;
using ReolMarked.Data;
using ReolMarked.Models;

class Program
{
    static void Main()
    {
        using var ctx = new ReolMarkedContext();

        bool kører = true;
        while (kører)
        {
            Console.WriteLine("\n--- ReolMarked Menu ---");
            Console.WriteLine("1. Vis alle reoler");
            Console.WriteLine("2. Tilføj reol");
            Console.WriteLine("3. Rediger reol");
            Console.WriteLine("4. Slet reol");
            Console.WriteLine("5. Afslut");
            Console.Write("Vælg handling: ");
            string valg = Console.ReadLine() ?? "";

            switch (valg)
            {
                case "1":
                    VisReoler(ctx);
                    break;
                case "2":
                    TilføjReol(ctx);
                    break;
                case "3":
                    RedigerReol(ctx);
                    break;
                case "4":
                    SletReol(ctx);
                    break;
                case "5":
                    kører = false;
                    break;
                default:
                    Console.WriteLine("Ugyldigt valg!");
                    break;
            }
        }
    }

    static void VisReoler(ReolMarkedContext ctx)
    {
        var reoler = ctx.Reoler.ToList();
        Console.WriteLine("\n--- Alle Reoler ---");
        foreach (var r in reoler)
        {
            Console.WriteLine($"ID: {r.ReolId} | Nr: {r.ReolNummer} | Status: {r.Status} | Pris: {r.Pris} | Type: {r.ReolType} | Lejer: {r.ReolLejer ?? "Ingen"} | Placering: {r.Placering}");
        }
    }

    static void TilføjReol(ReolMarkedContext ctx)
    {
        var nyReol = new Reol();

        Console.Write("ReolNummer: ");
        nyReol.ReolNummer = Console.ReadLine() ?? "";

        Console.Write("Placering: ");
        nyReol.Placering = Console.ReadLine() ?? "";

        Console.Write("Status: ");
        nyReol.Status = Console.ReadLine() ?? "Ledig";

        Console.Write("ReolType: ");
        nyReol.ReolType = Console.ReadLine() ?? "";

        Console.Write("Pris: ");
        nyReol.Pris = decimal.TryParse(Console.ReadLine(), out var pris) ? pris : 0;

        Console.Write("ReolLejer (kan være tom): ");
        nyReol.ReolLejer = string.IsNullOrWhiteSpace(Console.ReadLine()) ? null : Console.ReadLine();

        ctx.Reoler.Add(nyReol);
        ctx.SaveChanges();
        Console.WriteLine("Ny reol oprettet!");
    }

    static void RedigerReol(ReolMarkedContext ctx)
    {
        Console.Write("Indtast ReolId på reolen du vil redigere: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Ugyldigt ID!");
            return;
        }

        var reol = ctx.Reoler.FirstOrDefault(r => r.ReolId == id);
        if (reol == null)
        {
            Console.WriteLine("Reol ikke fundet!");
            return;
        }

        Console.Write($"ReolNummer ({reol.ReolNummer}): ");
        var input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) reol.ReolNummer = input;

        Console.Write($"Placering ({reol.Placering}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) reol.Placering = input;

        Console.Write($"Status ({reol.Status}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) reol.Status = input;

        Console.Write($"ReolType ({reol.ReolType}): ");
        input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) reol.ReolType = input;

        Console.Write($"Pris ({reol.Pris}): ");
        input = Console.ReadLine();
        if (decimal.TryParse(input, out var pris)) reol.Pris = pris;

        Console.Write($"ReolLejer ({reol.ReolLejer ?? "Ingen"}): ");
        input = Console.ReadLine();
        reol.ReolLejer = string.IsNullOrWhiteSpace(input) ? null : input;

        ctx.SaveChanges();
        Console.WriteLine("Reol opdateret!");
    }

    static void SletReol(ReolMarkedContext ctx)
    {
        Console.Write("Indtast ReolId på reolen du vil slette: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.WriteLine("Ugyldigt ID!");
            return;
        }

        var reol = ctx.Reoler.FirstOrDefault(r => r.ReolId == id);
        if (reol == null)
        {
            Console.WriteLine("Reol ikke fundet!");
            return;
        }

        ctx.Reoler.Remove(reol);
        ctx.SaveChanges();
        Console.WriteLine("Reol slettet!");
    }
} //testing1