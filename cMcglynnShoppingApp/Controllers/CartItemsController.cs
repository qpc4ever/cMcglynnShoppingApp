using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cMcglynnShoppingApp.Models;
using cMcglynnShoppingApp.Models.CodeFirst;
using Microsoft.AspNet.Identity;

namespace cMcglynnShoppingApp.Controllers
{
    public class CartItemsController : Universal
    {
        

        // GET: CartItems      
        [Authorize]
        public ActionResult Index()
        {   
            var user = db.Users.Find(User.Identity.GetUserId());// GRABS THE CURRENT USER OBJECT
            return View(user.CartItems.ToList());// THIS WILL DISPLAY ONLY THE USERS CART ITEMS
        }

        // GET: CartItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // GET: CartItems/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // id taken from id = item.id from Items index page
        public ActionResult Create(int? id)
            // BEGINING OF WHAT WE DID IN SCHOOL
        {   // to make sure its a good request
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            // this is the actual code that adds the items to the cart
            var user = db.Users.Find(User.Identity.GetUserId());

            if (user.CartItems.Any(c => c.ItemId == id))
            {
                var existingCartItem = user.CartItems.FirstOrDefault(c => c.ItemId == id);
                existingCartItem.Count += 1;
                db.SaveChanges();
            }
            else
            {
                CartItem cartitem = new CartItem();
                cartitem.Count = 1;
                cartitem.ItemId = id.Value;
                cartitem.created = System.DateTime.Now;
                cartitem.CustomerId = user.Id;
                db.CartItems.Add(cartitem);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Items");          
        }
        // END OF WHAT WE ACTUALLY DID IN SCHOOL TO ADD ITEMS TO CART
        // GET: CartItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemId,CustomerId,Count,created,MediaURL")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {

                db.Entry(cartItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartItem = db.CartItems.Find(id);
            if (cartItem == null)
            {
                return HttpNotFound();
            }
            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartItem = db.CartItems.Find(id);
            db.CartItems.Remove(cartItem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
