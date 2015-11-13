using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareAttribute = System.ComponentModel.DataAnnotations.CompareAttribute;
using CheeseShop.Domain.Members;
using Microsoft.Ajax.Utilities;

namespace CheeseShop.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberRegistrationService _registrationService;

        public MemberController(IMemberRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpGet]
        public ActionResult Register()
        {
            var model = new RegisterMemberModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(RegisterMemberModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _registrationService.Register(model.Email,
                                          model.Forename,
                                          model.Surname,
                                          model.Password);

            return RedirectToAction("Confirm", new { model.Email });
        }

        [HttpGet]
        public ActionResult Confirm(string email, int? confirmationCode)
        {
            var model = new ConfirmMemberModel
                {
                    ShowEmail = string.IsNullOrWhiteSpace(email),
                    Email = email,
                    ConfirmationCode = confirmationCode
                };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Confirm(ConfirmMemberModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!_registrationService.Confirm(model.Email, model.ConfirmationCode.Value))
            {
                ModelState.AddModelError("ConfirmationCode", "Unknown Email address or incorrect Confirmation Code");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }
    }

    public class ConfirmMemberModel
    {
        public bool ShowEmail { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, Display(Name = "Confirmation Code")]
        public int? ConfirmationCode { get; set; }
    }

    public class RegisterMemberModel
    {
        private const string Lowercase = "a-z";
        private const string Uppsercase = "A-Z";
        private const string Numbers = "0-9";
        private const string Special = @"`~!@#$%\^&*\(\)_-+=\{\}|\[\]\:"";'<>?,./";

        private const string Pattern =
            @"(?!^[0-9]*$)" +
            @"(?!^[a-z]*$)" +
            @"(?!^[A-Z]*$)" +
            @"^([a-zA-Z0-9 ]+)$";

        [Required, MaxLength(100)]
        public string Email { get; set; }
        [Required, MaxLength(100)]
        public string Forename { get; set; }
        [Required, MaxLength(100)]
        public string Surname { get; set; }
        [Required]
        [MinLength(6, ErrorMessage="The password must be at least six characters.")]
        [RegularExpression(Pattern, ErrorMessage = "The password must contain at least one number, one lowercase and one uppercase character.")]
        public string Password { get; set; }
        [Required, Compare("Password"), Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}