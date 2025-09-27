using ReolMarked.Data;
using ReolMarked.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReolMarked.Repositories;

public class ReolRepository : IReolRepository
{
    private readonly ReolMarkedContext _context;

    public ReolRepository(ReolMarkedContext context)
    {
        _context = context;
    }

    public IEnumerable<Reol> GetAll() => _context.Reoler.ToList();

    public Reol? GetById(int id) => _context.Reoler.FirstOrDefault(r => r.ReolId == id);

    public void Add(Reol reol) => _context.Reoler.Add(reol);

    public void Update(Reol reol) => _context.Reoler.Update(reol);

    public void Delete(int id)
    {
        var reol = GetById(id);
        if (reol != null) _context.Reoler.Remove(reol);
    }

    public void Save() => _context.SaveChanges();
}
