using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Preguntado.Models.General;
using Preguntado.Models.Enums;
using Preguntado.Models.Dominio;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Preguntado.Models.Validations;

namespace Preguntado.Models.ViewModel
{
    public class ViewModelAbmUsuario :EntidadViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        public string Password { get; set; }
        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Display(Name = "NickName")]
        public string NickName { get; set; }
        public List<SelectBoxItemViewModel> RolesDisponibles { get; set; }
        
        public List<Guid> RolesSeleccionadas { get; set; }
        //[Required(ErrorMessage = "An Album Title is required")]
        [ListHasElements(ErrorMessage = "List must contain an element")]

        public ViewModelAbmUsuario()
        {
            RolesDisponibles = new List<SelectBoxItemViewModel>();
            RolesSeleccionadas = new List<Guid>();
            Password = "123456";
            ConfirmPassword = "123456";
            LlenarListas();
        }

        public ViewModelAbmUsuario(Usuario usu)
        {
            Nombre = usu.Nombre;
            NickName = usu.NickName;
            Email = usu.ApplicationUser.Email;
            //Password = usu.ApplicationUser.PasswordHash;
            RolesDisponibles = new List<SelectBoxItemViewModel>();
            LlenarListas();
            RolesSeleccionadas = usu.ApplicationUser.Roles.Select(x => new Guid(x.RoleId)).ToList();
            ////////////Password = "123456";
            ////////////ConfirmPassword = "123456";

        }

        public void LlenarListas()
        {
            var db = new ApplicationDbContext();
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            RolesDisponibles = rm.Roles.ToList().Select(s => new SelectBoxItemViewModel()
            {
                Id = new Guid(s.Id),
                Nombre = s.Name
            }).ToList();
        }
    }
}