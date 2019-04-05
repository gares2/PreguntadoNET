using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Preguntado.Models.ViewModel.Api
{
    public class ApiCategoriasElement
    {
        //public string category { get; set; }
        private string Question;

        private string Category { get; set; }

        public string category
        {
            get
            {
                return Category;
            }
            set
            {
                Category = HttpUtility.UrlDecode(value); //.Replace("%20"," ");
            }
        }


        public string question
        {
            get
            {
                return Question;
            }
            set
            {
                Question = HttpUtility.UrlDecode(value); //.Replace("%20"," ");
            }
        }

        public string correct_answer
        {
            get
            {
                return Correct_answer;
            }
            set
            {
                Correct_answer = HttpUtility.UrlDecode(value); //.Replace("%20"," ");
            }
        }

        private string Correct_answer { get; set; }

        private List<string> Incorrect_answers { get; set; }

        public List<string> incorrect_answers
        {
            get
            {
                return Incorrect_answers;
            }
            set
            {

                //List<string> list = new List<string>();
                //for (int i = 0; i < value.Count; i++)
                //{
                //    list[i] = HttpUtility.UrlDecode(value[i]);
                //}

                Incorrect_answers = value.Select(x => HttpUtility.UrlDecode(x.ToString())).ToList();  //.Replace("%20"," ");
            }
        }

    }
}