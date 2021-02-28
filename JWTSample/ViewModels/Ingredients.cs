using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample
{
    public class Ingredients
    {
        int bacon;
        int salad;
        int meat;
        int cheese;

        public int Bacon { get => bacon; set => bacon = value; }
        public int Salad { get => salad; set => salad = value; }
        public int Meat { get => meat; set => meat = value; }
        public int Cheese { get => cheese; set => cheese = value; }
    }
}
