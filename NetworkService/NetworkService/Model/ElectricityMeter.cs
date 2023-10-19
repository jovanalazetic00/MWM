using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class ElectricityMeter
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public Type Type { get; set; }

        public double Value { get; set; }

        public ElectricityMeter(int id, string name, string type)
        {
            Id = id;
            Name = name;
            Type = new Type(type);
        }

        public ElectricityMeter()
        {
        }

        public ElectricityMeter(ElectricityMeter electricityMeter)
        {
            this.Id = electricityMeter.Id;
            this.Name = electricityMeter.Name;
            this.Value = electricityMeter.Value;
            this.Type = electricityMeter.Type;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
