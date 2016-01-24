namespace KalashianFamily.Web.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GuestBookMessage")]
    public partial class GuestBookMessage
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(150)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(500)]
        public string Message { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime SignedDate { get; set; }
    }
}
