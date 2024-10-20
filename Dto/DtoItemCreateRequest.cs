﻿namespace InvoiceApp.Dto
{
    public class DtoItemCreateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int InvoiceId { get; set; }
    }
}