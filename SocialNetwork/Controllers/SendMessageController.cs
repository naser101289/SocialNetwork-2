﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SocialNetwork.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SocialNetwork.ViewModels;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class SendMessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SendMessage
        public ActionResult Index()
        {

            //Gets all the users to show in a dropdown list, prints also if the message was sent. 

            var UserNames = db.Users.Select(x => new SelectListItem { Text=x.UserName, Value=x.UserName});
           

            SendMessageViewModel model = new SendMessageViewModel();
           
            
            model.Users = new SelectList(UserNames,"Value","Text");

            var parameter = Request.QueryString["Message"];

            

            if (parameter != null)
            {
                
                ViewBag.Message = parameter; 
                ViewBag.ReturnUrl = Url.Action("Index");
            }
            else
            {
                model.SuccessMessage = null;

            }

            return View(model);
        }

        // POST: SendMessage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "MessageSubject,MessageText, Receiver")] SendMessageViewModel message)
        {
            //Save message to db

            var Sender = db.Users.Find(User.Identity.GetUserId());

            var Receiver = db.Users.SingleOrDefault(recv => recv.UserName.Equals(message.Receiver));
            //var TotalMessages =    //.Where(u => u.Id == Sender.Id).Select(m => m);



            if (ModelState.IsValid)
            {
                Message MessageDB = new Message();

                MessageDB.MessageSubject = message.MessageSubject;
                MessageDB.MessageText = message.MessageText;
                MessageDB.MessageStatus = false;
                MessageDB.MessageTime=DateTime.Now;
                MessageDB.sender = Sender;
                MessageDB.receiver = Receiver;

                var findLoginInfo = db.LoginInfos.Where(i => i.LoginUser.Id == Sender.Id);

                /*UserInfo loginInfo = null;

                if (findLoginInfo.Count() == 0)
                {
                    loginInfo = new UserInfo();
                    loginInfo.TotalMessages += 1;
                    loginInfo.LoginUser = Sender;
                    db.LoginInfos.Add(loginInfo);
                    System.Diagnostics.Debug.WriteLine("if: ");
                }
                else
                {
                    loginInfo = db.LoginInfos.Find(findLoginInfo.SingleOrDefault().LoginInfoID);
                    db.LoginInfos.Attach(loginInfo);
                    loginInfo.TotalMessages += 1;
                    System.Diagnostics.Debug.WriteLine("Else: ");
                }*/


                db.Messages.Add(MessageDB);
                db.SaveChanges();

                

                var TotalMessages = db.LoginInfos.Where(u => u.LoginUser.Id == Sender.Id).Select(m => m.TotalMessages);

                var successMessage = "Message was sent to " + Receiver.UserName + " succesfully!, " + DateTime.Now;
                return RedirectToAction("Index", new { Message = successMessage });
                //return RedirectToAction("Index");
            }

            return View(message);
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
