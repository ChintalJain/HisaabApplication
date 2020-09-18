namespace DemoHisaab2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_Accounts
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name ="Account Name :- ")]
        public string AccountName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Account Type :- ")]
        public string AccountType { get; set; }
        [Display(Name = "Total Amount :- ")]
        public int TotalAmount { get; set; }

        public int UserId { get; set; }

        public virtual User_Details User_Details { get; set; }
    }
}
