using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class Login
    {
        [Display(Name = "Usuário")]
        [Required(ErrorMessage = "Informe o usuário.", AllowEmptyStrings = false)]
        [Description("Obrigatório")]
        public string USUARIO { get; set; }
        [Display(Name = "Senha")]
        [Required(ErrorMessage = "Informe a senha.", AllowEmptyStrings = false)]
        [Description("Obrigatório")]
        public string SENHA { get; set; }
        public bool IsValid { get; private set; } = false;
        public void SetIsValid(bool parm)
        {
            this.IsValid = parm;
        }
    }
}