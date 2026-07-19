using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public interface IDiscountable
    {
        decimal ApplyDiscount(decimal amount, decimal discountPercent);
    }
}
