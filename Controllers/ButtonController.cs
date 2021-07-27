using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ButtonGrid.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ButtonGrid.Controllers
{
    public class ButtonController : Controller
    {
        static List<ButtonModel> buttons = new List<ButtonModel>();
        Random random = new Random();
        const int GRID_SIZE = 25;
        public IActionResult Index()
        {
            // empty the list when the page loads.
            buttons = new List<ButtonModel>();
                for (int i = 0; i < GRID_SIZE; i++)
                {
                    buttons.Add(new ButtonModel { Id = i, ButtonState = random.Next(4) });
                }
            return View("Index", buttons);
        }

       

        public IActionResult RightClickShowOneButton(int buttonNumber)
        {
           
            // add one to the button state. if > 4 then reset to 0
            buttons.ElementAt(buttonNumber).ButtonState = 0;

            // 1. Render a message
            string buttonString = RenderRazorViewToString(this, "ShowOneButton", buttons.ElementAt(buttonNumber));
            // 2. Generate a win loss string based on the state of the buttons array
            bool DidIWinYet = true;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons.ElementAt(i).ButtonState != buttons.ElementAt(0).ButtonState)
                    DidIWinYet = false;
            }
            string messageString = "";
            if (DidIWinYet == true)
            {
                messageString = "<p>Contratulations. All buttons are of the same colour</p>";
            }
            else messageString = "<p>Not all the buttons are the same colour yet. see if you can make them match</P>";

            // 3. Assembly a JSON string that has two parts (button string html and win loss message)

            var package = new { part1 = buttonString, part2 = messageString };
            // 4. Send the JSON result
             return Json(package);
            // 5. in the site.js file, the data will have to be interpreted as two pieces of data instead of one.

            return PartialView("ShowOneButton",buttons.ElementAt(buttonNumber));
        }

        public IActionResult HandleButtonClick(string buttonNumber)
        {
            // convert from string to integer
            int btn = int.Parse(buttonNumber);
            // add one to the button state. if greater than 4 returns to 0
            buttons.ElementAt(btn).ButtonState = (buttons.ElementAt(btn).ButtonState + 1) % 4;
            // re-display the button
            return View("Index", buttons);
        }

      
      
        public IActionResult ShowOneButton(int buttonNumber)
        { 
               // add one to the button state. if > 4, then reset to 0,

              buttons.ElementAt(buttonNumber).ButtonState = (buttons.ElementAt(buttonNumber).ButtonState + 1) % 4;

              // 1. Render a message
              string buttonString = RenderRazorViewToString(this, "ShowOneButton", buttons.ElementAt(buttonNumber));
              // 2. Generate a win loss string based on the state of the buttons array
              bool DidIWinYet = true;
              for(int i = 0; i < buttons.Count; i++)
              {
                  if (buttons.ElementAt(i).ButtonState != buttons.ElementAt(0).ButtonState)
                      DidIWinYet = false;
              }
              string messageString = " ";
              if (DidIWinYet == true)
              {
                  messageString = "<p>Contratulations. All buttons are of the same colour</p>";
              }
              else
              {
                messageString = "<p>Not all the buttons are the same colour yet. see if you can make them match</P>";
              }
              // 3. Assembly a JSON string that has two parts (button string html and win loss message)

              var package = new { part1 = buttonString, part2 = messageString };
            // 4. Send the JSON result
             return Json(package);

            // 5. in the site.js file, the data will have to be interpreted as two pieces of data instead of one.

            return PartialView("ShowOneButton",buttons.ElementAt(buttonNumber));
          
        }

        public static string RenderRazorViewToString(Controller controller, string viewName, object model = null)
          {
              controller.ViewData.Model = model;
              using (var sw = new StringWriter())
              {
                  IViewEngine viewEngine =
                      controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as
                          ICompositeViewEngine;
                  ViewEngineResult viewResult = viewEngine.FindView(controller.ControllerContext, viewName, false);

                  ViewContext viewContext = new ViewContext(
                      controller.ControllerContext,
                      viewResult.View,
                      controller.ViewData,
                      controller.TempData,
                      sw,
                      new HtmlHelperOptions()
                  );
                  viewResult.View.RenderAsync(viewContext);
                  return sw.GetStringBuilder().ToString();
              }
          }
          
    }
}
