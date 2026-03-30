using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrandInspector.Models;

public class Brand
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public BrandConfig BrandConfig { get; set; } = new();
}
