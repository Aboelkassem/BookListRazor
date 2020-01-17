using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookListRazor.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BookListRazor.Pages.BookList
{
    // This Page For Both Update and Create Functionality That have similar Page UI
    public class UpsertModel : PageModel
    {
        private ApplicationDbContext _db;
        [BindProperty]
        public Book Book { get; set; }


        public UpsertModel(ApplicationDbContext db)
        {
            this._db = db;
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            Book = new Book();

            if (id == null)
            {
                // return same page to Create
                return Page();  
            }

            // for update
            Book = await _db.Book.FirstOrDefaultAsync(x => x.Id == id);
            if (Book == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                if (Book.Id == 0)
                {
                    _db.Book.Add(Book);
                }
                else
                {
                    _db.Book.Update(Book);  // this method for updating every property of the book object  

                    // this way for only update one or two or specific properties of the object
                    //var BookFromDb = await _db.Book.FindAsync(Book.Id);
                    //BookFromDb.Name = Book.Name;
                    //BookFromDb.Author = Book.Author;
                    //BookFromDb.ISBN = Book.ISBN;
                }

                await _db.SaveChangesAsync();

                return RedirectToPage("Index");
            }
            return RedirectToPage();
        }
    }
}