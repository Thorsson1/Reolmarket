using ReolMarked.Models;
using System.Collections.Generic;

namespace ReolMarked.Repositories;

public interface IProductRepository
{
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    void Add(Product product);
    void Update(Product product);
    void Delete(int id);
    void Save();
}
