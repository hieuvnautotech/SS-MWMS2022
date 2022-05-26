using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mvc_VD.Models.NewVersion
{
    [Table("author_action")]
    public class AuthorAction
    {
        [Key]
        [Column("id")]
        public int id { get; set; }
        [Column("mn_cd")]
        public string mn_cd { get; set; }
        [Column("url_link")]
        public string url_link { get; set; }
        [Column("id_button")]
        public string id_button { get; set; }
        [Column("type")]
        public string type { get; set; }
        [Column("name_table")]
        public string name_table { get; set; }
        [Column("sts_action")]
        public string sts_action { get; set; }
        [Column("re_mark")]
        public string Rere_markMark { get; set; }
        [Column("active")]
        public bool active { get; set; }
        [Column("create_id")]
        public string create_id { get; set; }
        [Column("create_date")]
        public DateTime create_date { get; set; }
        [Column("[change_id]")]
        public string change_id { get; set; }
        [Column("[change_date]")]
        public DateTime change_date { get; set; }
    }
}