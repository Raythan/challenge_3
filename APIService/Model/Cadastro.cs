using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace APIService.Model
{
    public class Cadastro
    {
        [JsonIgnore]
        public long ID { get; set; }
        [Required]
        [Description("Obrigatório")]
        public string NOME { get; set; }
        [Required]
        [Description("Obrigatório")]
        public string GENERO { get; set; }
        [Description("Opcional")]
        public string EMAIL { get; set; }
        [Required]
        [Description("Obrigatório")]
        public DateTime DATANASCIMENTO { get; set; }
        [Required]
        [Description("Obrigatório")]
        public string NATURALIDADE { get; set; }
        [Required]
        [Description("Obrigatório")]
        [DefaultValue("Brasileiro")]
        public string NACIONALIDADE { get; set; }
        [Required]
        [Description("Obrigatório - Formato específico, Registro único")]
        public string CPF { get; set; }
    }
}
