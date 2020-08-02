using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Cadastro
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o nome do usuário", AllowEmptyStrings = false)]
        [Description("Obrigatório")]
        public string NOME { get; set; }
        [Display(Name = "Gênero")]
        [Required(ErrorMessage = "Informe o gênero do usuário", AllowEmptyStrings = false)]
        [Description("Obrigatório")]
        public string GENERO { get; set; }
        [Display(Name = "Email")]
        [Description("Opcional")]
        public string EMAIL { get; set; }
        [Display(Name = "Data de Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime), Required(ErrorMessage = "Informe a data de nascimento do usuário")]
        [Description("Obrigatório")]
        public DateTime DATANASCIMENTO { get; set; }
        [Display(Name = "Naturalidade")]
        [Required(ErrorMessage = "Informe a naturalidade do usuário", AllowEmptyStrings = false)]
        [Description("Obrigatório")]
        public string NATURALIDADE { get; set; }
        [Display(Name = "Nacionalidade")]
        [Required(ErrorMessage = "Informe a nacionalidade do usuário", AllowEmptyStrings = false)]
        [Description("Obrigatório")]
        [DefaultValue("Brasileiro")]
        public string NACIONALIDADE { get; set; }
        [Display(Name = "CPF")]
        [Required(ErrorMessage = "Informe o CPF do usuário", AllowEmptyStrings = false)]
        [Description("Obrigatório - Formato específico, Registro único")]
        public string CPF { get; set; }
    }
}