using StudentAffairs.Authorization;
using StudentAffairs.Enums;
using StudentAffairs.Models;
using StudentAffairs.Services.Users;
using StudentAffairs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace StudentAffairs.Controllers
{
    [Authorized(ScreenId = "24")]
    public class UsersController : Controller
    {
        UsersServices usersServices = new UsersServices();
        //EmployeesServices employeesServices = new EmployeesServices();
        // GET: Users
        public ActionResult Index(Guid? SchoolId)
        {
            if ((Role)TempData["RoleId"] == Role.Super_Admin && SchoolId == null)
            {
                return View(new List<User>());
            }
            var users = usersServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["SchoolId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            if (SchoolId != null)
                users = users.Where(x => x.SchoolId == SchoolId).ToList();
            return View(users);
        }
        public ActionResult Create()
        {
            //ViewBag.Employees = employeesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            TreeFunction();
            return View("Upsert", new User());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(User user, string selectedItems, string IsAdmin)
        {
            string pages = ",0,";
            user.Id = Guid.NewGuid();
            user.RoleId = (int)Role.School_Admin;

            List<TreeViewNode> items = (new JavaScriptSerializer()).Deserialize<List<TreeViewNode>>(selectedItems);
            if (items != null)
            {
                foreach (var item in items)
                {
                    var id = int.Parse(item.id);

                    pages += item.id + ",";

                }
            }

            user.UserScreens = pages;
            //if (IsAdmin == "on")
            //    user.RoleId = (int)Role.SystemAdmin;
            //else
            //    user.RoleId = (int)Role.Employee;

            var result = usersServices.Create(user, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                user.Id = Guid.Empty;
                ViewBag.SchoolId = user.SchoolId;
                //ViewBag.Employees = employeesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                TreeFunction();
                TempData["warning"] = result.Message;
                return View("Upsert", user);
            }
        }

        public ActionResult Edit(Guid Id)
        {
            var user = usersServices.Get(Id);
            //if (user.RoleId == 1)
            //    ViewBag.IsAdmin = "checked";

            user.Password = Security.Decrypt(user.Password);
            ViewBag.SchoolId = user.SchoolId;

            //ViewBag.Employees = employeesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
            SelectedTreeFunction(user.UserScreens);
            return View("Upsert", user);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult Edit(User user, string selectedItems, string IsAdmin)
        {
            string pages = ",0,";

            List<TreeViewNode> items = (new JavaScriptSerializer()).Deserialize<List<TreeViewNode>>(selectedItems);
            if (items != null)
            {
                foreach (var item in items)
                {
                    var id = int.Parse(item.id);
                    
                        pages += item.id + ",";
                    
                }
            }

            user.UserScreens = pages;
            user.RoleId = (int)Role.School_Admin;

            //if (IsAdmin=="on")
            //    user.RoleId = (int)Role.SystemAdmin;
            //else
            //    user.RoleId = (int)Role.Employee;
            var result = usersServices.Edit(user, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SchoolId = user.SchoolId;

                //user.Password = Security.Decrypt(user.Password);
                //ViewBag.Employees = employeesServices.GetAll((Guid)TempData["UserId"], (Guid)TempData["EmployeeId"], (Role)TempData["RoleId"]);
                SelectedTreeFunction(user.UserScreens);
                TempData["warning"] = result.Message;
                return View("Upsert", user);
            }
        }
        public ActionResult Delete(Guid Id)
        {
            var result = usersServices.Delete(Id, (Guid)TempData["UserId"]);
            if (result.IsSuccess)
            {
                TempData["success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["warning"] = result.Message;
                return RedirectToAction("Index");
            }
        }

        public void TreeFunction()
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();
            var pagesPerants = usersServices.GetAllPerants();
            var pagesChilds = usersServices.GetAllChilds();
            if ((Guid)TempData["SchoolId"] != null && (Guid)TempData["SchoolId"] != Guid.Empty)
            {
                pagesPerants=pagesPerants.Where(x => x.Id != 24).ToList();
                pagesChilds=pagesChilds.Where(x => x.Id != 24).ToList();
            }
            //Loop and add the Parent Nodes.
            foreach (var item in pagesPerants)
            {
                nodes.Add(new TreeViewNode { id = item.Id.ToString(), parent = "#", text = item.Name, state = new { opened = false, selected = false } });
            }

            //Loop and add the Child Nodes.
            foreach (var item in pagesChilds)
            {
                nodes.Add(new TreeViewNode { id = item.ParentId.ToString() + "-" + item.Id.ToString(), parent = item.ParentId.ToString(), text = item.Name, state = new { opened = false, selected = false } });
            }


            //Serialize to JSON string.
            ViewBag.Json = (new JavaScriptSerializer()).Serialize(nodes);
        }
        public void SelectedTreeFunction(string user)
        {
            List<TreeViewNode> nodes = new List<TreeViewNode>();
            List<TreeViewNode> Selectednodes = new List<TreeViewNode>();

            var pagesPerants = usersServices.GetAllPerants();
            var pagesChilds = usersServices.GetAllChilds();
            if((Guid)TempData["SchoolId"]!=null&& (Guid)TempData["SchoolId"] !=Guid.Empty)
            {
                pagesPerants = pagesPerants.Where(x => x.Id != 24).ToList();
                pagesChilds = pagesChilds.Where(x => x.Id != 24).ToList();

            }
            //Loop and add the Parent Nodes.
            foreach (var item in pagesPerants)
            {
                if (user.Contains("," + item.Id + ","))
                {
                    nodes.Add(new TreeViewNode { id = item.Id.ToString(), parent = "#", text = item.Name, state = new { opened = true, selected = true } });
                    Selectednodes.Add(new TreeViewNode { id = item.Id.ToString(), parent = "#", text = item.Name, state = new { opened = false, selected = false } });

                }
                else
                {
                    nodes.Add(new TreeViewNode { id = item.Id.ToString(), parent = "#", text = item.Name, state = new { opened = false, selected = false } });
                }
            }

            //Loop and add the Child Nodes.
            foreach (var item in pagesChilds)
            {
                if (user.Contains("," + item.Id + ","))
                {
                    nodes.Add(new TreeViewNode { id = item.ParentId.ToString() + "-" + item.Id.ToString(), parent = item.ParentId.ToString(), text = item.Name, state = new { opened = true, selected = true } });
                    Selectednodes.Add(new TreeViewNode { id = item.Id.ToString(), parent = item.ParentId.ToString(), text = item.Name, state = new { opened = false, selected = false } });
                }
                else
                {
                    nodes.Add(new TreeViewNode { id = item.ParentId.ToString() + "-" + item.Id.ToString(), parent = item.ParentId.ToString(), text = item.Name, state = new { opened = false, selected = false } });
                }
            }


            //Serialize to JSON string.
            ViewBag.Json = (new JavaScriptSerializer()).Serialize(nodes);
            ViewBag.JsonSelected = (new JavaScriptSerializer()).Serialize(Selectednodes);
        }
    }
    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public object state { get; set; }

    }
}