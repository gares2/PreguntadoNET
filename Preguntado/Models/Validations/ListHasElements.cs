using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Preguntado.Models.Validations
{
    public class ListHasElements : ValidationAttribute
    {
        public override bool IsValid(object mylist)
        {
            if (mylist == null)
                return false;

            return true;
        }
    }
}