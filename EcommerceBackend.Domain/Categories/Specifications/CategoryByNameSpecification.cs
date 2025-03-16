using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceBackend.Domain.Categories.Specifications
{
    public class CategoryByNameSpecification
    {
        private readonly string _name;

        public CategoryByNameSpecification(string name)
        {
            _name = name;
        }

        public Expression<Func<Category, bool>> ToExpression()
        {
            return category => category.Name == _name;
        }
    }
}
