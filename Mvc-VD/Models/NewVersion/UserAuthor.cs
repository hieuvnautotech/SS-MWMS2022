using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("user_author")]
    public class UserAuthor
    {
        [Key]
        [Required(ErrorMessage = "User Author Id is required")]
        [Column("id")]
        public int Id { get; set; }

        [Column("userid")]
        public string UserId { get; set; }

        [Column("at_nm")]
        public string AtNM { get; set; }

        [Column("reg_dt")]
        public DateTime CreateDate { get; set; }

        [Column("chg_dt")]
        public DateTime ChangeDate { get; set; }

        [Column("active")]
        public bool Active { get; set; }
    }
}