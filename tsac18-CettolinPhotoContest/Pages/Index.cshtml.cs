using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using tsac18_CettolinPhotoContest.data;
using tsac18_CettolinPhotoContest.data.Models;

namespace tsac18_CettolinPhotoContest.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IDataAccess _data;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(IDataAccess data, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _data = data;
        }

        public class PostModel
        {
            [DataType(DataType.Text)]
            public int Vote { get; set; }
        }

        [BindProperty]
        public PostModel Input { get; set; }

        public IEnumerable<Photo> list { get; set; }        
        public void OnGet()
        {
            string UserId = _userManager.GetUserId(User);
            list = _data.GetPhoto(UserId);
        }
        public IActionResult OnPostVote(int id) //try catch con redirect error-
        {
            var UserId = _userManager.GetUserId(User);
            _data.Vote(id,UserId,Input.Vote);
            return RedirectToPage("/index");
        }
    }
}
