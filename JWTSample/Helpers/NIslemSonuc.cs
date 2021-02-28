using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VakifIlan
{
    public class NIslemSonuc<T>
    {
        public bool BasariliMi { get; set; }
        public T Veri { get; set; }
        public string Mesaj { get; set; }
        public NHata HataBilgi { get; set; }
    }
}