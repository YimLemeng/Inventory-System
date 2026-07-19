using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_pos.Entity
{
    public abstract class BaseModel
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => _id = value;
        }
        public abstract string GetInfo();
    }
}
