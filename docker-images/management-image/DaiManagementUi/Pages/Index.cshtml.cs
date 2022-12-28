using Docker.DotNet.Models;
using Docker.DotNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace DaiManagementUi.Pages
{
    public class IndexModel : PageModel
    {
        private IActionResult DoAction(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.ToString();
            }
            return Redirect("/");
        }

        public IActionResult OnGetStart(string id)
            => DoAction(() => Docker.StartContainer(id));

        public IActionResult OnGetStop(string id)
            => DoAction(() => Docker.StopContainer(id));

        public IActionResult OnPost(string tag)
            => DoAction(() => Docker.CreateContainer(tag));

        public IActionResult OnGetDelete(string id)
            => DoAction(() => Docker.DeleteContainer(id));
    }
}