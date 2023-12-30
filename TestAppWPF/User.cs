using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppWPF
{
    class User 
    {
        public User(int Id, string LastName, string FerstName, string MiddleName, string Phone)
        {
            this.Id = Id;
            this.LastName = LastName;
            this.FerstName = FerstName;
            this.MiddleName = MiddleName;
            this.Phone = Phone;
            
        }

        public int Id { get; set; }
        public string LastName { get; set; }
        public string FerstName { get; set; }
        public string MiddleName { get; set; }

        public string Phone {  get; set; }
    }
}
