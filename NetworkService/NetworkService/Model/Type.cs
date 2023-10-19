using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Model
{
    public class Type
    {

        private string _name;
        private string _imgSrc;

        public string Name 
        {
            get => _name;

            set
            {
                if(value == "Interval Meter" || value == "Smart Meter")
                {
                    _name = value;
                } else
                {
                    throw new ArgumentException("Odabrani tip nije odgovarajući. Odgovarajući tipovi su: Interval Meter, Smart Meter");
                }
            }
        }

        public Type(string name)
        {
            _name = name;
             if (name == "Interval Meter")
                _imgSrc = Environment.CurrentDirectory + "\\IntervalMeter.jpg";
            else if (name == "Smart Meter")
                _imgSrc = Environment.CurrentDirectory + "\\SmartMeter.jpg";
            ;
        }

        public Type()
        {
        }
        public string ImgSrc
        {
            get { return _imgSrc; }
          
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
