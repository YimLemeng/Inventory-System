using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public class Customer : BaseModel
    {
        private string _name;
        private string _phone;
        private string _memberType = "Regular"; 

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name cannot be empty");
                _name = value;
            }
        }
        public string Phone
        {
            get => _phone;
            set => _phone = value;
        }
        public string MemberType
        {
            get => _memberType;
            set => _memberType = value;
        }
        public override string GetInfo() => $"{Name} - {MemberType}";
        public override string ToString() => Name;
    }
}
