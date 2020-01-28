using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class UsersAdminController : Controller
    {
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, 
            ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // Add the Group Manager (NOTE: only access through the public
        // Property, not by the instance variable!)
        private ApplicationGroupManager _groupManager;
        public ApplicationGroupManager GroupManager
        {
            get
            {
                return _groupManager ?? new ApplicationGroupManager();
            }
            private set
            {
                _groupManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext()
                    .Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

       
        public async Task<ActionResult> Index()
        {
            // int id = orgid(User.Identity.GetUserId());

            //Get Users
             var userslist = await UserManager.Users.ToListAsync();


            //Get Groups for looged user
            var user = UserManager.FindByName(User.Identity.Name);
            string id = user.Id;
            var loggeduser = UserManager.FindByIdAsync(id);
            var userGroups = this.GroupManager.GetUserGroups(id);
            string org = userGroups.FirstOrDefault().Org.ToString();

            //if (org != "1")
            //{
            //    var final = userslist.Where(a => a.PhoneNumber == org).ToList<ApplicationUser>();
            //    return View(final);
            //}
            //else
            //{
                var final1 = userslist.ToList<ApplicationUser>();
                return View(final1);
            //}

            // var Final = userslist.Select(u => u.UserName).ToList();
            //var final = userslist.Where(a => a.PhoneNumber == org).ToList<ApplicationUser>();

            //List<ApplicationGroup> groups = new List<ApplicationGroup>();

            //List<ApplicationUser> Userlist = new List<ApplicationUser>();
            //IEnumerable<ApplicationUser> Userlist1 = new List<ApplicationUser>();


            //Userlist1 =Final.Select(Item => new ApplicationUser
            //{
                
            //    UserName = Item.ToString(),
            //}).ToList();
            //foreach (var group in userGroups)
            //{

            //    Userlist1= GroupManager.GetGroupUsers(group.Id);


            //}

           
        }


        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            // Show the groups the user belongs to:
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);
            ViewBag.GroupNames = userGroups.Select(u => u.Name).ToList();
            return View(user);
        }


        public ActionResult Create()
        {
           
            // Show a list of available groups:
            //ViewBag.GroupsList =
            //    new SelectList(this.GroupManager.Groups, "Id", "Name");

            var user = UserManager.FindByName(User.Identity.Name);
            string id = user.Id;
            var loggeduser = UserManager.FindByIdAsync(id);
            var userGroups = this.GroupManager.GetUserGroups(id);
            List<ApplicationGroup> item = new List<ApplicationGroup>();
            item = userGroups.ToList();
            ViewBag.GroupsList =
                new SelectList(item, "Id", "Name");
            ViewBag.Org = item.FirstOrDefault().Org.ToString();

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, 
            params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    UserName = userViewModel.Email, 
                    Email = userViewModel.Email, 
                    Fname=userViewModel.Fname,
                    PhoneNumber=userViewModel.org_id,
                    Lname=userViewModel.Lname
                   
                };
                var adminresult = await UserManager
                    .CreateAsync(user, userViewModel.Password);

                //Add User to the selected Groups 
                if (adminresult.Succeeded)
                {
                    if (selectedGroups != null)
                    {
                        selectedGroups = selectedGroups ?? new string[] { };
                        await this.GroupManager
                            .SetUserGroupsAsync(user.Id, selectedGroups);
                    }

                    var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    //   await UserManager.SendEmailAsync(user.Id, "gDGS Account Created", "<b>Dear User</b></br>"+"An account has been created for you on Global Document Generation System gDGS.<br/>"+"Username:"+ user.Id +"<br/>"+ "Please follow this link in order to set your password and activate your account: <a href=\"" + callbackUrl + "\">link</a>");
                    await UserManager.SendEmailAsync(user.Id, "gDGS Account Created", "<b>Dear"+ user.Fname+" "+user.Lname+ "</b><br /><br/>" + "An account has been created for you on Global Document Generation System gDGS.<br/> <br/>" + "<b>Username:</b>" + user.UserName + "<br/>" +"<b>Password:</b> United@12345 <br/>" +"Access Link : http://conf.unog.ch/GDGS <br/><br/>+"+ "Best regards,<br/>"+"gDGS Team");
                    ViewBag.Link = callbackUrl;

                    return View("ConfirmUserCreation", user);

                    // return RedirectToAction("Index");
                }
               // else { AddErrors(adminresult); }
            }

            ViewBag.Groups = new SelectList(
                await RoleManager.Roles.ToListAsync(), "Id", "Name");
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            throw new NotImplementedException();
            //foreach (var error in result.Errors)
            //{
            //    ModelState.AddModelError("", error);
            //}
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userlooged = UserManager.FindByName(User.Identity.Name);
            string idlooged = userlooged.Id;
            var loggeduser = UserManager.FindByIdAsync(idlooged);
            var allGroups = this.GroupManager.GetUserGroups(idlooged);

            // Display a list of available Groups:
           // var allGroups = this.GroupManager.Groups;
            var userGroups = await this.GroupManager.GetUserGroupsAsync(id);

            var model = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Fname=user.Fname,
                Lname =user.Lname
            };

            foreach (var group in allGroups)
            {
                var listItem = new SelectListItem()
                {
                    Text = group.Name,
                    Value = group.Id,
                    Selected = userGroups.Any(g => g.Id == group.Id)
                };
                model.GroupsList.Add(listItem);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Email,Id,Fname,Lname")] EditUserViewModel editUser, 
            params string[] selectedGroups)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Update the User:
                user.UserName = editUser.Email;
                user.Email = editUser.Email;
                user.Fname = editUser.Fname;
                user.Lname = editUser.Lname;
                await this.UserManager.UpdateAsync(user);

                // Update the Groups:
                selectedGroups = selectedGroups ?? new string[] { };
                await this.GroupManager.SetUserGroupsAsync(user.Id, selectedGroups);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }


        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                // Remove all the User Group references:
                await this.GroupManager.ClearUserGroupsAsync(id);

                // Then Delete the User:
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
