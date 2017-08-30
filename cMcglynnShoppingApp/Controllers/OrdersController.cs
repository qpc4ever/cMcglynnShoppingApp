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
    public class OrdersController : Universal
    {
        

        // GET: Orders
        public ActionResult Index()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            return View(db.Orders.ToList());
        }
        
        // GET: Orders/Details/5
        public ActionResult Details(int? id, bool? justCompleted)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            if (justCompleted != null && justCompleted == true)
            {
                ViewBag.JustCompleted = true;
            }
            else
            {
                ViewBag.JustCompleted = false;
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId,OrderDetails")] Order order, decimal total)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(User.Identity.GetUserId());     
                order.CustomerId = user.Id;
                order.OrderDate = System.DateTime.Now;
                order.Total = total;
                db.Orders.Add(order);
                db.SaveChanges();                                 //GENERATES THE ID OF THE ORDER 
                foreach (var item in user.CartItems.ToList())
                {
                    OrderItem orderitem = new OrderItem();      //CREATING A NEW ORDER ITEM FOR EACH CART ITEM
                    orderitem.ItemId = item.ItemId;
                    orderitem.OrderId = order.Id;              //ORDER GETS AN ID WHEN IT GETS SAVED TO DATABASE "db.SaveChanges();"
                    orderitem.Quantity = item.Count;
                    orderitem.UnitPrice = item.Item.Price;
                    db.OrederItems.Add(orderitem);
                    db.CartItems.Remove(item);                   //REMOVES ITEMS FROM CART ONCE THEY'RE ENTERED INTO ORDER
                    db.SaveChanges();                           // SAVES ALL CHANGES TO DATABASE
                }
                
                
                return RedirectToAction("Details", new { id = order.Id, justCompleted = true });
            }

            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId,OrderDetails")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
