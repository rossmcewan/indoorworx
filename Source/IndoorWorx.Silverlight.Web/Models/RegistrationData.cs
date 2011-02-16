namespace IndoorWorx.Silverlight.Web
{
    using System.ComponentModel.DataAnnotations;
    using IndoorWorx.Silverlight.Web.Resources;
    using System;
    using IndoorWorx.Infrastructure.Models;
    using IndoorWorx.Infrastructure.Enums;

    /// <summary>
    /// Class containing the values and validation rules for user registration.
    /// </summary>
    public sealed partial class RegistrationData
    {
        /// <summary>
        /// Gets and sets the user name.
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 0, Name = "UserNameLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessageResourceName = "ValidationErrorInvalidUserName", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "ValidationErrorBadUserNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string UserName { get; set; }

        /// <summary>
        /// Gets and sets the friendly name of the user.
        /// </summary>
        [Display(Order = 1, Name = "FriendlyNameLabel", Description = "FriendlyNameDescription", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(255, MinimumLength = 0, ErrorMessageResourceName = "ValidationErrorBadFriendlyNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets and sets the first name.
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 2, Name = "FirstNameLabel", ResourceType = typeof(RegistrationDataResources))]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets and sets the last name.
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 3, Name = "LastNameLabel", ResourceType = typeof(RegistrationDataResources))]
        public string LastName { get; set; }

        [Key]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 4, Name = "GenderLabel", ResourceType = typeof(RegistrationDataResources))]
        public Genders Gender { get; set; }


        /// <summary>
        /// Gets and sets the last name.
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 5, Name = "DateOfBirthLabel", ResourceType = typeof(RegistrationDataResources))]
        public DateTime DateOfBirth { get; set; }


        /// <summary>
        /// Gets and sets the last name.
        /// </summary>
        [Key]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 6, Name = "CountryLabel", ResourceType = typeof(RegistrationDataResources))]
        public Countries Country { get; set; }


        [Key]
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 7, Name = "EmailLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                           ErrorMessageResourceName = "ValidationErrorInvalidEmail", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Email { get; set; }

        ///// <summary>
        ///// Gets and sets the security question.
        ///// </summary>
        //[Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        //[Display(Order = 5, Name = "SecurityQuestionLabel", ResourceType = typeof(RegistrationDataResources))]
        //public string Question { get; set; }

        ///// <summary>
        ///// Gets and sets the answer to the security question.
        ///// </summary>
        //[Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        //[Display(Order = 6, Name = "SecurityAnswerLabel", ResourceType = typeof(RegistrationDataResources))]
        //public string Answer { get; set; }
    }
}
