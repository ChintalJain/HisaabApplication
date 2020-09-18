namespace DemoHisaab2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction")]
    public partial class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        [StringLength(50)]
        public string CreditAccount { get; set; }

        [StringLength(50)]
        public string DebitAccount { get; set; }

        public int Amount { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public int UserId { get; set; }

        public virtual User_Details User_Details { get; set; }
    }
}
