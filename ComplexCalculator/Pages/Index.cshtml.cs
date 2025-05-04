using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ComplexCalculator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<SelectListItem> Operations { get; } = new List<SelectListItem>
            {
                new SelectListItem{Value="add", Text ="+"},
                new SelectListItem{Value="subtract", Text ="-"},
                new SelectListItem{Value="multiply", Text ="*"},
                new SelectListItem{Value="divide", Text ="/"}
            };
        [BindProperty]
        public string Operation { get; set; }
        [BindProperty]
        public string Num1 { get; set; } = "0";
        [BindProperty]
        public string Num2 { get; set; } = "0";
        [BindProperty]
        public string Format { get; set; }

        private Complex _complex; // Add a private instance of Complex

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger; // Use the instance to call Format
        }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            switch (Operation)
            {
                case "add":
                    ViewData["Result"] = (Complex.Parse(Num1) + Complex.Parse(Num2)).ToString(Format);
                    break;
                case "subtract":
                    ViewData["Result"] = (Complex.Parse(Num1) - Complex.Parse(Num2)).ToString(Format);
                    break;
                case "multiply":
                    ViewData["Result"] = (Complex.Parse(Num1) * Complex.Parse(Num2)).ToString(Format);
                    break;
                case "divide":
                    ViewData["Result"] = (Complex.Parse(Num1) / Complex.Parse(Num2)).ToString(Format);
                    break;
            }
        }
    }
}
