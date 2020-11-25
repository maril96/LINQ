using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LINQ_ToObject
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double UnitPrice { get; set; }




    }
    public class ProductComparer : IEqualityComparer<Product>
    {
        public bool Equals([AllowNull] Product x, [AllowNull] Product y)
        {
            var first = (x as Product);
            var second = (y as Product);
            return first?.Id == second?.Id;
        }

        public int GetHashCode([DisallowNull] Product obj)
        {
            return obj.Id.GetHashCode();
        }
    }

}
