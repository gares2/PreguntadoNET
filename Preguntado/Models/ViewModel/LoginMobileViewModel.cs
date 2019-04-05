using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Preguntado.Models.Dominio;
using Preguntado.Models.General;
using System.ComponentModel.DataAnnotations;
namespace Preguntado.Models.ViewModel
{
    public class LoginMobileViewModel : EntidadViewModel
    {
      
        public string user { get; set; }

        public string IdUser { get; set; }
        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string password { get; set; }

        public string token { get; set; }

        public string NickName { get; set; }

    }
}