﻿using System.ComponentModel;

namespace kurs.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}