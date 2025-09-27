using ReolMarked.Models;
using System.Collections.Generic;

namespace ReolMarked.Repositories;

public interface IReolRepository
{
    IEnumerable<Reol> GetAll();
    Reol? GetById(int id);
    void Add(Reol reol);
    void Update(Reol reol);
    void Delete(int id);
    void Save();
}
