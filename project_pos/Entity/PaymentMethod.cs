using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class PaymentMethod : BaseModel
    {
        private string _methodName;

        public string MethodName
        {
            get => _methodName;
            set => _methodName = value;
        }

        public override string GetInfo() => MethodName;

        public override string ToString() => MethodName;
    }
}
