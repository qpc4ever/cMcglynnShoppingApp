using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace cMcglynnShoppingApp.Models
{
    public class Universal : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            if (User.Identity.IsAuthenticated)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.FullName = user.FullName;
                ViewBag.CartItems = user.CartItems;
                ViewBag.TotalCartItems =db.CartItems.Where(c => c.CustomerId == user.Id).ToList();
                ViewBag.TotalCartItems = user.CartItems.Sum(c => c.Count);
                decimal Total = 0;
                foreach (var item in user.CartItems)
                {
                    Total += item.Count * item.Item.Price;  // ADDS UP THE TOTAL OF CART ITEMS
                        
                }
                ViewBag.CartTotal = Total;

                base.OnActionExecuted(filterContext);

            }
        }
    }
}
