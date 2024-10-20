﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceApp.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }
        public string Country { get; set; }
        [JsonIgnore]
        public ICollection<Invoice> Invoices { get; set; }
        [JsonIgnore]
        public ICollection<Customer> Customers { get; set; }
    }
}
