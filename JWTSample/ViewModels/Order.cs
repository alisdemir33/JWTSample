using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JWTSample.ViewModels
{
    public class Order
    {
        Ingredients ingredients;
        OrderData orderData;
        double price;

        public Ingredients Ingredients { get => ingredients; set => ingredients = value; }
        public OrderData OrderData { get => orderData; set => orderData = value; }
        public double Price { get => price; set => price = value; }
    }


    public class OrderData {

        string country;
        string deliveryMethod;
        string email;
        string name;
        string street;
        string zipcode;

        public string Country { get => country; set => country = value; }
        public string DeliveryMethod { get => deliveryMethod; set => deliveryMethod = value; }
        public string Email { get => email; set => email = value; }
        public string Name { get => name; set => name = value; }
        public string Street { get => street; set => street = value; }
        public string Zipcode { get => zipcode; set => zipcode = value; }
    }

}
