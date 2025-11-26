using System.Text.Json.Serialization;

namespace WL_Server.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
//using System.Web.Mvc.Ajax;


//MODEL CLASS FOR USER
//THIS WILL STRUCTURE THE DATA OF THE USER, ANY APP OR BUSINESS LOGIC FOR THE USER WILL REFER BACK TO THIS MODEL
public class User
{
    //User ID UUID INT NOT NULL
    public int ID { get; set; }
    //User email  STRING NOT NULL
    public string? Email { get; set; }
    //User username STRING NOT NULL
    public string? Username { get; set; }
    //User password STRING NOT NULL
    public string? Password { get; set; }
    //User dateOfBirth DATE
    public DateTime? DateOfBirth { get; set; }
    
    
}