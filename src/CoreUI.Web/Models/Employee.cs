using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreUI.Web.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} requerid")]
        public string Email { get; set; }

        [Display(Name = "Nome")]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "{0} invalid")]
        public string Name { get; set; }

        [Display(Name = "Documento")]
        public string Document { get; set; }

        [Display(Name = "Modo de Contrato")]
        public string Contract_Mode { get; set; }

        [Display(Name = "Status")]
        public int Active { get; set; }

        /* 
        [Required(ErrorMessage = "{0} requerid")]
        [Range(100.0, 50000.0, ErrorMessage = "{0} must be from {1} to {2}")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double Salary { get; set; }
        */

        [Display(Name = "Apontamento")]
        public int Appointment { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} requerid")]

        public string Password { get; set; }

        [Display(Name = "Change Password")]
        public int Change_Password { get; set; }

        public Access_Level Access_Level { get; set; }
        public int Access_LevelId { get; set; }

        public Employee()
        {

        }

        public Employee(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public Employee(int id, string name, string document, string contract_Mode, int active, /*double salary,*/ int appointment, string password, int change_Password, Access_Level access_Level)
        {
            Id = id;
            Name = name;
            Document = document;
            Contract_Mode = contract_Mode;
            Active = active;
            //Salary = salary;
            Appointment = appointment;
            Password = password;
            Change_Password = change_Password;
            Access_Level = access_Level;
        }

    }
}
