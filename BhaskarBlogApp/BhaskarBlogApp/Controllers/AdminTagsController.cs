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
using BhaskarBlogApp.Repositories;

namespace BhaskarBlogApp.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        //private BloggieDbContext _bloggieDbContext;

        //public AdminTagsController(BloggieDbContext bloggieDbContext)
        //{
        //    _bloggieDbContext = bloggieDbContext;
        //}

        //as now we will be using repository we will be injecting ITagRepository

        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
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
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
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

            //_bloggieDbContext.Tags.Add(tag);
            //await _bloggieDbContext.Tags.AddAsync(tag);

            ////The SaveChanges method commits the changes to the database:

            ////_bloggieDbContext.SaveChanges();  //Without this line anything cant be saved into the database
            //await _bloggieDbContext.SaveChangesAsync();  

            //Now we are using repository , so every db called will not be done here in controller it will done in repository

            await _tagRepository.AddAsync(tag);
            return RedirectToAction("List");

            //The method takes user input encapsulated in the AddTagRequest object.
            //Maps it to a Tag domain model object.
            //Adds the new tag to the database using _bloggieDbContext.
            //Saves the changes to make the operation permanent.
            //Finally, it renders the "Add" view to the user.
        }
        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            //use DbContext to read the tags
            var tags = await _tagRepository.GetAllAsync();
            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            //1st method
            //var tag = _bloggieDbContext.Tags.Find(id);

            //2nd method
            //var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
            var tag = await _tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,
                };
                return View(editTagRequest); //returns the value 
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName,
            };

            //var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);

            //if (existingTag != null)
            //{
            //    existingTag.Name = tag.Name;
            //    existingTag.DisplayName = tag.DisplayName;

            //    //save changes
            //    await _bloggieDbContext.SaveChangesAsync();

            //    //show success notification
            //    return RedirectToAction("List", new { id = editTagRequest.Id });
            //}

            var updatedTag = await _tagRepository.UpdateAsync(tag);
            if (updatedTag != null)
            {
                //Show success notification
                return RedirectToAction("List", new { id = editTagRequest.Id });
            }
            //show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });

        }
        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
           
            //var existingTag = await _bloggieDbContext.Tags.FindAsync(editTagRequest.Id);

            //if (existingTag != null)
            //{
            //    _bloggieDbContext.Tags.Remove(existingTag);
            //    //save changes
            //    await _bloggieDbContext.SaveChangesAsync();

            //    //show success notification
            //    return RedirectToAction("List");
            //}


            var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                //show success notification
                return RedirectToAction("List");
            }
            //show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });

        }
        
    }
}
