using ReolMarked.Data;
using ReolMarked.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReolMarked.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ReolMarkedContext _context;

    public ProductRepository(ReolMarkedContext context)
    {
        _context = context;
    }

    public IEnumerable<Product> GetAll() => _context.Products.ToList();

    public Product? GetById(int id) => _context.Products.FirstOrDefault(p => p.ProductId == id);

    public void Add(Product product) => _context.Products.Add(product);

    public void Update(Product product) => _context.Products.Update(product);

    public void Delete(int id)
    {
        var product = GetById(id);
        if (product != null) _context.Products.Remove(product);
    }

    public void Save() => _context.SaveChanges();
}
