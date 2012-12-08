using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sample.WebForms_NET35.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {            
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            var registerUser = sender as CreateUserWizard;
            FormsAuthentication.SetAuthCookie(registerUser.UserName, false /* createPersistentCookie */);

            string continueUrl = registerUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);            
        }

        protected void RegisterUser_Init(object sender, EventArgs e)
        {
            var registerUser = sender as CreateUserWizard;
            registerUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }
    }
}
