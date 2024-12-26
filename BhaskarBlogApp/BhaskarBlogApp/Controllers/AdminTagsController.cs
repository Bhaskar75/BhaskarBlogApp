using Azure;
using System.Threading.Channels;
using BhaskarBlogApp.Data;
using BhaskarBlogApp.Models.Domain;
using BhaskarBlogApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;

namespace BhaskarBlogApp.Controllers
{
    public class AdminTagsController : Controller
    {
        private BloggieDbContext _bloggieDbContext;

        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            //note the name of the view needs to be same as the method the reason 
            //is as below return View() we have mentioned nothing ,
            //so it will look to the name of the method
            //and then it will find the name of the view
            //which should be same as the name of the method
            //if the name of the view is not same as the name of the the method 
            //then the name of view should be mentioned inside view() method for e.g return View("Add Something");
            //Even the name of the folder of this view will be same as the name before the word Controller
            //the name of the folder is AdminTags which is same as the the name AdmintagsController
            //The name of the folder can be different as we have created the view using shortcut it will set the name of the folder as the controller name
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            //var name = Request.Form["name"];
            //var displayName = Request.Form["displayName"];
            //var name = addTagRequest.Name;
            //var diplsayName = addTagRequest.DisplayName;

            //Mapping the add tag request to tag domain model
            //A new instance of the Tag domain model is created:
            //This maps the incoming request data(Name and DisplayName) to the corresponding fields in the Tag domain model. The domain model is typically the structure used to interact with the database.
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };

            //The newly created tag object is added to the Tags DbSet of the _bloggieDbContext
            //The Tags DbSet represents a table in the database, and the Add method queues the tag object to be inserted into the database.

            _bloggieDbContext.Tags.Add(tag);

            //The SaveChanges method commits the changes to the database:
        
            _bloggieDbContext.SaveChanges();  //Without this line anything cant be saved into the database
            List();
            return View("List");

            //The method takes user input encapsulated in the AddTagRequest object.
            //Maps it to a Tag domain model object.
            //Adds the new tag to the database using _bloggieDbContext.
            //Saves the changes to make the operation permanent.
            //Finally, it renders the "Add" view to the user.
        }
        [HttpGet]
        public IActionResult List()
        {
            //use DbContext to read the tags
            var tags = _bloggieDbContext.Tags.ToList();
            return View(tags);
        }
    }
}
